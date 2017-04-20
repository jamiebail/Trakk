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
    public class GameReportsController : Controller
    {
        private readonly IFixtureLogic _fixtureLogic = new FixtureLogic();
        private readonly IReportLogic _reportLogic = new ReportLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly IUserLogic _userLogic = new UserLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_reportLogic.GetAllReports(), JsonRequestBehavior.AllowGet);

            return Json(_reportLogic.GetReport(id.Value), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Post(GameReport report)
        {
            if (report == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                EntityResponse response = _reportLogic.CreateReport(report);

                if (response.Success)
                {
                    return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }


        [HttpPost]
        public ActionResult Put(GameReport report)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _reportLogic.UpdateReport(report);
                if (response.Success)
                    return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = "The fixture model provided is invalid" }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "The team record for that id doesn't exist" }, JsonRequestBehavior.AllowGet);
            }
            return Json(team, JsonRequestBehavior.AllowGet);
        }

    }
}
