using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.EntityConfigurations
{
    public class TransactionConfig : EntityTypeConfiguration<SalesTransaction>
    {
        public TransactionConfig()
        {
            this.HasKey(t => t.Id);

            this.HasMany(t => t.Payments)
                .WithRequired(p => p.SalesTransaction)
                .HasForeignKey(p => p.SalesTransactionId);


            this.HasRequired(t => t.Tenant).WithMany();
            this.HasRequired(t => t.Villa).WithMany();
        }
    }
}
