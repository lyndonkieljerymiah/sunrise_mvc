namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDateReconcile : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Reconcile", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("stm.Reconcile", "Date");
        }
    }
}
