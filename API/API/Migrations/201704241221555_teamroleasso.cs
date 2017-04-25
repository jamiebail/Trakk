namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamroleasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamRoles", "UserId");
            AddForeignKey("dbo.TeamRoles", "UserId", "dbo.TeamMembers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamRoles", "UserId", "dbo.TeamMembers");
            DropIndex("dbo.TeamRoles", new[] { "UserId" });
        }
    }
}
