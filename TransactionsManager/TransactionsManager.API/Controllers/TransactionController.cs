using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using Microsoft.Extensions.Hosting;
using TransactionsManager.API.DbEntities;
using TransactionsManager.API.Models;
using TransactionsManager.API.RequestModels.CommandRequestModels;
using TransactionsManager.API.RequestModels.QueryRequestModels;
using TransactionsManager.API.Utils;

namespace TransactionsManager.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    [EnableCors("AllowSpecificOrigin")]
    public class TransactionController : CoreController
    {
        private const string ImportFileName = "ImportData.xlsx";
        private const string ContentFolderName = "wwwroot";

        private readonly string[] _allowedExtensions = { ".csv" };
        private readonly IMediator _mediator;
        private readonly IHostEnvironment _environment;

        public TransactionController(ILogger<TransactionController> logger,
            IMediator mediator,
            IHostEnvironment environment
        ) : base(logger)
        {
            _mediator = mediator;
            _environment = environment;
        }

        /// <summary>
        /// Importing data
        /// </summary>
        /// <param name="file">File with import data</param>
        /// <response code="200">Returned if the data import was successful</response>
        /// <response code="400">Returned if unknown error occurred</response>
        /// <response code="422">Returned if input data is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessStatusModel), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        [ProducesResponseType(typeof(ErrorStatusModel), 422)]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                if (IsEmptyFile(file))
                    throw new ArgumentException("The file cannot be empty!");

                if (!IsAllowedExtension(file))
                    throw new ArgumentException("The file has an not allowed extension!");

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Transaction>();

                    var response = await _mediator.Send(new AddOrUpdateTransactionsRequestModel
                    {
                        Transactions = records
                    });

                    if (response.IsSuccess)
                    {
                        actionResult = Ok(SuccessStatusModel.CreateSuccessStatus());
                    }
                }
            }
            catch (ArgumentException ex)
            {
                actionResult = UnprocessableEntity(ErrorStatusModel.CreateErrorModel(ex.Message));
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        /// <summary>
        /// Exporting data
        /// </summary>
        /// <param name="requestModel">Model input data</param>
        /// <response code="200">Returned if the data export was successful</response>
        /// <response code="400">Returned if unknown error occurred</response>
        [Produces("application/ms-excel")]
        [HttpGet]
        [ProducesResponseType(typeof(FileResult), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        public async Task<IActionResult> Export([FromQuery] GetTransactionsRequestModel requestModel)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                var response = await _mediator.Send(requestModel);

                if (response.IsSuccess)
                {
                    Workbook wb = new Workbook();

                    Worksheet sheet = wb.Worksheets[0];

                    MakeDefaultInfoHeaders(sheet);

                    IList<Transaction> transactions = response.Transactions.ToList();
                    const int rowOffset = 2;

                    for (int i = 0; i < transactions.Count; i++)
                    {
                        var currentRow = i + rowOffset;

                        Cell idHeaderCell = sheet.Cells[$"A{currentRow}"];
                        idHeaderCell.PutValue(transactions[i].TransactionId);

                        Cell statusHeaderCell = sheet.Cells[$"B{currentRow}"];
                        statusHeaderCell.PutValue(transactions[i].Status);

                        Cell typeHeaderCell = sheet.Cells[$"C{currentRow}"];
                        typeHeaderCell.PutValue(transactions[i].Type);

                        Cell clientNameHeaderCell = sheet.Cells[$"D{currentRow}"];
                        clientNameHeaderCell.PutValue(transactions[i].ClientName);

                        Cell amountHeaderCell = sheet.Cells[$"E{currentRow}"];
                        amountHeaderCell.PutValue(transactions[i].Amount);
                    }

                    string filePath = Path.Combine(_environment.ContentRootPath, ContentFolderName, ImportFileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    wb.Save(filePath, SaveFormat.Xlsx);

                    actionResult = await FileUtils.GetFileContent(filePath, Path.GetFileName(filePath), HttpContext, "application/ms-excel");
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        /// <summary>
        /// Updating the transaction by ID
        /// </summary>
        /// <param name="requestModel">Model input data</param>
        /// <response code="200">Returned if transaction item was updated successfully</response>
        /// <response code="400">Returned if unknown error occurred</response>
        /// <response code="422">Returned if input data is invalid</response>
        [HttpPut]
        [ProducesResponseType(typeof(SuccessStatusModel), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        [ProducesResponseType(typeof(ErrorStatusModel), 422)]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionRequestModel requestModel)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                var response = await _mediator.Send(requestModel);

                if (response.IsSuccess)
                {
                    actionResult = Ok(SuccessStatusModel.CreateSuccessStatus());
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        /// <summary>
        /// Deleting the transaction by ID
        /// </summary>
        /// <param name="requestModel">Model input data</param>
        /// <response code="200">Returned if transaction item was deleted successfully</response>
        /// <response code="400">Returned if unknown error occurred</response>
        [HttpDelete]
        [ProducesResponseType(typeof(SuccessStatusModel), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        public async Task<IActionResult> Delete([FromQuery] RemoveTransactionRequestModel requestModel)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                var response = await _mediator.Send(requestModel);

                if (response.IsSuccess)
                {
                    actionResult = Ok(SuccessStatusModel.CreateSuccessStatus());
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        /// <summary>
        /// Getting a part of data by the input parameters
        /// </summary>
        /// <param name="requestModel">Model input data</param>
        /// <response code="200">Returned if all transactions were successfully is founded</response>
        /// <response code="400">Returned if unknown error occurred</response>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessStatusWithDataModel<IList<Transaction>>), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        public async Task<IActionResult> GetPart([FromQuery] GetTransactionsPartRequestModel requestModel)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                var response = await _mediator.Send(requestModel);

                if (response.IsSuccess)
                {
                    actionResult = Ok(SuccessStatusWithDataModel<IList<Transaction>>.CreateSuccessStatus(response.Transactions));
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        /// <summary>
        /// Getting the number of pages for the specified number of elements on the page
        /// </summary>
        /// <param name="requestModel">Model input data</param>
        /// <response code="200">Returned if the number of pages was counted successfully</response>
        /// <response code="400">Returned if unknown error occurred</response>
        [HttpGet]
        [ProducesResponseType(typeof(SuccessStatusWithDataModel<int>), 200)]
        [ProducesResponseType(typeof(ErrorStatusModel), 400)]
        public async Task<IActionResult> GetNumberOfPages([FromQuery] GetTransactionsNumberOfPagesRequestModel requestModel)
        {
            var actionResult = CreateDefaultActionResult();

            try
            {
                var response = await _mediator.Send(requestModel);

                if (response.IsSuccess)
                {
                    actionResult = Ok(SuccessStatusWithDataModel<int>.CreateSuccessStatus(response.NumberOfPages));
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(ErrorStatusModel.CreateErrorModel(ex.Message));
            }

            return actionResult;
        }

        private bool IsEmptyFile(IFormFile file) => file is null;

        private bool IsAllowedExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return _allowedExtensions.Contains(fileExtension);
        }

        private void MakeDefaultInfoHeaders(Worksheet sheet)
        {
            Cell idHeaderCell = sheet.Cells["A1"];
            idHeaderCell.PutValue("ID");

            Cell statusHeaderCell = sheet.Cells["B1"];
            statusHeaderCell.PutValue("Status");

            Cell typeHeaderCell = sheet.Cells["C1"];
            typeHeaderCell.PutValue("Type");

            Cell clientNameHeaderCell = sheet.Cells["D1"];
            clientNameHeaderCell.PutValue("Client name");

            Cell amountHeaderCell = sheet.Cells["E1"];
            amountHeaderCell.PutValue("Amount");
        }
    }
}
