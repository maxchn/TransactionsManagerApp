namespace TransactionsManager.API.Models
{
    /// <summary>
    /// Successful Status Model
    /// </summary>
    public class SuccessStatusModel
    {
        public bool Status { get; private set; }

        public SuccessStatusModel(bool status = true)
        {
            Status = status;
        }

        public static SuccessStatusModel CreateSuccessStatus() => new SuccessStatusModel();
    }
}
