using MediatR;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TransactionsManager.API.DbEntities;
using TransactionsManager.API.Interfaces;
using TransactionsManager.API.RequestModels.QueryRequestModels;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.Handlers.QueryHandlers
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsRequestModel, GetTransactionsResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetTransactionsResponseModel> Handle(GetTransactionsRequestModel request, CancellationToken cancellationToken)
        {
            var response = new GetTransactionsResponseModel { IsSuccess = false };

            try
            {
                Expression<Func<Transaction, bool>> predicate = null;

                if (!string.IsNullOrEmpty(request.Status) && !string.IsNullOrEmpty(request.Type))
                {
                    predicate = x => x.Status.ToLower().Equals(request.Status.ToLower()) && x.Type.ToLower().Equals(request.Type.ToLower());
                }
                else if (!string.IsNullOrEmpty(request.Status))
                {
                    predicate = x => x.Status.ToLower().Equals(request.Status.ToLower());
                }
                else if (!string.IsNullOrEmpty(request.Type))
                {
                    predicate = x => x.Type.ToLower().Equals(request.Type.ToLower());
                }

                if (predicate is null)
                {
                    response.Transactions = (await _unitOfWork.Transaction.FindAll()).ToList();
                }
                else
                {
                    response.Transactions = (await _unitOfWork.Transaction.Find(predicate)).ToList();
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return response;
        }
    }
}
