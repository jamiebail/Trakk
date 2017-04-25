namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teammembershipasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamMemberships", "MemberId");
            CreateIndex("dbo.TeamMemberships", "TeamId");
            AddForeignKey("dbo.TeamMemberships", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TeamMemberships", "MemberId", "dbo.TeamMembers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMemberships", "MemberId", "dbo.TeamMembers");
            DropForeignKey("dbo.TeamMemberships", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamMemberships", new[] { "TeamId" });
            DropIndex("dbo.TeamMemberships", new[] { "MemberId" });
        }
    }
}
