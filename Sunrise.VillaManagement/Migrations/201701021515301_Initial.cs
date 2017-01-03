namespace Sunrise.VillaManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "vm.VillaGallery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VillaId = c.String(nullable: false, maxLength: 128),
                        ImageData = c.Binary(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("vm.Villa", t => t.VillaId, cascadeDelete: true)
                .Index(t => t.VillaId);
            
            CreateTable(
                "vm.Villa",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateStamp = c.DateTime(nullable: false),
                        VillaNo = c.String(nullable: false, maxLength: 20),
                        ElecNo = c.String(),
                        WaterNo = c.String(),
                        QtelNo = c.String(),
                        Status = c.String(),
                        Type = c.String(),
                        Capacity = c.Int(nullable: false),
                        Description = c.String(),
                        Picture = c.Binary(),
                        RatePerMonth = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.VillaNo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("vm.VillaGallery", "VillaId", "vm.Villa");
            DropIndex("vm.Villa", new[] { "VillaNo" });
            DropIndex("vm.VillaGallery", new[] { "VillaId" });
            DropTable("vm.Villa");
            DropTable("vm.VillaGallery");
        }
    }
}
