using MediatR;
using TransactionsManager.API.ResponseModels.QueryResponseModels;

namespace TransactionsManager.API.RequestModels.QueryRequestModels
{
    public class GetTransactionByIdRequestModel : IRequest<GetTransactionByIdResponseModel>
    {
        public int TransactionId { get; set; }
    }
}
