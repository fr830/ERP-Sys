namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "annual_leave_bal", c => c.Single(nullable: false));
            AddColumn("dbo.AccountInfoes", "medical_leave_bal", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "medical_leave_bal");
            DropColumn("dbo.AccountInfoes", "annual_leave_bal");
        }
    }
}
