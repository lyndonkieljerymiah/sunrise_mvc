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

        public ReferenceDbContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<VillaView> Villas { get; set; }
        public DbSet<VillaGalleryView> Galleries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VillaView>().ToTable("VillaView", "dbo");
            modelBuilder.Entity<VillaGalleryView>().ToTable("VillaGalleryView", "dbo");
            
            modelBuilder.Entity<VillaGalleryView>()
                .HasRequired(v => v.Villa)
                .WithMany(v => v.Galleries)
                .HasForeignKey(v => v.VillaId);
                
        }
    }
}
