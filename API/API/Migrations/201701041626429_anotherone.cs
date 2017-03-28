namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fixtures", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fixtures", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fixtures", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fixtures", "DateTime");
        }
    }
}
