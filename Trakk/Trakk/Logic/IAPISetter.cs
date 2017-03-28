using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Trakk.Models;
using Trakk.Helpers;

namespace Trakk.Logic
{
    public interface IAPISetter
    {
        Task<EntityResponse> CreateUser(TeamMember user);
        Task<EntityResponse> CreateTeam(Team team);
        Task<EntityResponse> UpdateTeam(Team team);
    }
}
