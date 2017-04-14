namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamRoles", "TeamId");
            AddForeignKey("dbo.TeamRoles", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamRoles", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamRoles", new[] { "TeamId" });
        }
    }
}
