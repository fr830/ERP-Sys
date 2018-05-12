namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HolidayTables", "referenceKey", c => c.String());
            DropColumn("dbo.HolidayTables", "daysMonth");
            DropColumn("dbo.HolidayTables", "year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HolidayTables", "year", c => c.String());
            AddColumn("dbo.HolidayTables", "daysMonth", c => c.String());
            DropColumn("dbo.HolidayTables", "referenceKey");
        }
    }
}
