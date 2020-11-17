using MediatR;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.RequestModels.QueryRequestModels
{
    public class GetTransactionsRequestModel : IRequest<GetTransactionsResponseModel>
    {
        public string Status { get; set; }

        public string Type { get; set; }
    }
}