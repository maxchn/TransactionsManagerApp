using Microsoft.EntityFrameworkCore;
using TransactionsManager.API.DbEntities;

namespace TransactionsManager.API.DbContexts
{
    public class MsSqlDbContext : DbContext
    {
        public MsSqlDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
