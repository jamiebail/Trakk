using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Logic;
using API.Models;
using Trakk.Viewmodels;

namespace API.Controllers
{
    public class FormationsController : Controller
    {

        private readonly FormationLogic _formationLogic = new FormationLogic();
        private readonly IFixtureLogic _fixtureLogic = new FixtureLogic();

        [HttpPost]
        // GET: Formations/Create
        public ActionResult Post (Formation formation)
        {
            EntityResponse response = _formationLogic.CreateFormation(formation);
            if (response.Success)
            {
                return Json(new { success = true, responseText = response.Message, idreturn = response.IdReturn }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Formations/Edit/5
        [HttpPost]
        public ActionResult Put(Formation formation)
        {
            EntityResponse response = _formationLogic.UpdateFormation(formation);
            if (response.Success)
            {
                return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Formations/Delete/5
        [HttpPost]
        public ActionResult Delete(Formation formation)
        {
            EntityResponse response = _formationLogic.DeleteFormation(formation);
            if (response.Success)
            {
                return Json(new { success = true, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, responseText = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
