namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "email", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "email");
        }
    }
}
