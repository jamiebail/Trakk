namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportfixturerelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameReports", "FixtureId", c => c.Int(nullable: false));
            DropColumn("dbo.Fixtures", "ReportId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fixtures", "ReportId", c => c.Int(nullable: false));
            DropColumn("dbo.GameReports", "FixtureId");
        }
    }
}
