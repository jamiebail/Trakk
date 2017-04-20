namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class goalupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goals", "Scorer_Id", "dbo.TeamMembers");
            DropIndex("dbo.Goals", new[] { "Scorer_Id" });
            AddColumn("dbo.Goals", "ScorerId", c => c.Int(nullable: false));
            DropColumn("dbo.Goals", "Scorer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goals", "Scorer_Id", c => c.Int());
            DropColumn("dbo.Goals", "ScorerId");
            CreateIndex("dbo.Goals", "Scorer_Id");
            AddForeignKey("dbo.Goals", "Scorer_Id", "dbo.TeamMembers", "Id");
        }
    }
}
