namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateVillaImageBlob : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.Villa", "Picture_Size", c => c.Int(nullable: false));
            AddColumn("vm.Villa", "Picture_MimeType", c => c.String());
            AddColumn("vm.Villa", "Picture_Blob", c => c.Binary());
            DropColumn("vm.Villa", "Picture");
        }
        
        public override void Down()
        {
            AddColumn("vm.Villa", "Picture", c => c.Binary());
            DropColumn("vm.Villa", "Picture_Blob");
            DropColumn("vm.Villa", "Picture_MimeType");
            DropColumn("vm.Villa", "Picture_Size");
        }
    }
}
