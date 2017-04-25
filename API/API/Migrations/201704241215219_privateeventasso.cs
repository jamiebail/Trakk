namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class privateeventasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PrivateEvents", "UserId");
            CreateIndex("dbo.PrivateEvents", "EventId");
            CreateIndex("dbo.PrivateEvents", "TeamId");
            AddForeignKey("dbo.PrivateEvents", "EventId", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PrivateEvents", "UserId", "dbo.TeamMembers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PrivateEvents", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateEvents", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.PrivateEvents", "UserId", "dbo.TeamMembers");
            DropForeignKey("dbo.PrivateEvents", "EventId", "dbo.Events");
            DropIndex("dbo.PrivateEvents", new[] { "TeamId" });
            DropIndex("dbo.PrivateEvents", new[] { "EventId" });
            DropIndex("dbo.PrivateEvents", new[] { "UserId" });
        }
    }
}
