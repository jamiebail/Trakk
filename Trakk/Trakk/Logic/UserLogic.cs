using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using API.Helpers;
using Microsoft.AspNet.Identity;
using Trakk.Models;

namespace Trakk.Logic
{
    public class UserLogic : IUserLogic
    {
        public IAPIGetter _getter;

        public UserLogic(IAPIGetter getter)
        {
            _getter = getter;
        }

        public UserLogic()
        {
            _getter = new APIGetter();
        }


        public async Task<bool> CheckIfTeamAdmin(IIdentity identityIn, int teamId)
        {
            if (identityIn.IsAuthenticated)
            {
                TeamMember member = await _getter.GetUser(GetPlayerId(identityIn));
                Team team = await _getter.GetTeam(teamId);
                List<TeamRoles> adminRoles =
                    team.Roles.Where(
                        x =>
                            x.TeamId == teamId && x.UserId == member.Id &&
                            (x.Role == TrakkEnums.TeamRole.Committee || x.Role == TrakkEnums.TeamRole.Captain)).ToList();
                if (adminRoles.Count > 0)
                    return true;

                return false;
            }
            return false;
        }

        public async Task<List<Team>> CheckIfTeamAdminAny(IIdentity identityIn)
        {
            if (identityIn.IsAuthenticated)
            {
                TeamMember member = await _getter.GetUser(GetPlayerId(identityIn));
                List<TeamRoles> adminRoles= new List<TeamRoles>();
                foreach (var team in member.Teams)
                {
                    adminRoles.AddRange(team.Roles.Where(
                       x =>
                           x.TeamId == team.Id && x.UserId == member.Id &&
                           (x.Role == TrakkEnums.TeamRole.Committee || x.Role == TrakkEnums.TeamRole.Captain)).ToList());
                }

                return member.Teams.Where(x => adminRoles.Any(y => y.TeamId == x.Id)).ToList();
                   
            }
            return null;
        }

        public int GetPlayerId(IIdentity identityIn)
        {
            var identity = (ClaimsIdentity)identityIn;
            var playerId = (identity.FindFirst("PlayerId").ToString());
            int id = int.Parse(playerId.Replace("PlayerId: ", ""));
            return id;
        }

        public async Task<List<TeamMember>> GetAllUsers()
        {
            return await _getter.GetAllUsers();
        }

    }
}