namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stats : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TeamStatistics", "TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeamStatistics", "TeamId", c => c.Int(nullable: false));
        }
    }
}
