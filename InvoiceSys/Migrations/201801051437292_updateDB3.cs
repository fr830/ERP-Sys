namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "user_type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "user_type");
        }
    }
}
