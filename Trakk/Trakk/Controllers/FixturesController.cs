using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Models;
using Newtonsoft.Json;
using Trakk.Helpers;
using Trakk.Logic;
using Trakk.Models;
using Trakk.Viewmodels;

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
            return null;
        }

        // GET: Teams/Create
        public async Task<ActionResult> Create()
        {
            List<Team> adminteams = await _userLogic.CheckIfTeamAdminAny(User.Identity);
            if (adminteams?.Count > 0)
            {
                int id = _userLogic.GetPlayerId(User.Identity);
                TeamMember member = await _getter.GetUser(id);
                Sport sport = await _getter.GetSport(member.Teams[0].Sport.Id);
                IEnumerable<SelectListItem> selectUserTeamsList =
                    from team in adminteams
                    select new SelectListItem
                    {
                        Text = team.Name,
                        Value = team.Id.ToString()
                    };
                IEnumerable<SelectListItem> selectAllTeamsList =
                    from team in sport.Teams
                    select new SelectListItem
                    {
                        Text = team.Name,
                        Value = team.Id.ToString()
                    };
                FixtureCreateViewModel vm = new FixtureCreateViewModel()
                {
                    UserTeams = selectUserTeamsList,
                    AllTeams = selectAllTeamsList
                };
                return View(vm);
            }


            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for any teams.", Success = false });
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create( FixtureCreateReturnViewModel fixture)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, fixture.HomeId))
            {
                if (ModelState.IsValid)
                {
                    EntityResponse response = await _setter.CreateFixture(fixture);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(new EntityResponse() { Message = "Model Invalid", Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }




        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = await _getter.GetFixture(id.Value);
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, fixture.HomeId))
            {
                fixture.HomeTeam = await _getter.GetTeam(fixture.HomeId);
                fixture.AwayTeam = await _getter.GetTeam(fixture.AwayId);
                List<PlayerPositionViewModel> positions =
                    JsonConvert.DeserializeObject<List<PlayerPositionViewModel>>(fixture.Positions);

                FixtureEditViewModel editmodel = new FixtureEditViewModel()
                {
                    Fixture = fixture,
                    Positions = positions,
                    Members = fixture.HomeTeam.Members
                };
                return View(editmodel);
            }

            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }
    


        [HttpPost]
        public async Task<ActionResult> Edit(FixtureCreateReturnViewModel fixture)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, fixture.HomeId))
            {
                if (ModelState.IsValid)
                {
                    EntityResponse reponse = await _setter.UpdateFixture(fixture);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }

        [HttpPost]
        public async Task<ActionResult> CreateFormation(Formation newFormation)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, newFormation.TeamId))
            {

                if (ModelState.IsValid)
                {

                    EntityResponse response = await _setter.CreateFormation(newFormation);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }


        public async Task<ActionResult> UpdateFormation(Formation newFormation)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, newFormation.TeamId))
            {
                EntityResponse response = await _setter.UpdateFormation(newFormation);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return View("BadRequestView" ,new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }

        public async Task<PartialViewResult> GetTeamFormations(int teamId, bool second)
        {
            Team team = await _getter.GetTeam(teamId);
            if (second)
            {
                List<Formation> formations = team.Formations.GetRange(3, team.Formations.Count - 3).ToList();
                return PartialView("~/Views/Partials/FormationListPartial.cshtml", formations);
            }
            if (team.Formations.Count > 4)
            {
                List<Formation> formations = team.Formations.GetRange(0, 4).ToList();
                return PartialView("~/Views/Partials/FormationListPartial.cshtml", formations);
            }
            else
            {
                return PartialView("~/Views/Partials/FormationListPartial.cshtml", team.Formations);
            }

        }

        public async Task<JsonResult> GetFormation(int teamId, int formationId)
        {
            Team team = await _getter.GetTeam(teamId);
            return Json(team.Formations.FirstOrDefault(x=>x.Id == formationId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> CreateAvailability(TrakkEnums.UserAvailability availability, int eventId)
        {
            if (ModelState.IsValid)
            {
                int id = _userLogic.GetPlayerId(User.Identity);
                await _setter.UpdateFixtureAvailability(new PlayerFixtureAvailability() { Availability = availability, EventId = eventId, UserId = id });
                return RedirectToAction("Index", "Home");
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
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
