using TransactionsManager.API.DbEntities;

namespace TransactionsManager.API.ResponseModels.QueryResponseModels
{
    public class GetTransactionByIdResponseModel
    {
        public bool IsSuccess { get; set; }

        public Transaction Transaction { get; set; }
    }
}
