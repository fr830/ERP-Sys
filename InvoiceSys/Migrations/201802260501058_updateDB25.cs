namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "reset_retry_count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "reset_retry_count");
        }
    }
}
