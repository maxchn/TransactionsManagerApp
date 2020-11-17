using MediatR;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.RequestModels.QueryRequestModels
{
    public class GetTransactionsNumberOfPagesRequestModel : IRequest<GetTransactionsNumberOfPagesResponseModel>
    {
        public int PageSize { get; set; }
    }
}
