namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cardsmissing2 : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cards");
        }
    }
}
