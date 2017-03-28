namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fixtures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeId = c.Int(nullable: false),
                        AwayId = c.Int(nullable: false),
                        Location = c.String(),
                        Easting = c.Int(nullable: false),
                        Northing = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameReports", t => t.Result_Id)
                .Index(t => t.Result_Id);
            
            CreateTable(
                "dbo.GameReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeScore = c.Int(nullable: false),
                        AwayScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardColour = c.Int(nullable: false),
                        FixtureId = c.Int(nullable: false),
                        Player_Id = c.Int(),
                        GameReport_Id = c.Int(),
                        TeamStatistics_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TeamMembers", t => t.Player_Id)
                .ForeignKey("dbo.GameReports", t => t.GameReport_Id)
                .ForeignKey("dbo.TeamStatistics", t => t.TeamStatistics_Id)
                .Index(t => t.Player_Id)
                .Index(t => t.GameReport_Id)
                .Index(t => t.TeamStatistics_Id);
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Score = c.Int(nullable: false),
                        Position_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.Position_Id)
                .Index(t => t.Position_Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SportId = c.Int(nullable: false),
                        Name = c.String(),
                        Abbreviation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeamMemberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sport_Id = c.Int(nullable: false),
                        Statistics_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sports", t => t.Sport_Id, cascadeDelete: true)
                .ForeignKey("dbo.TeamStatistics", t => t.Statistics_Id)
                .Index(t => t.Sport_Id)
                .Index(t => t.Statistics_Id);
            
            CreateTable(
                "dbo.TeamStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        Won = c.Int(nullable: false),
                        Lost = c.Int(nullable: false),
                        Played = c.Int(nullable: false),
                        Drawn = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                        Conceded = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Teams", "Statistics_Id", "dbo.TeamStatistics");
            DropForeignKey("dbo.Cards", "TeamStatistics_Id", "dbo.TeamStatistics");
            DropForeignKey("dbo.Teams", "Sport_Id", "dbo.Sports");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Fixtures", "Result_Id", "dbo.GameReports");
            DropForeignKey("dbo.Cards", "GameReport_Id", "dbo.GameReports");
            DropForeignKey("dbo.Cards", "Player_Id", "dbo.TeamMembers");
            DropForeignKey("dbo.TeamMembers", "Position_Id", "dbo.Positions");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Teams", new[] { "Statistics_Id" });
            DropIndex("dbo.Teams", new[] { "Sport_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TeamMembers", new[] { "Position_Id" });
            DropIndex("dbo.Cards", new[] { "TeamStatistics_Id" });
            DropIndex("dbo.Cards", new[] { "GameReport_Id" });
            DropIndex("dbo.Cards", new[] { "Player_Id" });
            DropIndex("dbo.Fixtures", new[] { "Result_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TeamStatistics");
            DropTable("dbo.Teams");
            DropTable("dbo.TeamMemberships");
            DropTable("dbo.Sports");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Positions");
            DropTable("dbo.TeamMembers");
            DropTable("dbo.Cards");
            DropTable("dbo.GameReports");
            DropTable("dbo.Fixtures");
        }
    }
}
