using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionsManager.API.DbEntities
{
    /// <summary>
    /// Transaction Entity
    /// </summary>
    public class Transaction
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int TransactionId { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string ClientName { get; set; }

        public string Amount { get; set; }

        public override string ToString()
        {
            return $"TransactionId: {TransactionId}Status: {Status} Type: {Type} ClientName: {ClientName} Amount: {Amount}";
        }
    }
}
