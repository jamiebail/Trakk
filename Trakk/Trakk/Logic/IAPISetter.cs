using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Trakk.Models;
using Trakk.Helpers;
using Trakk.Viewmodels;

namespace Trakk.Logic
{
    public interface IAPISetter
    {
        Task<EntityResponse> CreateUser(TeamMember user);
        Task<EntityResponse> UpdateUser(TeamMember user);
        Task<EntityResponse> CreateTeam(TeamReturnCreateViewModel team);
        Task<EntityResponse> UpdateTeam(Team team);
        Task<EntityResponse> CreateSport(Sport sport);
        Task<EntityResponse> UpdateSport(Sport sport);
        Task<EntityResponse> CreateFixture(Fixture fixture);
        Task<EntityResponse> UpdateFixture(Fixture fixture);
        Task<EntityResponse> UpdateEvent(Event eventUpdate);
        Task<EntityResponse> CreateEvent(EventReturnCreateViewModel newEvent);

    }
}
