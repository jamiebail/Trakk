namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsUpdate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "FbFeed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "FbFeed");
        }
    }
}
