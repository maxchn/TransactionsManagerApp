using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TransactionsManager.API.DbEntities;

namespace TransactionsManager.API.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IList<Transaction>> GetPart(Expression<Func<Transaction, bool>> predicate, int limit, int offset);

        Task<int> GetCount();
    }
}
