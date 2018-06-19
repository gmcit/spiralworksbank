

using Microsoft.EntityFrameworkCore;
using SpiralWorks.Model;

namespace SpiralWorks.Data.Ef6
{
    public class SpiralWorksDBContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UniqueNumber> UniqueNumbers { get; set; }
        public SpiralWorksDBContext(DbContextOptions<SpiralWorksDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Transaction>().Property(p => p.RowVersion).IsConcurrencyToken();
            builder.Entity<Account>().Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}
