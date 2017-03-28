namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeamEvents");
        }
    }
}
