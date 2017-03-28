using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trakk.Models;
using Trakk.Logic;

namespace Trakk.Controllers
{
    [RequireHttps]
    public class FixturesController : Controller
    {
        private readonly IAPIGetter _callerGet = new APIGetter();
        private readonly IUserLogic _userLogic = new UserLogic();

        public  Task<ActionResult> Index()
        {
            return null;
        }
    }
}