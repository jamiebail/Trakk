namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartEnd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Fixtures", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Fixtures", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "DateTime");
            DropColumn("dbo.Fixtures", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fixtures", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fixtures", "End");
            DropColumn("dbo.Fixtures", "Start");
            DropColumn("dbo.Events", "End");
            DropColumn("dbo.Events", "Start");
        }
    }
}
