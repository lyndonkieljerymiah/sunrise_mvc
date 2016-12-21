using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.EntityConfigurations
{
    public class CompanyConfig : EntityTypeConfiguration<Company>
    {
        public CompanyConfig()
        {
            this.HasKey(c => c.TenantId);
        }
    }
}
