namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolidayTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        daysMonth = c.String(),
                        year = c.String(),
                        state = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HolidayTables");
        }
    }
}
