using System.Collections.Generic;

namespace TransactionsManager.API.Models
{
    /// <summary>
    /// Error Model
    /// </summary>
    public class ErrorStatusModel
    {
        public bool Status { get; private set; }

        public IList<string> Errors { get; private set; }

        public ErrorStatusModel(bool status = false, IList<string> errors = null)
        {
            Status = status;
            Errors = errors;
        }

        public static ErrorStatusModel CreateErrorsModel(IList<string> errors) => new ErrorStatusModel(errors: errors);

        public static ErrorStatusModel CreateErrorModel(string error)
        {
            IList<string> errors = new List<string> { error };
            return new ErrorStatusModel(errors: errors);
        }
    }
}
