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

namespace API.Controllers
{
    public class SportsController : Controller
    {
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly IUserLogic _userLogic = new UserLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_sportLogic.GetAllSports(), JsonRequestBehavior.AllowGet);
            Sport sport = _sportLogic.GetSportById(id.Value);
            sport.Teams = _sportLogic.GetTeamsBySport(sport);
            return Json(sport, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Post(Sport sport)
        {
            if (sport == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                EntityResponse response = _sportLogic.CreateSport(sport);

                if (response.Success)
                {
                    return Json(new { success = true, responseText = sport.Name + " updated successfully." },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = sport.Name + " updated successfully." },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }


        [HttpPost]
        public ActionResult Put(Sport sport)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _sportLogic.UpdateSport(sport);
                if (response.Success)
                    return Json(new { success = true, responseText = sport.Name + " updated successfully." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, responseText = sport.Name + " failed to update: " + response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = "The Team model provided is invalid" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = _sportLogic.GetSportById(id.Value);
            if (sport == null)
            {
                return Json(new { success = false, responseText = "The team record for that id doesn't exist" }, JsonRequestBehavior.AllowGet);
            }
            return Json(sport, JsonRequestBehavior.AllowGet);
        }

    }
}

