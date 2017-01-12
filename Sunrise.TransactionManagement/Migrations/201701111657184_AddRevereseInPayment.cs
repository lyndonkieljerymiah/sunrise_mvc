namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRevereseInPayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Payment", "IsReverse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("stm.Payment", "IsReverse");
        }
    }
}
