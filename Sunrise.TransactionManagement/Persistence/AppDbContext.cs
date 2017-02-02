using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.TransactionManagement.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        public DbSet<Contract> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("stm");
            modelBuilder.Entity<Contract>().ToTable("Transaction");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            
            modelBuilder.Entity<Contract>()
                .Property(t => t.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_TransactionCode")));

            modelBuilder.Entity<Contract>()
                .HasOptional(t => t.Terminate)
                .WithRequired(t => t.Transaction);

            modelBuilder.Entity<Contract>()
                .HasMany(t => t.Payments)
                .WithRequired(p => p.Transaction);


        }

    }
}
