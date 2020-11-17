using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TransactionsManager.API.DbEntities;
using TransactionsManager.API.Interfaces;
using TransactionsManager.API.RequestModels.QueryRequestModels;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.Handlers.QueryHandlers
{
    public class GetTransactionsPartQueryHandler : IRequestHandler<GetTransactionsPartRequestModel, GetTransactionsResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsPartQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetTransactionsResponseModel> Handle(GetTransactionsPartRequestModel request, CancellationToken cancellationToken)
        {
            var response = new GetTransactionsResponseModel { IsSuccess = false };

            try
            {
                Expression<Func<Transaction, bool>> predicate;

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
                else
                {
                    predicate = null;
                }

                IList<Transaction> transactions = await _unitOfWork.Transaction.GetPart(predicate, request.PageSize, (request.Page - 1) * request.PageSize);

                response.IsSuccess = true;
                response.Transactions = transactions;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return response;
        }
    }
}
