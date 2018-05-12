namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "employee_id", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "employee_id");
        }
    }
}
