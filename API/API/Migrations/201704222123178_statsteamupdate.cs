namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statsteamupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "Statistics_Id", "dbo.TeamStatistics");
            DropIndex("dbo.Teams", new[] { "Statistics_Id" });
            AddColumn("dbo.Teams", "TeamStatistics", c => c.Int(nullable: false));
            DropColumn("dbo.Teams", "Statistics_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teams", "Statistics_Id", c => c.Int());
            DropColumn("dbo.Teams", "TeamStatistics");
            CreateIndex("dbo.Teams", "Statistics_Id");
            AddForeignKey("dbo.Teams", "Statistics_Id", "dbo.TeamStatistics", "Id");
        }
    }
}
