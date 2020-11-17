using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using TransactionsManager.API.DbContexts;
using TransactionsManager.API.Interfaces;
using TransactionsManager.API.Repositories;

namespace TransactionsManager.API.UnitOfWork
{
    public class MsSqlUnitOfWork : IUnitOfWork
    {
        private MsSqlDbContext _context;
        private IDbContextTransaction _transaction;

        private MsSqlTransactionRepository _transactionRepository;

        public MsSqlUnitOfWork(MsSqlDbContext context)
        {
            _context = context;
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_transactionRepository is null)
                    _transactionRepository = new MsSqlTransactionRepository(_context);

                return _transactionRepository;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void DetachEntity(object entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void EnabledDisabledAutoDetectChanges(bool enable)
        {
            _context.ChangeTracker.QueryTrackingBehavior = enable ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
