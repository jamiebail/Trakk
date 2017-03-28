using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;

namespace API.Logic
{
    interface ISportLogic
    {
        List<Sport> GetAllSports();
        List<Position> GetSportPositions(int? id);
    }
}
