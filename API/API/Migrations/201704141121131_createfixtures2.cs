namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createfixtures2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fixtures", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fixtures", "State");
        }
    }
}
