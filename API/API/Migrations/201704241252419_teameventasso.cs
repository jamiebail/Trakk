namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teameventasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamEvents", "TeamId");
            CreateIndex("dbo.TeamEvents", "EventId");
            AddForeignKey("dbo.TeamEvents", "EventId", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TeamEvents", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamEvents", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamEvents", "EventId", "dbo.Events");
            DropIndex("dbo.TeamEvents", new[] { "EventId" });
            DropIndex("dbo.TeamEvents", new[] { "TeamId" });
        }
    }
}
