namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temafixturesetupasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamFixtureSetups", "TeamId");
            CreateIndex("dbo.TeamFixtureSetups", "FixtureId");
            AddForeignKey("dbo.TeamFixtureSetups", "FixtureId", "dbo.Fixtures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TeamFixtureSetups", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamFixtureSetups", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamFixtureSetups", "FixtureId", "dbo.Fixtures");
            DropIndex("dbo.TeamFixtureSetups", new[] { "FixtureId" });
            DropIndex("dbo.TeamFixtureSetups", new[] { "TeamId" });
        }
    }
}
