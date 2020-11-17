using MediatR;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.RequestModels.QueryRequestModels
{
    public class GetTransactionsPartRequestModel : IRequest<GetTransactionsResponseModel>
    {
        public string Status { get; set; }

        public string Type { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
