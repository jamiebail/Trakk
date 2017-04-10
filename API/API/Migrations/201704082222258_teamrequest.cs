namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamrequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMemberships", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamMemberships", "Accepted");
        }
    }
}
