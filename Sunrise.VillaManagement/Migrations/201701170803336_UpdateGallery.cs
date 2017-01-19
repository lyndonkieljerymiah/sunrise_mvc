namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGallery : DbMigration
    {
        public override void Up()
        {
            AddColumn("vm.VillaGallery", "Blob_Size", c => c.Int(nullable: false));
            AddColumn("vm.VillaGallery", "Blob_MimeType", c => c.String());
            AddColumn("vm.VillaGallery", "Blob_Blob", c => c.Binary());
            AddColumn("vm.VillaGallery", "Blob_FileName", c => c.String());
            AddColumn("vm.VillaGallery", "Blob_FileFormat", c => c.String());
            AddColumn("vm.Villa", "Picture_FileName", c => c.String());
            AddColumn("vm.Villa", "Picture_FileFormat", c => c.String());
            DropColumn("vm.VillaGallery", "ImageData");
            DropColumn("vm.VillaGallery", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("vm.VillaGallery", "ImagePath", c => c.String());
            AddColumn("vm.VillaGallery", "ImageData", c => c.Binary());
            DropColumn("vm.Villa", "Picture_FileFormat");
            DropColumn("vm.Villa", "Picture_FileName");
            DropColumn("vm.VillaGallery", "Blob_FileFormat");
            DropColumn("vm.VillaGallery", "Blob_FileName");
            DropColumn("vm.VillaGallery", "Blob_Blob");
            DropColumn("vm.VillaGallery", "Blob_MimeType");
            DropColumn("vm.VillaGallery", "Blob_Size");
        }
    }
}
