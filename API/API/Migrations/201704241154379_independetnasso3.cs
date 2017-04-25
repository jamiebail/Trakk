namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class independetnasso3 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PlayerEventAvailabilities", "UserId");
            CreateIndex("dbo.PlayerFixtureAvailabilities", "UserId");
            CreateIndex("dbo.PlayerFixtureAvailabilities", "EventId");
            CreateIndex("dbo.PlayerFixtureAvailabilities", "TeamId");
            AddForeignKey("dbo.PlayerEventAvailabilities", "UserId", "dbo.TeamMembers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlayerFixtureAvailabilities", "EventId", "dbo.Fixtures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlayerFixtureAvailabilities", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlayerFixtureAvailabilities", "UserId", "dbo.TeamMembers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerFixtureAvailabilities", "UserId", "dbo.TeamMembers");
            DropForeignKey("dbo.PlayerFixtureAvailabilities", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.PlayerFixtureAvailabilities", "EventId", "dbo.Fixtures");
            DropForeignKey("dbo.PlayerEventAvailabilities", "UserId", "dbo.TeamMembers");
            DropIndex("dbo.PlayerFixtureAvailabilities", new[] { "TeamId" });
            DropIndex("dbo.PlayerFixtureAvailabilities", new[] { "EventId" });
            DropIndex("dbo.PlayerFixtureAvailabilities", new[] { "UserId" });
            DropIndex("dbo.PlayerEventAvailabilities", new[] { "UserId" });
        }
    }
}
