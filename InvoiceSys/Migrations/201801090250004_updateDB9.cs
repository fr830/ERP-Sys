namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee_Management",
                c => new
                    {
                        employee_id = c.String(nullable: false, maxLength: 128),
                        username = c.String(),
                        employee_name = c.String(),
                        user_contact_no = c.String(),
                    })
                .PrimaryKey(t => t.employee_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employee_Management");
        }
    }
}
