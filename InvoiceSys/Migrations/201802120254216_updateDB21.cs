namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotation_Details", "company_name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotation_Details", "company_name", c => c.String(maxLength: 20));
        }
    }
}
