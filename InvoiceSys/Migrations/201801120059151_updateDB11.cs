namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer_WorkSheet", "isChargeable", c => c.String());
            AddColumn("dbo.Employee_WorkSheet", "isChargeable", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee_WorkSheet", "isChargeable");
            DropColumn("dbo.Customer_WorkSheet", "isChargeable");
        }
    }
}
