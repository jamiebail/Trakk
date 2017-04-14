namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixtureavailabilities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerFixtureAvailabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Availability = c.Int(nullable: false),
                        FixtureId = c.Int(nullable: false),
                        TeamMember_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamMembers", t => t.TeamMember_Id)
                .Index(t => t.TeamMember_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerFixtureAvailabilities", "TeamMember_Id", "dbo.TeamMembers");
            DropIndex("dbo.PlayerFixtureAvailabilities", new[] { "TeamMember_Id" });
            DropTable("dbo.PlayerFixtureAvailabilities");
        }
    }
}
