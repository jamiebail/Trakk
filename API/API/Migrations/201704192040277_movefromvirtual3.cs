namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movefromvirtual3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fixtures", "Result_Id", "dbo.GameReports");
            DropIndex("dbo.Fixtures", new[] { "Result_Id" });
            DropColumn("dbo.Fixtures", "Result_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fixtures", "Result_Id", c => c.Int());
            CreateIndex("dbo.Fixtures", "Result_Id");
            AddForeignKey("dbo.Fixtures", "Result_Id", "dbo.GameReports", "Id");
        }
    }
}
