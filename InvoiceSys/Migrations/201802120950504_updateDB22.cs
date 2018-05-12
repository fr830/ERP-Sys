namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoice_Details", "company_name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoice_Details", "company_name", c => c.String(maxLength: 20));
        }
    }
}
