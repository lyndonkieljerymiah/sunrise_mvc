using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.EntityConfigurations
{
    public class SelectionConfig : EntityTypeConfiguration<Selection>
    {
        public SelectionConfig()
        {
            this.Property(s => s.Type).IsRequired().HasMaxLength(80);
            this.Property(s => s.Code).IsRequired().HasMaxLength(10);
        }
    }
}
