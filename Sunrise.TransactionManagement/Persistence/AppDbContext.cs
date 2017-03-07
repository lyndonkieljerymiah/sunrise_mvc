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
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Reconcile> Reconcile { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("stm");

            modelBuilder.Entity<Contract>().ToTable("Contract");
            modelBuilder.Entity<Bill>().ToTable("Bill");

            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Payment>().Ignore(p => p.IsMarkDeleted);

            modelBuilder.Entity<Reconcile>().ToTable("Reconcile");
            modelBuilder.Entity<Reconcile>().Ignore(p => p.IsMarkDeleted);
            
            modelBuilder.Entity<Contract>()
                .Property(t => t.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_TransactionCode")));

            modelBuilder.Entity<Contract>()
                .HasOptional(t => t.Terminate)
                .WithRequired(t => t.Transaction);

            modelBuilder.Entity<Bill>()
                .HasRequired(b => b.Contract)
                .WithMany();
            
            modelBuilder.Entity<Bill>()
                 .HasMany(b => b.Payments)
                 .WithRequired(p => p.Bill);

            modelBuilder.Entity<Bill>()
                .HasMany(b => b.Reconciles)
                .WithRequired(r => r.Bill);


            modelBuilder.Entity<Reconcile>().Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }

    }
}
