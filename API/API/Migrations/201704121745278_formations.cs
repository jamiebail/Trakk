namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Formations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        FormationJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Formations");
        }
    }
}
