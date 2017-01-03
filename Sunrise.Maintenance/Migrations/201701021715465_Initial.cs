namespace Sunrise.Maintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "main.Selection",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false, maxLength: 20),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Code)
                .Index(t => t.Type, name: "IX_SelectionType");
            
        }
        
        public override void Down()
        {
            DropIndex("main.Selection", "IX_SelectionType");
            DropTable("main.Selection");
        }
    }
}
