namespace Sunrise.Maintenance.Migrations
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sunrise.Maintenance.Persistence.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sunrise.Maintenance.Persistence.AppDbContext context)
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

            if (!context.Selections.Any()) {
                ICollection<Selection> selections = new List<Selection>
                {
                    new Selection() {Type = "TenantType",Code = "ttin" ,Description = "Individual" },
                    new Selection() {Type = "TenantType",Code = "ttco", Description = "Company" },
                    new Selection() {Type = "RentalType",Code ="rtff", Description = "Fully Furnished" },
                    new Selection() {Type = "RentalType",Code ="rtsf", Description = "Semi-Furnished" },
                    new Selection() {Type = "ContractStatus",Code ="csl", Description = "Legalized" },
                    new Selection() {Type = "ContractStatus",Code = "csmb", Description = "Monthly Basis" },
                    new Selection() {Type = "PaymentTerm", Code="ptcs", Description = "Cash" },
                    new Selection() {Type = "PaymentTerm", Code="ptcq", Description = "Cheque" },
                    new Selection() {Type = "PaymentStatus", Code="psc", Description = "Clear" },
                    new Selection() {Type = "PaymentStatus", Code="psb", Description = "Bounce" },
                    new Selection() {Type = "PaymentStatus", Code="psh", Description = "Hold" },
                    new Selection() {Type = "PaymentStatus", Code="psv", Description = "Verification" },
                    new Selection() {Type = "SalesStatus", Code="ssp", Description = "Pending" },
                    new Selection() {Type = "SalesStatus", Code="sscn", Description = "Confirmed" },
                    new Selection() {Type = "SalesStatus", Code="sscc", Description = "Cancelled" },
                    new Selection() {Type = "VillaStatus", Code="vsav", Description = "Vacant" },
                    new Selection() {Type = "VillaStatus", Code="vsres", Description = "Reserved" },
                    new Selection() {Type = "VillaStatus", Code="vsna", Description = "Occupied" },
                    new Selection() {Type = "PaymentMode", Code="pmp", Description = "Payment" },
                    new Selection() {Type = "PaymentMode", Code="pmsd", Description = "Security Deposit" },
                    new Selection() {Type = "Bank", Code="bdb", Description = "Doha Bank" },
                    new Selection() {Type = "Bank", Code="bqnb", Description = "QNB" }

                };
                context.Selections.AddRange(selections);
            }
        }
    }
}
