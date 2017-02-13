namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePaymentDescription : DbMigration
    {
        public override void Up()
        {
            DropColumn("stm.Payment", "PaymentTypeDescription");
            DropColumn("stm.Payment", "PaymentModeDescription");
            DropColumn("stm.Payment", "BankDescription");
            DropColumn("stm.Payment", "StatusDescription");
        }
        
        public override void Down()
        {
            AddColumn("stm.Payment", "StatusDescription", c => c.String());
            AddColumn("stm.Payment", "BankDescription", c => c.String());
            AddColumn("stm.Payment", "PaymentModeDescription", c => c.String());
            AddColumn("stm.Payment", "PaymentTypeDescription", c => c.String());
        }
    }
}
