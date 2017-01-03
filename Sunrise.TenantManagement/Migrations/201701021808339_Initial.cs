namespace Sunrise.TenantManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "tm.Tenant",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateRegistered = c.DateTime(nullable: false),
                        Name = c.String(),
                        Code = c.String(maxLength: 50),
                        TenantType = c.String(),
                        EmailAddress = c.String(),
                        TelNo = c.String(),
                        MobileNo = c.String(),
                        FaxNo = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Address_Address1 = c.String(),
                        Address_Address2 = c.String(),
                        Address_City = c.String(),
                        Address_PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, name: "IX_TenantCode");
            
            CreateTable(
                "tm.Company",
                c => new
                    {
                        TenantId = c.String(nullable: false, maxLength: 128),
                        BusinessType = c.String(),
                        CrNo = c.String(),
                        ValidityDate = c.DateTime(nullable: false),
                        Representative = c.String(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("tm.Tenant", t => t.TenantId)
                .Index(t => t.TenantId);
            
            CreateTable(
                "tm.Individual",
                c => new
                    {
                        TenantId = c.String(nullable: false, maxLength: 128),
                        Birthday = c.DateTime(nullable: false),
                        Gender = c.Int(),
                        QatarId = c.String(),
                        Company = c.String(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("tm.Tenant", t => t.TenantId)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("tm.Individual", "TenantId", "tm.Tenant");
            DropForeignKey("tm.Company", "TenantId", "tm.Tenant");
            DropIndex("tm.Individual", new[] { "TenantId" });
            DropIndex("tm.Company", new[] { "TenantId" });
            DropIndex("tm.Tenant", "IX_TenantCode");
            DropTable("tm.Individual");
            DropTable("tm.Company");
            DropTable("tm.Tenant");
        }
    }
}
