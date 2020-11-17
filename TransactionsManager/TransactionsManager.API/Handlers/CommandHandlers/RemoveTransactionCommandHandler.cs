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
    public class RemoveTransactionCommandHandler : IRequestHandler<RemoveTransactionRequestModel, RemoveTransactionResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveTransactionResponseModel> Handle(RemoveTransactionRequestModel request, CancellationToken cancellationToken)
        {
            var response = new RemoveTransactionResponseModel { IsSuccess = true };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.Transaction.Delete(request.TransactionId);
                await _unitOfWork.SaveChangesAsync();
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
