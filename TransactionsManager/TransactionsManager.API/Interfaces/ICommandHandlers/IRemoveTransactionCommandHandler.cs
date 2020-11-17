using TransactionsManager.API.RequestModels.CommandRequestModels;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.Interfaces.ICommandHandlers
{
    public interface IRemoveTransactionCommandHandler
    {
        RemoveTransactionResponseModel RemoveTransaction(RemoveTransactionRequestModel model);
    }
}
