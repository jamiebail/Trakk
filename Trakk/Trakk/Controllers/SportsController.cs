using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trakk.Helpers;
using Trakk.Logic;
using Trakk.Models;

namespace Trakk.Controllers
{
    public class SportsController : Controller
    {
        public IAPIGetter _getter;
        public IAPISetter _setter;
        public IUserLogic _userLogic;
        public SportsController(IAPIGetter getter, IAPISetter setter, IUserLogic userLogic)
        {
            _getter = getter;
            _setter = setter;
            _userLogic = userLogic;
        }

        public SportsController()
        {
            _getter = new APIGetter();
            _setter = new APISetter();
            _userLogic = new UserLogic();
        }

        // GET: Sports/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = await _getter.GetSport(id.Value);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return PartialView(sport);
        }

        // GET: Sports/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Sports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                Task<EntityResponse> reponse = _setter.CreateSport(sport);
                return RedirectToAction("Index", "Home");
            }

            return View(sport);
        }

        // GET: Sports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = await _getter.GetSport(id.Value);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                _setter.UpdateSport(sport);
                return RedirectToAction("Index", "Home");
            }
            return View(sport);
        }

        //// GET: Sports/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Sport sport = db.Sports.Find(id);
        //    if (sport == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sport);
        //}

        //// POST: Sports/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Sport sport = db.Sports.Find(id);
        //    db.Sports.Remove(sport);
        //    db.SaveChanges();
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
