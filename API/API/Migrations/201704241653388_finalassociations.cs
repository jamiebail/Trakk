namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalassociations : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Cards", "PlayerId");
            CreateIndex("dbo.Cards", "ReportId");
            CreateIndex("dbo.GameReports", "FixtureId");
            CreateIndex("dbo.Goals", "ScorerId");
            CreateIndex("dbo.Goals", "ReportId");
            AddForeignKey("dbo.Cards", "PlayerId", "dbo.TeamMembers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameReports", "FixtureId", "dbo.Fixtures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Cards", "ReportId", "dbo.GameReports", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Goals", "ReportId", "dbo.GameReports", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Goals", "ScorerId", "dbo.TeamMembers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "ScorerId", "dbo.TeamMembers");
            DropForeignKey("dbo.Goals", "ReportId", "dbo.GameReports");
            DropForeignKey("dbo.Cards", "ReportId", "dbo.GameReports");
            DropForeignKey("dbo.GameReports", "FixtureId", "dbo.Fixtures");
            DropForeignKey("dbo.Cards", "PlayerId", "dbo.TeamMembers");
            DropIndex("dbo.Goals", new[] { "ReportId" });
            DropIndex("dbo.Goals", new[] { "ScorerId" });
            DropIndex("dbo.GameReports", new[] { "FixtureId" });
            DropIndex("dbo.Cards", new[] { "ReportId" });
            DropIndex("dbo.Cards", new[] { "PlayerId" });
        }
    }
}
