namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class privateteamid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateEvents", "TeamId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateEvents", "TeamId");
        }
    }
}
