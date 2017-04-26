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
using API.Repositories;
using API.Viewmodels;
using API.ViewModels;
using Trakk.Viewmodels;

namespace API.Controllers
{
    public class FixturesController : Controller
    {
        private readonly IFixtureLogic _fixtureLogic = new FixtureLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly IUserLogic _userLogic = new UserLogic();
        private readonly IReportLogic _reportLogic = new ReportLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_fixtureLogic.GetFixtures(), JsonRequestBehavior.AllowGet);
            Fixture fixture = _fixtureLogic.GetFixture(id.Value);
            return Json(fixture, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetWithAvailability(FixtureAvailabilityViewModel fixtureRequest)
        {
            if (fixtureRequest == null)
                return null;
            Fixture fixture = _fixtureLogic.GetFixture(fixtureRequest.Id);
            var attendance =
                _fixtureLogic.GetFixtureAvailability(fixture.Id, fixtureRequest.UserId);
            if (attendance != null)
                fixture.AttendanceState = attendance.Availability;
            return Json(fixture, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Member(int? id, DateTime? month)
        {
            if (id == null)
                return null;
            return Json(_fixtureLogic.GetUserFixtures(id.Value, month), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Post(FixtureCreateReturnViewModel fixture)
        {
            if (fixture == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                EntityResponse response = _fixtureLogic.CreateFixture(fixture);

                if (response.Success)
                {
                    return Json(new { success = true, responseText = response.Message}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }


        [HttpPost]
        public ActionResult Put(FixtureCreateReturnViewModel fixture)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _fixtureLogic.UpdateFixture(fixture);
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


        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _fixtureLogic.DeleteFixture(id);
            return Json(new { success = true, responseText = "The fixture model provided has been deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAvailability(AvailabilityViewModel availabilityIn)
        {
            if (ModelState.IsValid)
            {
                PlayerFixtureAvailability availability = new PlayerFixtureAvailability()
                {
                    Availability = availabilityIn.Availability,
                    EventId = availabilityIn.EventId,
                    UserId = availabilityIn.UserId,
                    TeamId = availabilityIn.TeamId
                };

                EntityResponse response = _userLogic.UpdateFixtureAvailability(availability);
                if (response.Success)
                {
                    return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "The fixture model provided is invalid" }, JsonRequestBehavior.AllowGet);
        }
    }
}

