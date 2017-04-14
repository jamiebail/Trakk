using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
            int id = _userLogic.GetPlayerId(User.Identity);
            TeamMember member = await _getter.GetUser(id);
            Sport sport = await _getter.GetSport(member.Teams[0].Sport.Id);
            IEnumerable<SelectListItem> selectUserTeamsList =
            from team in member.Teams
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

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create( FixtureCreateReturnViewModel fixture)
        {
            if (ModelState.IsValid)
            {
                EntityResponse response = await _setter.CreateFixture(fixture);
                return RedirectToAction("Index", "Home");
            }

            return null;
        }




        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixture fixture = await _getter.GetFixture(id.Value);
            if (fixture == null)
            {
                return HttpNotFound();
            }
            fixture.HomeTeam = await _getter.GetTeam(fixture.HomeId);
            fixture.AwayTeam = await _getter.GetTeam(fixture.AwayId);
            List<PlayerPositionViewModel> positions = JsonConvert.DeserializeObject<List<PlayerPositionViewModel>>(fixture.Positions);

            FixtureEditViewModel editmodel = new FixtureEditViewModel()
            {
                Fixture = fixture,
                Positions = positions,
                Members = fixture.HomeTeam.Members
            };

            return View(editmodel);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FixtureCreateReturnViewModel fixture)
        {
            if (ModelState.IsValid)
            {
                _setter.UpdateFixture(fixture);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateFormation(Formation newFormation)
        {
            if (ModelState.IsValid)
            {
            
            EntityResponse response = await _setter.CreateFormation(newFormation);
            return Json(response, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        public async Task<JsonResult> UpdateFormation(Formation newFormation)
        {
           EntityResponse response  = await _setter.UpdateFormation(newFormation);
            return Json(response, JsonRequestBehavior.AllowGet);
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
