namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProfileImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.Villa", "ProfileImage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("vm.Villa", "ProfileImage");
        }
    }
}
