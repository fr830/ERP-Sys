namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employee_WorkSheet", "employee_ID", c => c.String(nullable: false));
            AlterColumn("dbo.Employee_WorkSheet", "employee_name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee_WorkSheet", "employee_name", c => c.String(maxLength: 100));
            AlterColumn("dbo.Employee_WorkSheet", "employee_ID", c => c.String());
        }
    }
}
