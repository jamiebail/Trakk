using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;

namespace API.Logic
{
    interface IUserLogic
    {
        List<TeamMember> GetUsers();
        TeamMember GetUser(int id);
        EntityResponse DeleteUser(TeamMember user);
        EntityResponse UpdateUser(TeamMember user);
        EntityResponse CreateUser(TeamMember user);
        EntityResponse SetUserTeam(int userId, int teamId);
        EntityResponse SetUserRole(TeamRoles role);
        EntityResponse UpdateUserRole(TeamRoles role);
        EntityResponse UpdateAvailability(PlayerEventAvailability availability);
        EntityResponse UpdateFixtureAvailability(PlayerFixtureAvailability availability);
        TeamMembership GetUserMembership(int userId, int teamId);
        EntityResponse AcceptTeamInvite(TeamMembership membership);
        EntityResponse RejectTeamInvite(TeamMembership membership);
    }
}
