﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

        public TrakkEnums.Side CheckTeamSide(TeamMember member, Fixture fixture)
        {
            foreach (Team team in member.Teams)
            {
                if (team.Id == fixture.HomeId)
                {
                    return TrakkEnums.Side.Home;
                }
                if (team.Id == fixture.AwayId)
                {
                    return TrakkEnums.Side.Away;
                }
            }
            return 0;
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

        public byte[] GetUserImage(int userId)
        {
            ApplicationUser user = new ApplicationDbContext().Users.FirstOrDefault(x => x.MemberId == userId);
            if (user != null)
            {
                if (user.ProfilePicture != null)
                {
                    return user.ProfilePicture;
                }
            }
            return null;
        }
    }
}