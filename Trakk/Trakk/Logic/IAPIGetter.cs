using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trakk.Models;

namespace Trakk.Logic
{
    public interface IAPIGetter
    {
        Task<Fixture> GetFixture(int id);
        Task<List<Fixture>> GetAllFixtures();
        Task<Team> GetTeam(int id);
        Task<List<Team>> GetAllTeams(int id);
        Task<TeamMember> GetUser(int id);
        Task<List<Event>> GetUserEvents(int id, bool primary);
        Task<List<TeamMember>> GetAllUsers();
        Task<Sport> GetSport(int? id);
        Task<List<Sport>> GetAllSports();
        Task<List<Sport>> GetSportList(List<Sport> sportsList);
        Task<Event> GetEvent(int? id);
    }
}
