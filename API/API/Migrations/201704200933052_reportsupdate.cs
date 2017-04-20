namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportsupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "TeamStatistics_Id", "dbo.TeamStatistics");
            DropIndex("dbo.Cards", new[] { "TeamStatistics_Id" });
            AddColumn("dbo.Goals", "ReportId", c => c.Int(nullable: false));
            AddColumn("dbo.Cards", "ReportId", c => c.Int(nullable: false));
            DropColumn("dbo.Cards", "FixtureId");
            DropColumn("dbo.Cards", "TeamStatistics_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "TeamStatistics_Id", c => c.Int());
            AddColumn("dbo.Cards", "FixtureId", c => c.Int(nullable: false));
            DropColumn("dbo.Cards", "ReportId");
            DropColumn("dbo.Goals", "ReportId");
            CreateIndex("dbo.Cards", "TeamStatistics_Id");
            AddForeignKey("dbo.Cards", "TeamStatistics_Id", "dbo.TeamStatistics", "Id");
        }
    }
}
