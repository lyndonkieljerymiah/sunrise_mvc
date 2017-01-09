using Sunrise.VillaManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.VillaManagement.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        public virtual DbSet<Villa> Villas { get; set; }
        public virtual DbSet<VillaGallery> Galleries { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("vm");
            modelBuilder.Entity<Villa>().ToTable("Villa");
            modelBuilder.Entity<VillaGallery>().ToTable("VillaGallery");

            //configuration
            modelBuilder.Entity<Villa>().Property(v => v.VillaNo)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Villa>()
                .Property(v => v.VillaNo)
                .HasColumnAnnotation("Index",new IndexAnnotation(new IndexAttribute("IX_VillaNo")));

            modelBuilder.Entity<Villa>()
                .HasMany(v => v.Galleries)
                .WithRequired(g => g.Villa);



        }


    }
}
