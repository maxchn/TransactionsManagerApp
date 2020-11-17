using MediatR;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.RequestModels.CommandRequestModels
{
    public class UpdateTransactionRequestModel : IRequest<UpdateTransactionResponseModel>
    {
        public int TransactionId { get; set; }

        public string Status { get; set; }
    }
}
