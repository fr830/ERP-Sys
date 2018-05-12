namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "tempPassword", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "tempPassword");
        }
    }
}
