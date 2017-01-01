using System.Collections.Generic;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sunrise.Client.Persistence.Context.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sunrise.Client.Persistence.Context.AppDbContext context)
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

            //seed
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


            if (!context.Villas.Any())
            {
                ICollection<Villa> villas = new List<Villa>()
                {
                    new Villa()
                    {
                        VillaNo = "V100",
                        ElecNo = "E12344",
                        WaterNo = "W123",
                        QtelNo = "Q1234",
                        Capacity = 9,
                        Description = "3 Rooms and 2 Toilets full furnished"
                    },
                    new Villa()
                    {
                        VillaNo = "V200",
                        ElecNo = "E32344",
                        WaterNo = "W223",
                        QtelNo = "Q2234",
                        Capacity = 10,
                        Description = "3 Rooms and 5 Toilets full furnished"
                    }
                };
                context.Villas.AddRange(villas);
            }
            context.SaveChanges();
        }
    }
}
