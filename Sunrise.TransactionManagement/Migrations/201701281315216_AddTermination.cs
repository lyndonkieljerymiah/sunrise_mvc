namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermination : DbMigration
    {
        public override void Up()
        {
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
        }
        
        public override void Down()
        {
            DropForeignKey("stm.Terminates", "Transaction_Id", "stm.Transaction");
            DropIndex("stm.Terminates", new[] { "Transaction_Id" });
            DropTable("stm.Terminates");
        }
    }
}
