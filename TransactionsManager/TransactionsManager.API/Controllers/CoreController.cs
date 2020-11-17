using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using TransactionsManager.API.Models;
using TransactionsManager.API.Utils;

namespace TransactionsManager.API.Controllers
{
    public class CoreController : ControllerBase
    {
        protected readonly ILogger<CoreController> logger;

        public CoreController(ILogger<CoreController> logger)
        {
            this.logger = logger;
        }

        protected IList<string> GetModelErrors()
        {
            return ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }

        protected IList<string> GetIdentityErrors(IdentityResult identityResult)
        {
            return identityResult.Errors.Select(e => e.Description).ToList();
        }

        protected IActionResult CreateUnprocessableEntityResult(IdentityResult result)
        {
            IList<string> errors = GetIdentityErrors(result);
            return UnprocessableEntity(errors);
        }

        protected IActionResult CreateDefaultActionResult()
        {
            return BadRequest(ErrorStatusModel.CreateErrorModel(StatusErrorType.Unknown.GetStringValue()));
        }
    }
}
