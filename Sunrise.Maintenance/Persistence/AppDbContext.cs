using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralContext;

namespace Sunrise.Maintenance.Persistence
{
    public class AppDbContext : BaseDbContext
    {
        public DbSet<Selection> Selections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("main");
            modelBuilder.Entity<Selection>().ToTable("Selection");

            modelBuilder.Entity<Selection>()
                .HasKey(s => s.Code);

            modelBuilder.Entity<Selection>()
                .Property(s => s.Type)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_SelectionType")));
            
        }
    }
}
