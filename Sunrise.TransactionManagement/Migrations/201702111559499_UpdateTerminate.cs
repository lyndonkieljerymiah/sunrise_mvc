namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTerminate : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Terminates", "ReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("stm.Terminates", "ReferenceNo");
        }
    }
}
