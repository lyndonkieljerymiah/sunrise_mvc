namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentCoveredPeriod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "CoveredPeriodFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payments", "CoveredPeriodTo", c => c.DateTime(nullable: false));
            DropColumn("dbo.Payments", "CoveredPeriod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "CoveredPeriod", c => c.DateTime(nullable: false));
            DropColumn("dbo.Payments", "CoveredPeriodTo");
            DropColumn("dbo.Payments", "CoveredPeriodFrom");
        }
    }
}
