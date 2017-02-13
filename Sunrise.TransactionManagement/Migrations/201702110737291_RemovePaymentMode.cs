namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePaymentMode : DbMigration
    {
        public override void Up()
        {
            DropColumn("stm.Reconcile", "PaymentMode_Code");
        }
        
        public override void Down()
        {
            AddColumn("stm.Reconcile", "PaymentMode_Code", c => c.String());
        }
    }
}
