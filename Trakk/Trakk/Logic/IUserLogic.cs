using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Trakk.Models;

namespace Trakk.Logic
{
    public interface IUserLogic
    {
        int GetPlayerId(IIdentity identityIn);
        Task<List<TeamMember>> GetAllUsers();
        Task<bool> CheckIfTeamAdmin(IIdentity identityIn, int teamId);
        Task<List<Team>> CheckIfTeamAdminAny(IIdentity identityIn);
    }
}
