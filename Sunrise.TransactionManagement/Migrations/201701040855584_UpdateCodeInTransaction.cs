namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCodeInTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Transaction", "Code", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("stm.Transaction", "Code", name: "IX_TransactionCode");
        }
        
        public override void Down()
        {
            DropIndex("stm.Transaction", "IX_TransactionCode");
            DropColumn("stm.Transaction", "Code");
        }
    }
}
