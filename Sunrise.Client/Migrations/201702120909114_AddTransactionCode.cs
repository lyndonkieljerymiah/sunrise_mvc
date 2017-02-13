namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("user.User", "TransactionCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("user.User", "TransactionCode");
        }
    }
}
