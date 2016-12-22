namespace Sunrise.Client.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "BusinessType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "BusinessType");
        }
    }
}
