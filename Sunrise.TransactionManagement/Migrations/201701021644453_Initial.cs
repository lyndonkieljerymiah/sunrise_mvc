namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "stm.Payment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionId = c.String(nullable: false, maxLength: 128),
                        PaymentType = c.String(),
                        PaymentMode = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        ChequeNo = c.String(),
                        Bank = c.String(),
                        CoveredPeriodFrom = c.DateTime(nullable: false),
                        CoveredPeriodTo = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        StatusDate = c.DateTime(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("stm.Transaction", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "stm.Transaction",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        RentalType = c.String(),
                        ContractStatus = c.String(),
                        PeriodStart = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        AmountPayable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        VillaId = c.String(),
                        TenantId = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("stm.Payment", "TransactionId", "stm.Transaction");
            DropIndex("stm.Payment", new[] { "TransactionId" });
            DropTable("stm.Transaction");
            DropTable("stm.Payment");
        }
    }
}
