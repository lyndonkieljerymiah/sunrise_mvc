using System.Collections.Generic;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sunrise.Client.Persistence.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sunrise.Client.Persistence.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Selections.Any())
            {
                ICollection<Selection> selections = new List<Selection>
                {
                    new Selection() {Type = "TenantType",Code = "ttin" ,Description = "Individual" },
                    new Selection() {Type = "TenantType",Code = "ttco", Description = "Company" },
                    new Selection() {Type = "RentalType",Code ="rtff", Description = "Fully Furnished" },
                    new Selection() {Type = "RentalType",Code ="rtsf", Description = "Semi-Furnished" },
                    new Selection() {Type = "ContractStatus",Code ="csl", Description = "Legalized" },
                    new Selection() {Type = "ContractStatus",Code = "csmb", Description = "Monthly Basis" },
                    new Selection() {Type = "PaymentTerms", Code="ptcs", Description = "Cash" },
                    new Selection() {Type = "PaymentTerms", Code="ptcq", Description = "Cheque" },
                    new Selection() {Type = "PaymentStatus", Code="psc", Description = "Clear" },
                    new Selection() {Type = "PaymentStatus", Code="psb", Description = "Bounce" },
                    new Selection() {Type = "PaymentStatus", Code="psh", Description = "Hold" },
                    new Selection() {Type = "PaymentStatus", Code="psv", Description = "Verification" },
                    new Selection() {Type = "SalesStatus", Code="ssp", Description = "Pending" },
                    new Selection() {Type = "SalesStatus", Code="ssb", Description = "Confirmed" },
                    new Selection() {Type = "SalesStatus", Code="ssc", Description = "Cancelled" }
                };
                context.Selections.AddRange(selections);
            }
        }
    }
}
