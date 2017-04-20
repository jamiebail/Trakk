using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Configuration.ProxyCreationEnabled = false;
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<TeamMember> Members { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<GameReport> Results { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamStatistics> TeamStatistics { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; } 
        public DbSet<Position> Positions { get; set; } 
        public DbSet<Event> Events { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<TeamEvent> TeamEvents { get; set; }
        public DbSet<PlayerEventAvailability> Availabilities { get; set; }
        public DbSet<PrivateEvent> PrivateEvents { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<PlayerFixtureAvailability> FixtureAvailabilities { get; set; }
        public DbSet<TeamRoles> TeamRoles { get; set; }
    }
}