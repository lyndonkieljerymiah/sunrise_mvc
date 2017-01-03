namespace Sunrise.TenantManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("tm.Company", "TenantId", "tm.Tenant");
            DropForeignKey("tm.Individual", "TenantId", "tm.Tenant");
            AddForeignKey("tm.Company", "TenantId", "tm.Tenant", "Id", cascadeDelete: true);
            AddForeignKey("tm.Individual", "TenantId", "tm.Tenant", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("tm.Individual", "TenantId", "tm.Tenant");
            DropForeignKey("tm.Company", "TenantId", "tm.Tenant");
            AddForeignKey("tm.Individual", "TenantId", "tm.Tenant", "Id");
            AddForeignKey("tm.Company", "TenantId", "tm.Tenant", "Id");
        }
    }
}
