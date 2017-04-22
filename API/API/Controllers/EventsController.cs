using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Logic;
using API.Models;
using API.ViewModels;

namespace API.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
         private readonly IFixtureLogic _fixtureLogic = new FixtureLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly IUserLogic _userLogic = new UserLogic();
        private readonly IEventLogic _eventLogic = new EventLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return null;

            return Json(_eventLogic.GetEvent(id.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Member(int? id, DateTime? month)
        {
            if (id == null)
                return null;

            return Json(_eventLogic.GetUserEvents(id.Value, month).OrderBy(x => x.Start), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MemberMonth(int? id, DateTime month)
        {
            if (id == null)
                return null;

            return Json(_eventLogic.GetUserEvents(id.Value, month ).OrderBy(x => x.Start), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Post(EventReturnCreateViewModel newEvent)
        {
            if (newEvent == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                EntityResponse response = _eventLogic.CreateEvent(newEvent);

                if (response.Success)
                {
                    return Json(new { success = true, responseText = response.Message}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }


        [HttpPost]
        public ActionResult Put(EventReturnEditViewModel eventUpdate)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _eventLogic.UpdateEvent(eventUpdate);
                if (response.Success)
                    return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, responseText = "The fixture model provided is invalid" }, JsonRequestBehavior.AllowGet);
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

        public ActionResult CreateAvailability(AvailabilityViewModel availabilityIn)
        {
            if (ModelState.IsValid)
            {
                PlayerEventAvailability availability = new PlayerEventAvailability()
                {
                    Availability = availabilityIn.Availability,
                    EventId = availabilityIn.EventId,
                    UserId = availabilityIn.UserId
                };

               EntityResponse response = _userLogic.UpdateAvailability(availability);
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