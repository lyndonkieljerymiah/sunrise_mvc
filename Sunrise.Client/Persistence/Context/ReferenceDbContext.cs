using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.DTO;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.EntityConfigurations;

namespace Sunrise.Client.Persistence.Context
{
    public class ReferenceDbContext : DbContext
    {

        public DbSet<VillaDTO> VillaDtos { get; set; }
        public DbSet<SalesTransactionDTO> SalesTransactionDtos { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<PaymentDTO> Payments { get; set; }
        public ReferenceDbContext() : base("DbConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VillaDTO>().ToTable("VillasView", "dbo");
            modelBuilder.Entity<SalesTransactionDTO>().ToTable("SalesTransactionsView", "dbo");
            modelBuilder.Entity<PaymentDTO>().ToTable("PaymentView", "dbo");
            modelBuilder.Entity<Tenant>().ToTable("Tenants", "dbo");
            modelBuilder.Entity<Company>().ToTable("Companies", "dbo");
            modelBuilder.Entity<Individual>().ToTable("Individuals", "dbo");
            

            modelBuilder.Entity<Company>().HasKey(c => c.TenantId);
            modelBuilder.Entity<Individual>().HasKey(c => c.TenantId);
            modelBuilder.Entity<Tenant>().HasOptional(t => t.Individual).WithRequired();
            modelBuilder.Entity<Tenant>().HasOptional(t => t.Company).WithRequired();

            modelBuilder.Entity<SalesTransactionDTO>()
                .HasMany(s => s.Payments)
                .WithRequired()
                .HasForeignKey(m => m.SalesTransactionId);


        }
    }
}
