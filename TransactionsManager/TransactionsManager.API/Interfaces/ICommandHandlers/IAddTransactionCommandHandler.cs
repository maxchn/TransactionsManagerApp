using TransactionsManager.API.RequestModels.CommandRequestModels;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.Interfaces.ICommandHandlers
{
    public interface IAddTransactionCommandHandler
    {
        AddTransactionResponseModel AddTransaction(AddOrUpdateTransactionsRequestModel model);
    }
}
