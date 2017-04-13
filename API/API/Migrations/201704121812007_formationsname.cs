namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formationsname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formations", "Name", c => c.String());
            CreateIndex("dbo.Formations", "TeamId");
            AddForeignKey("dbo.Formations", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Formations", "TeamId", "dbo.Teams");
            DropIndex("dbo.Formations", new[] { "TeamId" });
            DropColumn("dbo.Formations", "Name");
        }
    }
}
