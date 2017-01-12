namespace Sunrise.TransactionManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentStamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("stm.Payment", "UpdateStamp_UserId", c => c.String());
            AddColumn("stm.Payment", "UpdateStamp_DateStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("stm.Payment", "UpdateStamp_DateStamp");
            DropColumn("stm.Payment", "UpdateStamp_UserId");
        }
    }
}
