﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trakk.Logic;
using Trakk.Models;

namespace Trakk.Controllers
{
    public class FixturesController : Controller
    {
        public IAPIGetter _getter;
        public IAPISetter _setter;
        public IUserLogic _userLogic;
        public FixturesController(IAPIGetter getter, IAPISetter setter, IUserLogic userLogic)
        {
            _getter = getter;
            _setter = setter;
            _userLogic = userLogic;
        }

        public FixturesController()
        {
            _getter = new APIGetter();
            _setter = new APISetter();
        }



        // GET: Teams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await _getter.GetTeam(id.Value);
            if (team == null)
            {
                return HttpNotFound();
            }
            return null;
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return PartialView("~/Views/Fixtures/Create.cshtml");
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await _getter.GetTeam(id.Value);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                _setter.UpdateTeam(team);
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
