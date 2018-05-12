namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leave_Application", "status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leave_Application", "status", c => c.String(nullable: false));
        }
    }
}
