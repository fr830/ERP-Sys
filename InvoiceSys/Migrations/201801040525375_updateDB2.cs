namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountInfoes", "employee_id", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccountInfoes", "employee_id", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
