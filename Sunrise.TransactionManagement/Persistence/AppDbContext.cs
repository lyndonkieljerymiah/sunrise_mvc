using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.TransactionManagement.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("stm");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Payment>().ToTable("Payment");

            modelBuilder.Entity<Transaction>()
                .HasMany(t => t.Payments)
                .WithRequired();


        }

    }
}
