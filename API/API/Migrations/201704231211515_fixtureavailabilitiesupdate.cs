namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixtureavailabilitiesupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerFixtureAvailabilities", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayerFixtureAvailabilities", "TeamId");
        }
    }
}
