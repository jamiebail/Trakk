namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsUpdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Attending", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Invited", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Invited");
            DropColumn("dbo.Events", "Attending");
        }
    }
}
