namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReverseInContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Transaction", "IsReversed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("stm.Transaction", "IsReversed");
        }
    }
}
