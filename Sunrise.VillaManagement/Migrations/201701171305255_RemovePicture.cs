namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePicture : DbMigration
    {
        public override void Up()
        {
            DropColumn("vm.Villa", "Picture_Size");
            DropColumn("vm.Villa", "Picture_MimeType");
            DropColumn("vm.Villa", "Picture_Blob");
            DropColumn("vm.Villa", "Picture_FileName");
            DropColumn("vm.Villa", "Picture_FileFormat");
        }
        
        public override void Down()
        {
            AddColumn("vm.Villa", "Picture_FileFormat", c => c.String());
            AddColumn("vm.Villa", "Picture_FileName", c => c.String());
            AddColumn("vm.Villa", "Picture_Blob", c => c.Binary());
            AddColumn("vm.Villa", "Picture_MimeType", c => c.String());
            AddColumn("vm.Villa", "Picture_Size", c => c.Int(nullable: false));
        }
    }
}
