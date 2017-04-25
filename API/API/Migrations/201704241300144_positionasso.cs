namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class positionasso : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Positions", "SportId");
            AddForeignKey("dbo.Positions", "SportId", "dbo.Sports", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "SportId", "dbo.Sports");
            DropIndex("dbo.Positions", new[] { "SportId" });
        }
    }
}
