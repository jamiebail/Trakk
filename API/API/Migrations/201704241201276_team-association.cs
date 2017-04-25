namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamassociation : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Teams", "SportId");
            CreateIndex("dbo.Teams", "TeamStatistics");
            AddForeignKey("dbo.Teams", "SportId", "dbo.Sports", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Teams", "TeamStatistics", "dbo.TeamStatistics", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "TeamStatistics", "dbo.TeamStatistics");
            DropForeignKey("dbo.Teams", "SportId", "dbo.Sports");
            DropIndex("dbo.Teams", new[] { "TeamStatistics" });
            DropIndex("dbo.Teams", new[] { "SportId" });
        }
    }
}
