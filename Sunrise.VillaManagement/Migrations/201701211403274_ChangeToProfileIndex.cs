namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToProfileIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.Villa", "ProfileIndex", c => c.Int(nullable: false));
            DropColumn("vm.Villa", "ProfileImage");
        }
        
        public override void Down()
        {
            AddColumn("vm.Villa", "ProfileImage", c => c.Int(nullable: false));
            DropColumn("vm.Villa", "ProfileIndex");
        }
    }
}
