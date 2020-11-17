using TransactionsManager.API.Models;

namespace TransactionsManager.API.Utils
{
    public static class AppExtensions
    {
        public static string GetStringValue(this StatusErrorType errorType)
        {
            return errorType switch
            {
                StatusErrorType.InvalidInputParams => "Invalid input parameters",
                StatusErrorType.ItemCannotBeIdentified => "Item could not be identified",
                StatusErrorType.Unknown => "Unknown error",
                StatusErrorType.Duplicate => "Duplicate meaning",
                StatusErrorType.DeniedTwoOpenPositions => "It is forbidden to have more than one open position",
                _ => "Unknown error"
            };
        }
    }
}
