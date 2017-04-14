namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createfixtures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fixtures", "Comments", c => c.String());
            AddColumn("dbo.Fixtures", "Positions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fixtures", "Positions");
            DropColumn("dbo.Fixtures", "Comments");
        }
    }
}
