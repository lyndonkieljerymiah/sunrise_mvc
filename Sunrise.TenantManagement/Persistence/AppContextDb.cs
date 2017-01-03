using Sunrise.TenantManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.TenantManagement.Persistence
{
    public class AppContextDb : BaseDbContext
    {   
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.HasDefaultSchema("tm");
            modelBuilder.Entity<Tenant>().ToTable("Tenant");
            modelBuilder.Entity<Individual>().ToTable("Individual");
            modelBuilder.Entity<Company>().ToTable("Company");

            /********************************
             * configuration
             ********************************/
            modelBuilder.Entity<Tenant>()
               .Property(t => t.Code)
               .HasMaxLength(50)
               .HasColumnAnnotation("Index", 
               new IndexAnnotation(new IndexAttribute("IX_TenantCode")));

            modelBuilder.Entity<Individual>()
                .HasKey(i => i.TenantId);

            modelBuilder.Entity<Company>()
                .HasKey(i => i.TenantId);

            modelBuilder.Entity<Tenant>()
                .HasOptional(t => t.Individual)
                .WithRequired(i => i.Tenant)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Tenant>()
                .HasOptional(t => t.Company)
                .WithRequired(c => c.Tenant)
                .WillCascadeOnDelete();


        }
    }
}
