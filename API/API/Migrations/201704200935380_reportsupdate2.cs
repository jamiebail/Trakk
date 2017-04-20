namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportsupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "GameReport_Id", "dbo.GameReports");
            DropForeignKey("dbo.Goals", "GameReport_Id", "dbo.GameReports");
            DropIndex("dbo.Goals", new[] { "GameReport_Id" });
            DropIndex("dbo.Cards", new[] { "GameReport_Id" });
            DropColumn("dbo.Goals", "GameReport_Id");
            DropTable("dbo.Cards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardColour = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        GameReport_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Goals", "GameReport_Id", c => c.Int());
            CreateIndex("dbo.Cards", "GameReport_Id");
            CreateIndex("dbo.Goals", "GameReport_Id");
            AddForeignKey("dbo.Goals", "GameReport_Id", "dbo.GameReports", "Id");
            AddForeignKey("dbo.Cards", "GameReport_Id", "dbo.GameReports", "Id");
        }
    }
}
