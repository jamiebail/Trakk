namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picsupdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePicture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePicture", c => c.Binary());
        }
    }
}
