namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer_PrivateInfo", "state", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer_PrivateInfo", "state");
        }
    }
}
