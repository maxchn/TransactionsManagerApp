using MediatR;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.RequestModels.CommandRequestModels
{
    public class RemoveTransactionRequestModel : IRequest<RemoveTransactionResponseModel>
    {
        public int TransactionId { get; set; }
    }
}
