namespace MrAng_Invoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leave_Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        leave_applied_from = c.DateTime(nullable: false),
                        leave_applied_to = c.DateTime(nullable: false),
                        no_days_applied = c.Int(nullable: false),
                        status = c.String(nullable: false),
                        type_of_leave = c.String(nullable: false),
                        reason = c.String(),
                        submitted_by = c.String(),
                        submitted_date = c.DateTime(nullable: false),
                        approved_by = c.String(),
                        approval_date = c.DateTime(nullable: false),
                        approval_reason = c.String(),
                        username_submitted = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leave_Application");
        }
    }
}
