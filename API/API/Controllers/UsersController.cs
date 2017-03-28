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
using Microsoft.AspNet.Identity;

namespace API.Controllers
{
    public class UsersController : Controller
    {
        readonly IUserLogic _userLogic = new UserLogic();
        readonly ITeamLogic _teamLogic = new TeamLogic();

        public ActionResult Get(int? id)
        {
            if (id == null)
                return Json(_userLogic.GetUsers(), JsonRequestBehavior.AllowGet);

            TeamMember member = _userLogic.GetUser(id.Value);
            member.Teams = _teamLogic.GetTeamsByUserId(id.Value);

            return Json(member, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Post(TeamMember member)
        {
            if (member == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                EntityResponse response = _userLogic.CreateUser(member);

                if (response.Success)
                {
                    return Json(response ,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(response ,JsonRequestBehavior.AllowGet);
                }
            }
            else return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Put(TeamMember member)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = _userLogic.UpdateUser(member);
                if (response.Success)
                    return Json(response, JsonRequestBehavior.AllowGet);
                else
                    return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new EntityResponse(false, "The team data provided is invalid"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeamMember member = _userLogic.GetUser(id.Value);

            if (member == null)
            {
                return Json(new EntityResponse(false, "The team record for that id doesn't exist"), JsonRequestBehavior.AllowGet);
            }

            EntityResponse response = _userLogic.DeleteUser(member);

            return Json(response.Message, JsonRequestBehavior.AllowGet);
        }


    }
}
