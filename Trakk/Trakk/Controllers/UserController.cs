using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trakk.Logic;
using Trakk.Models;
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    [RequireHttps]
    public class UserController : Controller
    {
        private IUserLogic _userLogic;

        public UserController(IUserLogic userLogicIn)
        {
            _userLogic = userLogicIn;
        }

        public UserController()
        {
            _userLogic = new UserLogic();
        }

        public FileContentResult Photo(string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return new FileContentResult(user.ProfilePicture, "image/jpeg");
            }
            return null;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> UserList(string term)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<TeamMember> users = await _userLogic.GetAllUsers();
                List<string>  names = users.Select(x => x.Name).ToList();
                List<TeamMember> matchedUsers = users.Where(x => x.Name.ToLower().Contains(term.ToLower())).ToList();
                List<UserViewModel> matchedVM = new List<UserViewModel>();
                if (matchedUsers.Count > 0)
                {
                    foreach (TeamMember member in matchedUsers)
                    {
                        UserViewModel uvm = new UserViewModel()
                        {
                            Id = member.Id,
                            Name = member.Name,
                            
                        };
                      if (member.Teams.Count > 0)
                            foreach(var team in member.Teams)
                                if(team != null)
                            uvm.Sport = team.Sport.Name;
                        else
                            uvm.Sport = "None";
                        matchedVM.Add(uvm);

                    }
                    return Json(matchedVM, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
    }
}