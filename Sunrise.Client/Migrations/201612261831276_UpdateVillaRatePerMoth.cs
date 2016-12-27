namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVillaRatePerMoth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Villas", "RatePerMonth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Villas", "RatePerMonth");
        }
    }
}
