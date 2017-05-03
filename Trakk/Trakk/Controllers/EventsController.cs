using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Models;
using Trakk.Helpers;
using Trakk.Logic;
using Trakk.Models;
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    public class EventsController : Controller
    {
        public IAPIGetter _getter;
        public IAPISetter _setter;
        public IUserLogic _userLogic;
        public EventsController(IAPIGetter getter, IAPISetter setter, IUserLogic userLogic)
        {
            _getter = getter;
            _setter = setter;
            _userLogic = userLogic;
        }

        public EventsController()
        {
            _getter = new APIGetter();
            _setter = new APISetter();
            _userLogic = new UserLogic();
        }

        //// GET: Events/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = await db.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        // GET: Events/Create
        public async Task<ActionResult> Create()
        {
            List<Team> adminteams = await _userLogic.CheckIfTeamAdminAny(User.Identity);
            if (adminteams?.Count > 0)
            {
                int id = _userLogic.GetPlayerId(User.Identity);
                TeamMember member = await _getter.GetUser(id);
                IEnumerable<SelectListItem> selectSportsList =
                    from team in member.Teams
                    select new SelectListItem
                    {
                        Text = team.Name,
                        Value = team.Id.ToString()
                    };
                EventCreateViewModel vm = new EventCreateViewModel()
                {
                    UserTeams = selectSportsList
                };
                return PartialView("~/Views/Events/Create.cshtml", vm);
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for any teams.", Success = false });
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create( EventReturnCreateViewModel newEvent)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, newEvent.TeamId))
            {
                if (ModelState.IsValid)
                {
                    EntityResponse reponse = await _setter.CreateEvent(newEvent);
                    return Json(reponse);
                }
                else
                {
                    return Json(new {success = false, message="Event submitted is invalid, check inputs."});
                }
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event ev = await _getter.GetEvent(id.Value);
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, ev.TeamId))
            {
                Team team = await _getter.GetTeam(ev.TeamId);
                List<TeamMember> teamMembers = team.Members;
                teamMembers.RemoveAll(item => ev.Members.Contains(item));
                EventEditViewModel vm = new EventEditViewModel()
                {
                    Event = ev,
                    Members = ev.Members,
                    AllMembers = teamMembers
                };
                return View(vm);
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }

        //POST: Events/Edit/5
         //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "EventId, TeamId,Start,End,Location,Comments,Title,Invited,Type,UserIds")] EventReturnEditViewModel @event)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, @event.TeamId))
            {
                if (ModelState.IsValid)
                {
                    EntityResponse response = await _setter.UpdateEvent(@event);
                    if (response.Success)
                        return RedirectToAction("Index", "Home");
                }
                return Json(new EntityResponse() { Message = "Model Invalid", Success = false }, JsonRequestBehavior.AllowGet);
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }


        [HttpPost]
        public async Task<ActionResult> CreateAvailability(TrakkEnums.UserAvailability availability, int eventId)
        {
                if (ModelState.IsValid)
                {
                    int id = _userLogic.GetPlayerId(User.Identity);
                    await _setter.UpdateAvailability(new PlayerEventAvailability(){Availability = availability, EventId = eventId, UserId = id});
                    return RedirectToAction("Index", "Home");
                }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }

        //// GET: Events/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = await db.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        //// POST: Events/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Event @event = await db.Events.FindAsync(id);
        //    db.Events.Remove(@event);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
