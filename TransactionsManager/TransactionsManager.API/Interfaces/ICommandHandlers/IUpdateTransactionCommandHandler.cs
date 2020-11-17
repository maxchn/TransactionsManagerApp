using TransactionsManager.API.RequestModels.CommandRequestModels;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.Interfaces.ICommandHandlers
{
    public interface IUpdateTransactionCommandHandler
    {
        UpdateTransactionResponseModel UpdateTransaction(UpdateTransactionRequestModel model);
    }
}
