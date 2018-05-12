namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leave_Application", "leave_ticket_id", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leave_Application", "leave_ticket_id");
        }
    }
}
