namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "stm.Bill",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ContractId = c.String(nullable: false, maxLength: 128),
                        Code = c.String(),
                        DateStamp = c.DateTime(nullable: false),
                        Status_Code = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("stm.Transaction", t => t.ContractId, cascadeDelete: true)
                .Index(t => t.ContractId);
            
            CreateTable(
                "stm.Transaction",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        RentalType_Code = c.String(),
                        ContractStatus_Code = c.String(),
                        Period_Start = c.DateTime(nullable: false),
                        Period_End = c.DateTime(nullable: false),
                        Amount_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status_Code = c.String(),
                        VillaId = c.String(),
                        TenantId = c.String(),
                        UserId = c.String(),
                        IsReversed = c.Boolean(nullable: false),
                        IsTerminated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, name: "IX_TransactionCode");
            
            CreateTable(
                "stm.Terminates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStamp = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserId = c.String(),
                        Transaction_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("stm.Transaction", t => t.Transaction_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "stm.Payment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentDate = c.DateTime(nullable: false),
                        BillId = c.String(nullable: false, maxLength: 128),
                        PaymentType_Code = c.String(),
                        PaymentMode_Code = c.String(),
                        ChequeNo = c.String(),
                        Bank_Code = c.String(),
                        Period_Start = c.DateTime(nullable: false),
                        Period_End = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status_Code = c.String(),
                        StatusDate = c.DateTime(),
                        Remarks = c.String(),
                        IsReverse = c.Boolean(nullable: false),
                        UpdateStamp_UserId = c.String(),
                        UpdateStamp_DateStamp = c.DateTime(nullable: false),
                        PaymentTypeDescription = c.String(),
                        PaymentModeDescription = c.String(),
                        BankDescription = c.String(),
                        StatusDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("stm.Bill", t => t.BillId, cascadeDelete: true)
                .Index(t => t.BillId);
            
            CreateTable(
                "stm.Reconcile",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BillId = c.String(nullable: false, maxLength: 128),
                        PaymentMode_Code = c.String(),
                        PaymentType_Code = c.String(),
                        Bank_Code = c.String(),
                        ChequeNo = c.String(),
                        ReferenceNo = c.String(),
                        DishonouredAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Period_Start = c.DateTime(nullable: false),
                        Period_End = c.DateTime(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("stm.Bill", t => t.BillId, cascadeDelete: true)
                .Index(t => t.BillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("stm.Reconcile", "BillId", "stm.Bill");
            DropForeignKey("stm.Payment", "BillId", "stm.Bill");
            DropForeignKey("stm.Bill", "ContractId", "stm.Transaction");
            DropForeignKey("stm.Terminates", "Transaction_Id", "stm.Transaction");
            DropIndex("stm.Reconcile", new[] { "BillId" });
            DropIndex("stm.Payment", new[] { "BillId" });
            DropIndex("stm.Terminates", new[] { "Transaction_Id" });
            DropIndex("stm.Transaction", "IX_TransactionCode");
            DropIndex("stm.Bill", new[] { "ContractId" });
            DropTable("stm.Reconcile");
            DropTable("stm.Payment");
            DropTable("stm.Terminates");
            DropTable("stm.Transaction");
            DropTable("stm.Bill");
        }
    }
}
