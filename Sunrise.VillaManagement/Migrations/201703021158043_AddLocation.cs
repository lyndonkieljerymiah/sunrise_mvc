namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.Villa", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("vm.Villa", "Location");
        }
    }
}
