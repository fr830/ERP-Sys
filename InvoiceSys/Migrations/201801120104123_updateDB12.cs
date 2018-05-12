namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer_WorkSheet", "working_date_type", c => c.String(maxLength: 5));
            AlterColumn("dbo.Customer_WorkSheet", "employee_name", c => c.String(maxLength: 100));
            AlterColumn("dbo.Customer_WorkSheet", "isChargeable", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Employee_WorkSheet", "working_date_type", c => c.String(maxLength: 5));
            AlterColumn("dbo.Employee_WorkSheet", "employee_name", c => c.String(maxLength: 100));
            AlterColumn("dbo.Employee_WorkSheet", "isChargeable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee_WorkSheet", "isChargeable", c => c.String());
            AlterColumn("dbo.Employee_WorkSheet", "employee_name", c => c.String());
            AlterColumn("dbo.Employee_WorkSheet", "working_date_type", c => c.String());
            AlterColumn("dbo.Customer_WorkSheet", "isChargeable", c => c.String());
            AlterColumn("dbo.Customer_WorkSheet", "employee_name", c => c.String());
            AlterColumn("dbo.Customer_WorkSheet", "working_date_type", c => c.String());
        }
    }
}
