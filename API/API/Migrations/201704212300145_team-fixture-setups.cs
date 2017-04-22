namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamfixturesetups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamFixtureSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        FixtureId = c.Int(nullable: false),
                        Comments = c.String(),
                        Positions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Fixtures", "Comments");
            DropColumn("dbo.Fixtures", "Positions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fixtures", "Positions", c => c.String());
            AddColumn("dbo.Fixtures", "Comments", c => c.String());
            DropTable("dbo.TeamFixtureSetups");
        }
    }
}
