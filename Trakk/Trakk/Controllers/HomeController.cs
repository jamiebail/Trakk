using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trakk.Models;
using Microsoft.AspNet.Identity;
using Trakk.Extensions;
using Trakk.Logic;
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
        {
        readonly IAPIGetter _getter = new APIGetter();
        readonly IUserLogic _userLogic = new UserLogic();
        public async Task<ActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel();
            if (User.Identity.IsAuthenticated)
            {
                // Get Player data from API using id key stored in the player's Trakk account. 
                string cookievalue;
                if (Request.Cookies["session"] != null)
                {
                    cookievalue = Request.Cookies["session"].Value;
                    if (cookievalue != _userLogic.GetPlayerId(User.Identity).ToString())
                    {
                        var httpCookie = Response.Cookies["session"];
                        if (httpCookie != null)
                            httpCookie.Value = _userLogic.GetPlayerId(User.Identity).ToString();
                    }
                }
                else
                {
                    var httpCookie = Response.Cookies["session"];
                    if (httpCookie != null)
                        httpCookie.Value = _userLogic.GetPlayerId(User.Identity).ToString();
                }
                TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
                if(member != null)
                    if (member.Id != 0)
                    {
                        hvm.Events = await _getter.GetUserEvents(member.Id, true);
                    }
            }
            return View(hvm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}