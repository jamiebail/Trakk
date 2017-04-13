namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sportidteamsave : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports");
            DropIndex("dbo.Teams", new[] { "Sport_Id" });
            AddColumn("dbo.Teams", "SportId", c => c.Int(nullable: false));
            DropColumn("dbo.Teams", "Sport_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teams", "Sport_Id", c => c.Int());
            DropColumn("dbo.Teams", "SportId");
            CreateIndex("dbo.Teams", "Sport_Id");
            AddForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports", "Id");
        }
    }
}
