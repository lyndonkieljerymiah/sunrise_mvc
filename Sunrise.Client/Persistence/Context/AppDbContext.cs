using Microsoft.AspNet.Identity.EntityFramework;
using Sunrise.Client.Domains.Models.Identity;
using System.Data.Entity;

namespace Sunrise.Client.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {

      
        
        public AppDbContext()
            : base("DbConnection", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("user");
            modelBuilder.Entity<IdentityUser>().ToTable("User");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");

        }
    }
}