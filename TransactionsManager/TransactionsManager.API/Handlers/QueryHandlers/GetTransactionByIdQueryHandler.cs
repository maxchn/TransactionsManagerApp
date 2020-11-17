using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TransactionsManager.API.Interfaces;
using TransactionsManager.API.RequestModels.QueryRequestModels;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.Handlers.QueryHandlers
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdRequestModel, GetTransactionByIdResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetTransactionByIdResponseModel> Handle(GetTransactionByIdRequestModel request, CancellationToken cancellationToken)
        {
            var response = new GetTransactionByIdResponseModel { IsSuccess = false };

            try
            {
                var transaction = await _unitOfWork.Transaction.FindById(request.TransactionId);

                response.Transaction = transaction;
                response.IsSuccess = transaction != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return response;
        }
    }
}
