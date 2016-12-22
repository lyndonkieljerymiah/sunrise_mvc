using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.Models.Identity;
using Sunrise.Client.Persistence.EntityConfigurations;

namespace Sunrise.Client.Persistence
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<SalesTransaction> Sales {get; set;}
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Selection> Selections { get; set; }
        
        public AppDbContext()
            : base("DbConnection", throwIfV1Schema: false)
        {

        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");

            modelBuilder.Entity<Tenant>().ToTable("Tenant");
            modelBuilder.Entity<Villa>().ToTable("Villa");
            modelBuilder.Entity<Selection>().ToTable("Selection");

            modelBuilder.Configurations.Add(new IndividualConfig());
            modelBuilder.Configurations.Add(new CompanyConfig());
            modelBuilder.Configurations.Add(new TenantConfig());
            modelBuilder.Configurations.Add(new VillaConfig());
            modelBuilder.Configurations.Add(new SelectionConfig());

        }
    }
}