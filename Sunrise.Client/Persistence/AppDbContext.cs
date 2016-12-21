using Microsoft.AspNet.Identity.EntityFramework;
using Sunrise.Client.Domains.Models.Identity;

namespace Sunrise.Client.Persistence
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext()
            : base("DbConnection", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}