using MediatR;
using System.Collections.Generic;
using TransactionsManager.API.DbEntities;
using TransactionsManager.API.ResponseModels.CommandResponseModels;

namespace TransactionsManager.API.RequestModels.CommandRequestModels
{
    public class AddOrUpdateTransactionsRequestModel : IRequest<AddTransactionResponseModel>
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
