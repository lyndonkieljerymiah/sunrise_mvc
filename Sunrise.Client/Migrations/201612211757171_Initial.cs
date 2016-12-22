namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesTransactionId = c.Int(nullable: false),
                        Term = c.Int(nullable: false),
                        ChequeNo = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalesTransactions", t => t.SalesTransactionId, cascadeDelete: true)
                .Index(t => t.SalesTransactionId);
            
            CreateTable(
                "dbo.SalesTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        RentalType = c.String(),
                        ContractStatus = c.String(),
                        PeriodStart = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        VillaId = c.Int(nullable: false),
                        TenantId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tenants", t => t.TenantId, cascadeDelete: true)
                .ForeignKey("dbo.Villas", t => t.VillaId, cascadeDelete: true)
                .Index(t => t.VillaId)
                .Index(t => t.TenantId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(),
                        TenantType = c.String(nullable: false),
                        DateRegistered = c.DateTime(nullable: false),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        TenantId = c.Int(nullable: false),
                        CrNo = c.String(),
                        ValidityDate = c.DateTime(nullable: false),
                        Representative = c.String(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.TenantId);
            
            CreateTable(
                "dbo.Individuals",
                c => new
                    {
                        TenantId = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Gender = c.Int(),
                        QatarId = c.String(),
                        Company = c.String(),
                    })
                .PrimaryKey(t => t.TenantId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.TenantId);
            
            CreateTable(
                "dbo.Villas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStamp = c.DateTime(nullable: false),
                        VillaNo = c.String(nullable: false, maxLength: 20),
                        ElecNo = c.String(),
                        WaterNo = c.String(),
                        QtelNo = c.String(),
                        Status = c.String(),
                        Type = c.String(),
                        Capacity = c.Int(nullable: false),
                        Description = c.String(),
                        Picture = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Selections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 80),
                        Code = c.String(nullable: false, maxLength: 10),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserLogin", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserClaim", "IdentityUser_Id", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.SalesTransactions", "VillaId", "dbo.Villas");
            DropForeignKey("dbo.SalesTransactions", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Individuals", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Companies", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Payments", "SalesTransactionId", "dbo.SalesTransactions");
            DropIndex("dbo.UserLogin", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserClaim", new[] { "IdentityUser_Id" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.UserRole", new[] { "IdentityUser_Id" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.Individuals", new[] { "TenantId" });
            DropIndex("dbo.Companies", new[] { "TenantId" });
            DropIndex("dbo.SalesTransactions", new[] { "TenantId" });
            DropIndex("dbo.SalesTransactions", new[] { "VillaId" });
            DropIndex("dbo.Payments", new[] { "SalesTransactionId" });
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User");
            DropTable("dbo.Selections");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.Villas");
            DropTable("dbo.Individuals");
            DropTable("dbo.Companies");
            DropTable("dbo.Tenants");
            DropTable("dbo.SalesTransactions");
            DropTable("dbo.Payments");
        }
    }
}
