namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer_WorkSheet", "working_date_type", c => c.String());
            AddColumn("dbo.Employee_WorkSheet", "working_date_type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee_WorkSheet", "working_date_type");
            DropColumn("dbo.Customer_WorkSheet", "working_date_type");
        }
    }
}
