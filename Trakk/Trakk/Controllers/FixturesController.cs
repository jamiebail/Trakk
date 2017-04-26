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
        public InterfaceLogic _interfaceLogic { get; set; }
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
            _interfaceLogic = new InterfaceLogic();
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
                List<Team> teams = await _getter.GetAllTeams();
                IEnumerable<SelectListItem> selectUserTeamsList =
                    from team in adminteams
                    select new SelectListItem
                    {
                        Text = team.Name,
                        Value = team.Id.ToString()
                    };
                IEnumerable<SelectListItem> selectAllTeamsList =
                    from team in teams
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
                vm.Fixture = new Fixture();
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
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, fixture.UsersTeamId))
            {
                if (ModelState.IsValid)
                {
                    if (fixture.Side == TrakkEnums.Side.Home)
                    {
                        fixture.HomeId = fixture.UsersTeamId;
                        fixture.AwayId = fixture.OpponentsId;
                    }
                    else
                    {
                        fixture.HomeId = fixture.OpponentsId;
                        fixture.AwayId = fixture.UsersTeamId;
                    }
                EntityResponse response = await _setter.CreateFixture(fixture);
                    return Json(response, JsonRequestBehavior.AllowGet);
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
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            Fixture fixture = await _getter.GetFixture(id.Value);

            int teamId = 0; 
            foreach (var team in member.Teams)
            {
                if (team.Id == fixture.HomeId)
                {
                    teamId = fixture.HomeId;
                }
                else if(team.Id == fixture.AwayId)
                {
                    teamId = fixture.AwayId;
                }
            }
            if(teamId > 0) {
                if (await _userLogic.CheckIfTeamAdmin(User.Identity, teamId))
                {
                    fixture.HomeTeam = await _getter.GetTeam(fixture.HomeId);
                    fixture.AwayTeam = await _getter.GetTeam(fixture.AwayId);
                    if (fixture.TeamSetups != null)
                    {
                        foreach (Team t in member.Teams)
                        {
                            foreach (var setup in fixture.TeamSetups)
                            {
                                if (t.Id == setup.TeamId)
                                {
                                    // We know the team
                                    fixture.Positions = setup.Positions;
                                    fixture.Comments = setup.Comments;
                                }
                            }
                        }
                    }
                    List<PlayerPositionViewModel> positions = null;
                    if (fixture.Positions != null)
                    {

                        positions = JsonConvert.DeserializeObject<List<PlayerPositionViewModel>>(fixture.Positions);
                    }
                    FixtureEditViewModel editmodel = new FixtureEditViewModel()
                    {
                        Fixture = fixture,
                        Positions = positions,
                        Members = fixture.HomeTeam.Members
                    };
                    foreach (Team team in member.Teams)
                    {
                        if (team.Id == fixture.HomeId)
                        {
                            editmodel.Side = TrakkEnums.Side.Home;
                        }
                        else if (team.Id == fixture.AwayId)
                        {
                            editmodel.Side = TrakkEnums.Side.Away;
                        }
                    }
                    foreach (var memb in editmodel.Members)
                    {
                        memb.Photo = _userLogic.GetUserImage(memb.Id);
                    }
                    if (editmodel.Positions != null)
                    {
                        foreach (var position in editmodel.Positions)
                        {
                            position.Profile = _userLogic.GetUserImage(position.PlayerId);
                        }
                    }
                    fixture.HomeTeam.Sport.Pitch = _interfaceLogic.GetPitch(fixture.HomeTeam.Sport.Id);
                    return View(editmodel);
                }
            }

            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }
    


        [HttpPost]
        public async Task<ActionResult> Edit(FixtureCreateReturnViewModel fixture)
        {

                TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
                int teamId = 0;
                foreach (Team t in member.Teams)
                {
                    if (t.Id == fixture.HomeId)
                    {
                        fixture.Side = TrakkEnums.Side.Home;
                        teamId = fixture.HomeId;
                    }
                    else if (t.Id == fixture.AwayId)
                    {
                        fixture.Side = TrakkEnums.Side.Away;
                        teamId = fixture.AwayId;
                    }
                }
                if (teamId > 0)
                {
                    if (await _userLogic.CheckIfTeamAdmin(User.Identity, teamId))
                    {
                        if (ModelState.IsValid)
                        {


                            EntityResponse reponse = await _setter.UpdateFixture(fixture);
                            return Json(reponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return View("BadRequestView",
                        new EntityResponse() {Message = "You are not an admin for this team.", Success = false});
                }
                return View("BadRequestView", new EntityResponse() {Message = "Id of team not found", Success = false});
  
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
                TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
                Fixture fixture = await _getter.GetFixture(eventId);
                TrakkEnums.Side side = _userLogic.CheckTeamSide(member, fixture);
                int teamId = 0;

                if (side == TrakkEnums.Side.Home)
                {
                    teamId = fixture.HomeId;
                }
                if(side == TrakkEnums.Side.Away)
                {
                    teamId = fixture.AwayId;
                }
              
                await _setter.UpdateFixtureAvailability(new PlayerFixtureAvailability() { Availability = availability, EventId = eventId, UserId = member.Id, TeamId = teamId});
                return RedirectToAction("Index", "Home");
            }
            return View("BadRequestView", new EntityResponse() { Message = "Model submitted invalid", Success = false });
        }

        public async Task<ActionResult> CreateReport(ReportViewModel reportIn)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, reportIn.TeamId))
            {
                if (ModelState.IsValid)
                {
                    TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
                    Fixture fixture = await _getter.GetFixture(reportIn.FixtureId);
                    if (reportIn.TeamId == fixture.HomeId)
                    {
                        foreach (var card in reportIn.Cards)
                        {
                            card.Side = TrakkEnums.Side.Home;
                        }
                        foreach (var goal in reportIn.Goals)
                        {
                            goal.Side = TrakkEnums.Side.Home;
                        }
                    }
                    else if (reportIn.TeamId == fixture.AwayId)
                    {
                        int score = reportIn.AwayScore;
                        reportIn.AwayScore = reportIn.HomeScore;
                        reportIn.HomeScore = score;

                        if(reportIn.Cards != null)
                            foreach (var card in reportIn.Cards)
                            {
                                card.Side = TrakkEnums.Side.Away;
                            }
                        if(reportIn.Goals != null)
                            foreach (var goal in reportIn.Goals)
                            {
                                goal.Side = TrakkEnums.Side.Away;
                            }
                    }
                    GameReport report = new GameReport() {AwayScore = reportIn.AwayScore, FixtureId = reportIn.FixtureId, HomeScore = reportIn.HomeScore, Cards = reportIn.Cards, Goals = reportIn.Goals};
      
                    await _setter.CreateReport(report);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("BadRequestView", new EntityResponse() { Message = "You are not an admin for this team.", Success = false });
        }
        public async Task<ActionResult> UpdateReport(ReportViewModel reportIn)
        {
            if (await _userLogic.CheckIfTeamAdmin(User.Identity, reportIn.TeamId))
            {
                if (ModelState.IsValid)
                {
                    GameReport report = new GameReport() {AwayScore = reportIn.AwayScore, HomeScore = reportIn.HomeScore, Cards = reportIn.Cards, Goals = reportIn.Goals};
                    int id = _userLogic.GetPlayerId(User.Identity);
                    await _setter.UpdateReport(report);
                    return RedirectToAction("Index", "Home");
                }
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

        public async Task<ActionResult> DeleteFixture(int? fixtureId)
        {
            if (fixtureId != null)
            {
                Fixture fixture = await _getter.GetFixture(fixtureId.Value);
                if (fixture != null)
                {
                    TrakkEnums.Side side =
                        _userLogic.CheckTeamSide(await _getter.GetUser(_userLogic.GetPlayerId(User.Identity)), fixture);
                    var teamId = 0;
                    if (side == TrakkEnums.Side.Home)
                        teamId = fixture.HomeId;
                    else
                        teamId = fixture.AwayId;
                    if (await _userLogic.CheckIfTeamAdmin(User.Identity, teamId))
                    {
                        if (ModelState.IsValid)
                        {
                            EntityResponse response = await _setter.DeleteFixture(fixtureId.Value);
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return View("BadRequestView", new EntityResponse() {Message = "You are not an admin for this team.", Success = false});
            }
             return View("BadRequestView", new EntityResponse() { Message = "Model submitted invalid", Success = false });
        }
    }
}
