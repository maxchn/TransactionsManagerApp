namespace TransactionsManager.API.Models
{
    public enum StatusErrorType
    {
        InvalidInputParams,
        ItemCannotBeIdentified,
        Unknown,
        Duplicate,
        DeniedTwoOpenPositions
    }
}
