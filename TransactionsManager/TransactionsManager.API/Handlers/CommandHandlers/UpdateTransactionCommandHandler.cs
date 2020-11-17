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
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionRequestModel, UpdateTransactionResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateTransactionResponseModel> Handle(UpdateTransactionRequestModel request, CancellationToken cancellationToken)
        {
            var response = new UpdateTransactionResponseModel { IsSuccess = true };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var transaction = await _unitOfWork.Transaction.FindById(request.TransactionId);
                transaction.Status = request.Status;

                await _unitOfWork.Transaction.Update(transaction);
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
