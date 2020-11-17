using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TransactionsManager.API.Interfaces
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity">Entity Type</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Search for items matching a specified predicate
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        // Search for an item by ID
        Task<TEntity> FindById(object id);

        // Search all items
        Task<IEnumerable<TEntity>> FindAll();

        // Insert a new entity
        Task Insert(TEntity entity);

        // Entity Update
        Task Update(TEntity entityToUpdate);

        // Deleting an entity by ID
        Task Delete(object id);
    }
}
