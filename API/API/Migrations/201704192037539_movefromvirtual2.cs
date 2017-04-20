namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movefromvirtual2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fixtures", "ReportId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fixtures", "ReportId");
        }
    }
}
