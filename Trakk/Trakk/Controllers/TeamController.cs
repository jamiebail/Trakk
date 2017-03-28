using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trakk.Logic;

namespace Trakk.Controllers
{
    [RequireHttps]
    public class TeamController : Controller
    {
        IAPIGetter _caller = new APIGetter();
        public ActionResult Manage(int id)
        {
            //caller.GetTeam()
            return View()
            ;
        }
    }
}