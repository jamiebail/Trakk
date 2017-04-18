using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Models;
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
        Task<EntityResponse> UpdateTeam(TeamReturnEditViewModel team);
        Task<EntityResponse> CreateSport(Sport sport);
        Task<EntityResponse> UpdateSport(Sport sport);
        Task<EntityResponse> CreateFixture(FixtureCreateReturnViewModel fixture);
        Task<EntityResponse> UpdateFixture(FixtureCreateReturnViewModel fixture);
        Task<EntityResponse> UpdateEvent(EventReturnEditViewModel eventUpdate);
        Task<EntityResponse> CreateEvent(EventReturnCreateViewModel newEvent);
        Task<EntityResponse> CreateFormation(Formation formation);
        Task<EntityResponse> UpdateFormation(Formation formation);
        Task<EntityResponse> UpdateAvailability(PlayerEventAvailability eventUpdate);
        Task<EntityResponse> UpdateFixtureAvailability(PlayerFixtureAvailability eventUpdate);
    }
}
