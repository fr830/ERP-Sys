namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "remarks", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "remarks");
        }
    }
}
