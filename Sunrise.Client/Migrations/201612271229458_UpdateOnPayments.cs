namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOnPayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Bank", c => c.String());
            AddColumn("dbo.Payments", "PaymentMode", c => c.String());
            AddColumn("dbo.Payments", "CoveredPeriod", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "CoveredPeriod");
            DropColumn("dbo.Payments", "PaymentMode");
            DropColumn("dbo.Payments", "Bank");
        }
    }
}
