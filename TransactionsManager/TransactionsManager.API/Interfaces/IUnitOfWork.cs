using System.Threading.Tasks;

namespace TransactionsManager.API.Interfaces
{
    /// <summary>
    /// Manager repositories
    /// </summary>
    public interface IUnitOfWork
    {
        ITransactionRepository Transaction { get; }

        /// <summary>
        /// Transaction Creation
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Transaction commit (confirmation)
        /// </summary>
        void Commit();

        /// <summary>
        /// Transaction cancellation
        /// </summary>
        void Rollback();

        /// <summary>
        /// Saving current changes
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();

        void DetachEntity(object entity);

        /// <summary>
        /// Enable/Disable entity detect changes
        /// </summary>
        /// <returns></returns>
        void EnabledDisabledAutoDetectChanges(bool enable);
    }
}
