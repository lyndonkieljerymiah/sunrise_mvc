namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIsPrimary : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.VillaGallery", "IsPrimary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("vm.VillaGallery", "IsPrimary");
        }
    }
}
