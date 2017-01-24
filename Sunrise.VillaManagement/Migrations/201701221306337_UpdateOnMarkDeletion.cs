namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOnMarkDeletion : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.VillaGallery", "IsMarkForDeletion", c => c.Boolean(nullable: false));
            DropColumn("vm.VillaGallery", "IsPrimary");
        }
        
        public override void Down()
        {
            AddColumn("vm.VillaGallery", "IsPrimary", c => c.Boolean(nullable: false));
            DropColumn("vm.VillaGallery", "IsMarkForDeletion");
        }
    }
}
