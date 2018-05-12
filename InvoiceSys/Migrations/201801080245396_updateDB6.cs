namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HolidayTables", "year", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HolidayTables", "year");
        }
    }
}
