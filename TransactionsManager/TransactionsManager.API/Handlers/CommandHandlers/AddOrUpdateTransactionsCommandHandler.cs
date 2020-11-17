using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TransactionsManager.API.Interfaces;
using TransactionsManager.API.RequestModels.CommandRequestModels;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.Handlers.CommandHandlers
{
    public class AddOrUpdateTransactionsCommandHandler : IRequestHandler<AddOrUpdateTransactionsRequestModel, AddTransactionResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOrUpdateTransactionsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddTransactionResponseModel> Handle(AddOrUpdateTransactionsRequestModel request, CancellationToken cancellationToken)
        {
            var response = new AddTransactionResponseModel() { IsSuccess = true };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var transaction in request.Transactions)
                {
                    var searchTransaction = await _unitOfWork.Transaction.FindById(transaction.TransactionId);

                    if (searchTransaction is null)
                    {
                        await _unitOfWork.Transaction.Insert(transaction);
                    }
                    else
                    {
                        searchTransaction.Status = transaction.Status;

                        await _unitOfWork.Transaction.Update(searchTransaction);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
                
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                _unitOfWork.Rollback();

                response.IsSuccess = false;
            }

            return response;
        }
    }
}
