namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availbilitesupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerFixtureAvailabilities", "EventId", c => c.Int(nullable: false));
            DropColumn("dbo.PlayerFixtureAvailabilities", "FixtureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerFixtureAvailabilities", "FixtureId", c => c.Int(nullable: false));
            DropColumn("dbo.PlayerFixtureAvailabilities", "EventId");
        }
    }
}
