using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;
using API.ViewModels;

namespace API.Logic
{
    interface ITeamLogic
    {
        List<Team> GetAllTeams();
        Team GetTeamById(int id);
        //TeamStatistics GetTeamStats(int id);
        EntityResponse UpdateTeamStatistics(StatUpdateViewModel update);
        List<TeamMember> GetTeamMembersByTeamId(int id);
        EntityResponse UpdateTeam(Team team);
        EntityResponse CreateTeam(Team team);
        List<Team> GetTeamsByUserId(int id);
    }
}
