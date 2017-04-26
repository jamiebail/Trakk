namespace Trakk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sportpitches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SportPitchModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SportId = c.Int(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SportPitchModels");
        }
    }
}
