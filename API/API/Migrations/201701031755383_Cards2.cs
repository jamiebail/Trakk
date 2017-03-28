namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cards2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cards", "Side", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cards", "Side");
        }
    }
}
