namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer_WorkSheet", "employee_name", c => c.String());
            AddColumn("dbo.Employee_WorkSheet", "employee_name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee_WorkSheet", "employee_name");
            DropColumn("dbo.Customer_WorkSheet", "employee_name");
        }
    }
}
