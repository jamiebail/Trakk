using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Repositories;

namespace API.Logic
{
    public class SportLogic : ISportLogic
    {
        private readonly IRepository<TeamMember> _userRepository = new Repository<TeamMember>();
        private readonly IRepository<Team> _teamRepository = new Repository<Team>();
        private readonly IRepository<Sport> _sportRepository = new Repository<Sport>(); 
        private readonly IRepository<Position> _positionRepository = new Repository<Position>(); 
        public List<Sport> GetAllSports()
        {
            return _sportRepository.GetAll();
        }
        public List<Position> GetSportPositions(int? id)
        {
            return _positionRepository.FindBy(x => x.SportId == id);
        }
    }
}