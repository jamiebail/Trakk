namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Goals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Minute = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Scorer_Id = c.Int(),
                        GameReport_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamMembers", t => t.Scorer_Id)
                .ForeignKey("dbo.GameReports", t => t.GameReport_Id)
                .Index(t => t.Scorer_Id)
                .Index(t => t.GameReport_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goals", "GameReport_Id", "dbo.GameReports");
            DropForeignKey("dbo.Goals", "Scorer_Id", "dbo.TeamMembers");
            DropIndex("dbo.Goals", new[] { "GameReport_Id" });
            DropIndex("dbo.Goals", new[] { "Scorer_Id" });
            DropTable("dbo.Goals");
        }
    }
}
