using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;

namespace API.Logic
{
    interface ISportLogic
    {
        List<Sport> GetAllSports();
        List<Position> GetSportPositions(int? id);
        Sport GetSportById(int id);
        List<Team> GetTeamsBySport(Sport sportIn);
        EntityResponse UpdateSport(Sport sport);
        EntityResponse CreateSport(Sport newSport);
    }
}
