using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TransactionsManager.API.DbContexts;
using TransactionsManager.API.DbEntities;
using TransactionsManager.API.Interfaces;

namespace TransactionsManager.API.Repositories
{
    public class MsSqlTransactionRepository : ITransactionRepository
    {
        private MsSqlDbContext _context;

        public MsSqlTransactionRepository(MsSqlDbContext context)
        {
            _context = context;
        }

        public async Task Delete(object id)
        {
            var item = await FindById(id);

            if (item is null)
                throw new ArgumentException("Position item with the specified ID not found!!!");

            _context.Transactions.Remove(item);
        }

        public async Task<IEnumerable<Transaction>> Find(Expression<Func<Transaction, bool>> predicate)
        {
            return await _context.Transactions
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> FindAll()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> FindById(object id)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(x => x.TransactionId == (int)id);
        }

        public async Task<int> GetCount()
        {
            return (await _context.Transactions.ToListAsync()).Count();
        }

        public async Task<IList<Transaction>> GetPart(Expression<Func<Transaction, bool>> predicate, int limit, int offset)
        {
            IList<Transaction> transactions;

            if (predicate is null)
            {
                transactions = await _context.Transactions
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            }
            else
            {
                transactions = await _context.Transactions
                .Where(predicate)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            }

            return transactions;
        }

        public async Task Insert(Transaction entity)
        {
            await _context.Transactions.AddAsync(entity);
        }

        public async Task Update(Transaction entityToUpdate)
        {
            var entity = await FindById(entityToUpdate.TransactionId);

            if (entity is null)
                throw new ArgumentException("Transaction with the specified ID not found!!!");
        }
    }
}
