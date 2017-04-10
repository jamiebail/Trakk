namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class privateevents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrivateEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PrivateEvents");
        }
    }
}
