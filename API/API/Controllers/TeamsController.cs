using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Logic;
using API.Models;
using API.ViewModels;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly IUserLogic _userLogic = new UserLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_teamLogic.GetAllTeams(), JsonRequestBehavior.AllowGet);

            Team team = _teamLogic.GetTeamById(id.Value);
            team.Members = _teamLogic.GetTeamMembersByTeamId(id.Value);

            return Json(team, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult Post(TeamReturnCreateViewModel team)
        {
            if (team == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                Team newTeam = new Team
                {
                    Sport = _sportLogic.GetSportById(team.SportId),
                    Name = team.TeamName,
                    Statistics = new TeamStatistics()
                };

                EntityResponse response = _teamLogic.CreateTeam(newTeam);
                int teamId = newTeam.Id;
                foreach (int member in team.PlayerIDs)
                {
                    _userLogic.SetUserTeam(member, teamId);
                }

                if (response.Success)
                {
                    return Json(new {success = true, responseText = newTeam.Name + " updated successfully."},
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new {success = false, responseText = newTeam.Name + " updated successfully."},
                        JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }


        [HttpPost]
        public ActionResult Put(Team team)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _teamLogic.UpdateTeam(team);
                if (response.Success)
                    return Json(new {success = true, responseText = team.Name + " updated successfully."}, JsonRequestBehavior.AllowGet);
                else
                    return Json(new {success = false, responseText = team.Name + " failed to update: " + response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {success = false, responseText = "The Team model provided is invalid"}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = _teamLogic.GetTeamById(id.Value);
            if (team == null)
            {
                return Json(new {success = false, responseText = "The team record for that id doesn't exist"}, JsonRequestBehavior.AllowGet);
            }
            return Json(team, JsonRequestBehavior.AllowGet);
        }

    }
}
