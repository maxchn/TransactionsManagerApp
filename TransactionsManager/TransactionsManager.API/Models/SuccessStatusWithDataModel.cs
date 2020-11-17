namespace TransactionsManager.API.Models
{
    /// <summary>
    /// Successful Status With Data Model
    /// </summary>
    public class SuccessStatusWithDataModel<T>
    {
        public bool Status { get; private set; }

        public T Data { get; set; }

        public SuccessStatusWithDataModel(T data, bool status = true)
        {
            Status = status;
            Data = data;
        }

        public static SuccessStatusWithDataModel<T> CreateSuccessStatus(T data) => new SuccessStatusWithDataModel<T>(data);
    }
}
