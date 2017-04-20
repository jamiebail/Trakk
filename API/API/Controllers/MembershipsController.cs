using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Logic;
using API.Models;

namespace API.Controllers
{
    public class MembershipsController : Controller
    {
        private IUserLogic _userLogic = new UserLogic();
        public ActionResult TeamMembership(int userId, int teamId, bool accepted)
        {
            TeamMembership membership = _userLogic.GetUserMembership(userId, teamId);
            EntityResponse response;
            if (accepted)
            {
                response = _userLogic.AcceptTeamInvite(membership);
            }
            else
            {
                response = _userLogic.RejectTeamInvite(membership);
            }

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
