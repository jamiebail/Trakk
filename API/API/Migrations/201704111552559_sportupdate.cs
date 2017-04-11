namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sportupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports");
            DropIndex("dbo.Teams", new[] { "Sport_Id" });
            AlterColumn("dbo.Teams", "Sport_Id", c => c.Int());
            CreateIndex("dbo.Teams", "Sport_Id");
            AddForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports");
            DropIndex("dbo.Teams", new[] { "Sport_Id" });
            AlterColumn("dbo.Teams", "Sport_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Teams", "Sport_Id");
            AddForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports", "Id", cascadeDelete: true);
        }
    }
}
