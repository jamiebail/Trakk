﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using API.Helpers;
using Trakk.Models;

namespace Trakk.Logic
{
    public interface IUserLogic
    {
        int GetPlayerId(IIdentity identityIn);
        Task<List<TeamMember>> GetAllUsers();
        Task<bool> CheckIfTeamAdmin(IIdentity identityIn, int teamId);
        Task<List<Team>> CheckIfTeamAdminAny(IIdentity identityIn);
        TrakkEnums.Side CheckTeamSide(TeamMember member, Fixture fixture);
        byte[] GetUserImage(int userId);
    }
}
