using Sunrise.TransactionManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.TransactionManagement.Persistence
{
    public class ReferenceDbContext : BaseDbContext 
    {

        public DbSet<TenantView> Tenants { get; set; }
        public DbSet<TransactionView> Transactions { get; set; }
        public DbSet<VillaView> Villas { get; set; }
        public DbSet<PaymentView> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantView>().ToTable("TenantView", "dbo");
            modelBuilder.Entity<TransactionView>().ToTable("TransactionView", "dbo");
            modelBuilder.Entity<VillaView>().ToTable("VillaView", "dbo");
            modelBuilder.Entity<VillaGalleryView>().ToTable("VillaGalleryView", "dbo");
            modelBuilder.Entity<PaymentView>().ToTable("PaymentView", "dbo");


            modelBuilder.Entity<TransactionView>().HasKey(t => t.Id);
            modelBuilder.Entity<VillaView>().HasKey(v => v.Id);

            modelBuilder.Entity<TenantView>().HasKey(v => v.Id);

            modelBuilder.Entity<PaymentView>()
                .HasRequired<TransactionView>(p => p.Transaction)
                .WithMany(tr => tr.Payments)
                .HasForeignKey(p => p.TransactionId);
            
            modelBuilder.Entity<TransactionView>()
                .HasRequired(t => t.Tenant)
                .WithMany()
                .HasForeignKey(t => t.TenantId);

            modelBuilder.Entity<TransactionView>()
                .HasRequired(t => t.Villa)
                .WithMany()
                .HasForeignKey(t => t.VillaId);

            modelBuilder.Entity<VillaGalleryView>()
                .HasRequired(v => v.Villa)
                .WithMany(v=> v.Galleries)
                .HasForeignKey(v => v.VillaId);

        }
    }
}
