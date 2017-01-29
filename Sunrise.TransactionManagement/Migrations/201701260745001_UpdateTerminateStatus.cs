namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTerminateStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Transaction", "IsTerminated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("stm.Transaction", "IsTerminated");
        }
    }
}
