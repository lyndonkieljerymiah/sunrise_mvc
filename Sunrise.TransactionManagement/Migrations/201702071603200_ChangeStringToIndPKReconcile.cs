namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStringToIndPKReconcile : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("stm.Reconcile");
            AlterColumn("stm.Reconcile", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("stm.Reconcile", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("stm.Reconcile");
            AlterColumn("stm.Reconcile", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("stm.Reconcile", "Id");
        }
    }
}
