namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test123 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "TeamMember_Id", c => c.Int());
            CreateIndex("dbo.Teams", "TeamMember_Id");
            AddForeignKey("dbo.Teams", "TeamMember_Id", "dbo.TeamMembers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "TeamMember_Id", "dbo.TeamMembers");
            DropIndex("dbo.Teams", new[] { "TeamMember_Id" });
            DropColumn("dbo.Teams", "TeamMember_Id");
        }
    }
}
