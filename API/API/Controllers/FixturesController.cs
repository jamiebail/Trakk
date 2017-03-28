﻿using System;
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

namespace API.Controllers
{
    public class FixturesController : Controller
    {
        private readonly IFixtureLogic _fixtureLogic = new FixtureLogic();
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly ISportLogic _sportLogic = new SportLogic();
        private readonly IUserLogic _userLogic = new UserLogic();

        [HttpGet]
        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_fixtureLogic.GetFixtures(), JsonRequestBehavior.AllowGet);

            return Json(_fixtureLogic.GetFixture(id.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Member(int? id)
        {
            if (id == null)
                return null;
            return Json(_fixtureLogic.GetUserFixtures(id.Value), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Post(Fixture fixture)
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
        public ActionResult Put(Fixture fixture)
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

