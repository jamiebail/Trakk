namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availbilitesupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayerEventAvailabilities", "TeamMember_Id", "dbo.TeamMembers");
            DropForeignKey("dbo.PlayerFixtureAvailabilities", "TeamMember_Id", "dbo.TeamMembers");
            DropIndex("dbo.PlayerEventAvailabilities", new[] { "TeamMember_Id" });
            DropIndex("dbo.PlayerFixtureAvailabilities", new[] { "TeamMember_Id" });
            AddColumn("dbo.PlayerEventAvailabilities", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerFixtureAvailabilities", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.PlayerEventAvailabilities", "TeamMember_Id");
            DropColumn("dbo.PlayerFixtureAvailabilities", "TeamMember_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerFixtureAvailabilities", "TeamMember_Id", c => c.Int());
            AddColumn("dbo.PlayerEventAvailabilities", "TeamMember_Id", c => c.Int());
            DropColumn("dbo.PlayerFixtureAvailabilities", "UserId");
            DropColumn("dbo.PlayerEventAvailabilities", "UserId");
            CreateIndex("dbo.PlayerFixtureAvailabilities", "TeamMember_Id");
            CreateIndex("dbo.PlayerEventAvailabilities", "TeamMember_Id");
            AddForeignKey("dbo.PlayerFixtureAvailabilities", "TeamMember_Id", "dbo.TeamMembers", "Id");
            AddForeignKey("dbo.PlayerEventAvailabilities", "TeamMember_Id", "dbo.TeamMembers", "Id");
        }
    }
}
