using System.Collections.Generic;
using TransactionsManager.API.DbEntities;

namespace TransactionsManager.API.ResponseModels.QueryResponseModels
{
    public class GetTransactionsResponseModel
    {
        public bool IsSuccess { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
