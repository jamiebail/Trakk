namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "DateTime");
        }
    }
}
