namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leave_Application", "no_days_applied", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leave_Application", "no_days_applied", c => c.Int(nullable: false));
        }
    }
}
