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
    public class GetTransactionsNumberOfPagesQueryHandler : IRequestHandler<GetTransactionsNumberOfPagesRequestModel, GetTransactionsNumberOfPagesResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionsNumberOfPagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetTransactionsNumberOfPagesResponseModel> Handle(GetTransactionsNumberOfPagesRequestModel request, CancellationToken cancellationToken)
        {
            var response = new GetTransactionsNumberOfPagesResponseModel { IsSuccess = false };

            try
            {
                var count = await _unitOfWork.Transaction.GetCount();

                response.IsSuccess = true;
                response.NumberOfPages = (int)Math.Ceiling(count / (double)request.PageSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return response;
        }
    }
}
