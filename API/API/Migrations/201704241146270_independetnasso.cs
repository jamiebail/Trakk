namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class independetnasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PlayerEventAvailabilities", "EventId");
            AddForeignKey("dbo.PlayerEventAvailabilities", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerEventAvailabilities", "EventId", "dbo.Events");
            DropIndex("dbo.PlayerEventAvailabilities", new[] { "EventId" });
        }
    }
}
