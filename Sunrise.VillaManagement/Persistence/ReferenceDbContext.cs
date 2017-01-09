using Sunrise.VillaManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.VillaManagement.Persistence
{
    public class ReferenceDbContext : BaseDbContext
    {
        public DbSet<VillaView> Villas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VillaView>().ToTable("VillaView", "dbo");
        }
    }
}
