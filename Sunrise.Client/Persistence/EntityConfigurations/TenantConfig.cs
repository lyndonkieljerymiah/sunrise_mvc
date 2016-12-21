using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.EntityConfigurations
{
    public class TenantConfig : EntityTypeConfiguration<Tenant>
    {

        public TenantConfig()
        {

            this.Property(t => t.Name).IsRequired().HasMaxLength(150);
            this.Property(t => t.TenantType).IsRequired();

            this.HasOptional(t => t.Individual).WithRequired();

            this.HasOptional(t => t.Company).WithRequired();
        }   
    }
}
