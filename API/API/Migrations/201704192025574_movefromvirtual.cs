namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movefromvirtual : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "Player_Id", "dbo.TeamMembers");
            DropIndex("dbo.Cards", new[] { "Player_Id" });
            AddColumn("dbo.Cards", "PlayerId", c => c.Int(nullable: false));
            DropColumn("dbo.Cards", "Player_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "Player_Id", c => c.Int());
            DropColumn("dbo.Cards", "PlayerId");
            CreateIndex("dbo.Cards", "Player_Id");
            AddForeignKey("dbo.Cards", "Player_Id", "dbo.TeamMembers", "Id");
        }
    }
}
