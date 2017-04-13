using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;
using API.ViewModels;

namespace API.Logic
{
    public class FormationLogic
    {
        private IRepository<Formation> _formationLogic = new Repository<Formation>();
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly IUserLogic _userLogic = new UserLogic();

        public List<Formation> GetTeamFormations(int teamId)
        {
            return _formationLogic.FindBy(x => x.TeamId == teamId);
        }

        public EntityResponse CreateFormation(Formation formation)
        {
            try
            {
                _formationLogic.Add(formation);
                _formationLogic.Save();

                return new EntityResponse(true, "Event : " + formation.Name + " created successfully.", formation.Id);
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Event : " + formation.Name + " creation failed: " + e.Message);
            }
        }

        public EntityResponse UpdateFormation(Formation formation)
        {
            try
            {
                _formationLogic.Update(formation);
                _formationLogic.Save();
                return new EntityResponse(true, "Event : " + formation.Name + " updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Event : " + formation.Name + " update failed" + e.Message);
            }
        }

        public EntityResponse DeleteFormation(Formation formation)
        {
            _formationLogic.Remove(formation);
            _formationLogic.Save();
            return new EntityResponse(true, "Event deleted successfully");
        }
    }
}