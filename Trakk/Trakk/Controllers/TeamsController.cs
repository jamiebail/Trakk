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
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    public class TeamsController : Controller
    {
        public IAPIGetter _getter;
        public IAPISetter _setter;
        public IUserLogic _userLogic;
        public TeamsController(IAPIGetter getter, IAPISetter setter, IUserLogic userLogic)
        {
            _getter = getter;
            _setter = setter;
            _userLogic = userLogic;
        }

        public TeamsController()
        {
            _getter = new APIGetter();
            _setter = new APISetter();
            _userLogic = new UserLogic();
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
            return View(team);
        }

        // GET: Teams/Create
        public async Task<ActionResult> Create()
        {
            List<Sport> sports = await _getter.GetAllSports();
            IEnumerable<SelectListItem> selectSportsList =
            from sport in sports
            select new SelectListItem
            {
                Text = sport.Name,
                Value = sport.Id.ToString()
            };
            List<TeamMember> members = await _getter.GetAllUsers();
            IEnumerable<SelectListItem> selectUserList =
            from member in members
            select new SelectListItem
            {
                Text = member.Name,
                Value = member.Id.ToString()
            };
            return View(new TeamCreateViewModel() { Sports = selectSportsList, Team = null, Users = members, UserId = _userLogic.GetPlayerId(User.Identity) });
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(TeamReturnCreateViewModel team)
        {
            team.PlayerIDs = team.PlayerIDs.Distinct().ToList();
            if (ModelState.IsValid)
            {
                _setter.CreateTeam(team);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpGet]
        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Team team = await _getter.GetTeam(id.Value);
                if (await _userLogic.CheckIfTeamAdmin(User.Identity, team.Id))
                {
                    List<Sport> sports = await _getter.GetAllSports();
                    IEnumerable<SelectListItem> selectSportsList =
                        from sport in sports
                        select new SelectListItem
                        {
                            Text = sport.Name,
                            Value = sport.Id.ToString()
                        };
                    List<TeamMember> members = team.Members;
                    IEnumerable<SelectListItem> selectUserList =
                        from member in members
                        select new SelectListItem
                        {
                            Text = member.Name,
                            Value = member.Id.ToString()
                        };

                    return View(new TeamCreateViewModel()
                    {
                        Sports = selectSportsList,
                        Team = team,
                        Users = members
                    });
                }
            }
            return View("BadRequestView",
                new EntityResponse() {Message = "You are not an admin for this team.", Success = false});
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit(TeamReturnEditViewModel team)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, team.TeamId))
            {
                if (ModelState.IsValid)
                {
                    EntityResponse response = await _setter.UpdateTeam(team);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("BadRequestView",
                new EntityResponse() {Message = "You are not an admin for this team.", Success = false});
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, id.Value))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

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
