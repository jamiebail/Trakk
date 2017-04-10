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

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create( EventReturnCreateViewModel newEvent)
        {
            if (ModelState.IsValid)
            {
                await _setter.CreateEvent(newEvent);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _getter.GetEvent(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        //POST: Events/Edit/5
         //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Start,End,Location,Comments,Title,Attending,Invited,Type,AttendanceState")] Event @event)
        {
            if (ModelState.IsValid)
            {
                await _setter.UpdateEvent(@event);
                return RedirectToAction("Index", "Home");
            }
            return View(@event);
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
