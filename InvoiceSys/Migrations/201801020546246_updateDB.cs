namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountInfoes", "password", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccountInfoes", "password", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
