using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
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

        public List<Team> GetTeamsBySport(Sport sportIn)
        {
           List<Team> teams = _teamRepository.FindBy(x => x.SportId == sportIn.Id);
            return teams;
        }

        public Sport GetSportById(int id)
        {
            Sport sport = _sportRepository.FindBy(x => x.Id == id).FirstOrDefault();
            return sport;
        }

        public EntityResponse CreateSport(Sport newSport)
        {
            try
            {
                _sportRepository.Add(newSport);
                _sportRepository.Save();
                return new EntityResponse(true, newSport.Name + " created successfully.");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, newSport.Name + " creation failed: " + e.Message);
            }
            
        }
        
        public EntityResponse UpdateSport(Sport sport)
        {
            try
            {
                _sportRepository.Update(sport);
                _sportRepository.Save();
                return new EntityResponse(true, sport.Name + " updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, sport.Name + " updating failed: " + e.Message);
            }
        }
    }
}