using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Models;
using Newtonsoft.Json;
using Trakk.Logic;
using Trakk.Models;
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    public class PartialsController : Controller
    {
        public IAPIGetter _getter;
        public IUserLogic _userLogic;
        public InterfaceLogic interfaceLogic = new InterfaceLogic();
        public PartialsController(IAPIGetter getterIn, IUserLogic uLogicIn)
        {
            _getter = getterIn;
            _userLogic = uLogicIn;
        }
        public PartialsController()
        {
            _getter = new APIGetter();
            _userLogic = new UserLogic();
        }
        // GET: Partials
        public PartialViewResult UserCalendar()
        {
            return PartialView("CalendarPartial");
        }

        [HttpPost]
        public async Task<ActionResult> UserCalendarEvents(DateTime month)
        {
            List<Event> events = await _getter.GetUserEvents(_userLogic.GetPlayerId(User.Identity), month, false);
            Fixture fixture;
            foreach(var e in events)
                if (e.Type != TrakkEnums.EventType.Social && e.Type != TrakkEnums.EventType.Training)
                {
                    fixture = (Fixture) e;
                    e.Title = fixture.HomeTeam.Name + " v " + fixture.AwayTeam.Name;
                }

            List<CalendarEvent> calendarEvents = events.Select(_event => new CalendarEvent
            {
                id = _event.Id,
                title = _event.Title,
                start = _event.Start,
                end = _event.End,
                allDay = false
            }).ToList(); 
            return Json(calendarEvents, JsonRequestBehavior.AllowGet);
        }

        // GET: Teams
        public async Task<PartialViewResult> UserTeamList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
                return PartialView("~/Views/Partials/TeamListPartial.cshtml", member);
            return null;
        }

        // GET: Events
        public async Task<PartialViewResult> UserEventList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            EventsListViewModel vm = new EventsListViewModel(){ Events = await _getter.GetUserOtherEvents(member.Id, false)};
            vm.Events = vm.Events.Where(x => x.Type == TrakkEnums.EventType.Social || x.Type == TrakkEnums.EventType.Training).ToList();
            return PartialView("~/Views/Partials/UserEventsPartial.cshtml", vm);
        }

        // GET: Fixtures
        public async Task<PartialViewResult> UserFixtureList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            EventsListViewModel vm = new EventsListViewModel() { Fixtures = await _getter.GetUserFixtures(member.Id) };

            List<FixtureViewModel> fixtures = new List<FixtureViewModel>();
            List<TeamMember> teamAvailable = new List<TeamMember>();
            foreach (var e in vm.Fixtures)
            {
                // Run checks on fixtures before view generation to apply changes in state.
                Fixture fixture = (Fixture) e;

                if (_userLogic.CheckTeamSide(member, fixture) == TrakkEnums.Side.Home)
                {
                    var availabilities = fixture.Availabilities.Where(x => x.TeamId == fixture.HomeId);
                    teamAvailable = availabilities.Select(availability => fixture.Available.Find(x => x.Id == availability.UserId)).ToList();
                }
                else if(_userLogic.CheckTeamSide(member, fixture) == TrakkEnums.Side.Away)
                {
                    var availabilities = fixture.Availabilities.Where(x => x.TeamId == fixture.AwayId);
                    teamAvailable = availabilities.Select(availability => fixture.Available.Find(x => x.Id == availability.UserId)).ToList();
                }
                if (fixture.End < DateTime.Now)
                {
                    fixture.State = TrakkEnums.FixtureState.Finished;
                }
                fixtures.Add(new FixtureViewModel() { HomeTeam = fixture.HomeTeam, AwayTeam = fixture.AwayTeam, Fixture = fixture, Playing = teamAvailable, UserId = member.Id});
            }
            return PartialView("~/Views/Partials/UserFixturePartial.cshtml", fixtures);
        } 
        // GET: Fixtures
        public async Task<PartialViewResult> UserFixtureDetails(int id)
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            Fixture fixture = await _getter.GetFixture(new FixtureAvailabilityViewModel() {Id = id, UserId = member.Id});
            if (fixture.End < DateTime.Now)
            {
                //if(fixture.Result == null)
                //    fixture.Result = new GameReport();
                fixture.State = TrakkEnums.FixtureState.Finished;
            }
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
            TrakkEnums.Side side = 0;
            List<TeamMember> teamAvailable = new List<TeamMember>();
            foreach (Team team in member.Teams)
            {
                if (team.Id == fixture.HomeId)
                {
                    side = TrakkEnums.Side.Home;
                    var availabilities = fixture.Availabilities.Where(x => x.TeamId == team.Id && x.Availability == TrakkEnums.UserAvailability.Accepted);
                  teamAvailable = availabilities.Select(availability => fixture.Available.Find(x => x.Id == availability.UserId)).ToList();
                }
                else if (team.Id == fixture.AwayId)
                {
                    side = TrakkEnums.Side.Away;
                    var availabilities = fixture.Availabilities.Where(x => x.TeamId == team.Id && x.Availability == TrakkEnums.UserAvailability.Accepted);
                    teamAvailable = availabilities.Select(availability => fixture.Available.Find(x => x.Id == availability.UserId)).ToList();
                }
            }
            foreach (var memb in teamAvailable)
            {
                memb.Photo = _userLogic.GetUserImage(memb.Id);
            }
            if (positions != null)
            {
                foreach (var position in positions)
                {
                    position.Profile = _userLogic.GetUserImage(position.PlayerId);
                }
            }
            fixture.HomeTeam.Sport.Pitch = interfaceLogic.GetPitch(fixture.HomeTeam.Sport.Id);
            FixtureViewModel vm = new FixtureViewModel(){ HomeTeam = fixture.HomeTeam, AwayTeam = await _getter.GetTeam(fixture.AwayId), Fixture = fixture, Positions = positions, Playing = teamAvailable, UserId = member.Id, Side = side};
            if (vm.Fixture.Result != null)
            {
                vm.Fixture.Result.FixtureId = vm.Fixture.Id;
            }
           
            return PartialView("~/Views/Partials/FixtureDetailsPartial.cshtml", vm);
        }

        // GET: Fixtures
        public async Task<PartialViewResult> UserSportList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            List<Sport> sportsList = new List<Sport>();
            foreach (Team team in member.Teams)
            {
                if(!sportsList.Contains(team.Sport))
                    sportsList.Add(team.Sport);
            }
            List<Sport> sports = await _getter.GetSportList(sportsList);
            return PartialView("~/Views/Partials/UserSportsPartial.cshtml", sports);
        }

        // GET: UserPage
        public async Task<PartialViewResult> UserInfoPartial()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            return PartialView("~/Views/Partials/TeamListPartial.cshtml", member);
        }

        public async Task<PartialViewResult> UserDayEvents(DateTime date)
        {
            DateTime selectedDate = Convert.ToDateTime(date).Date;
            List<Event> events = await _getter.GetUserEvents(_userLogic.GetPlayerId(User.Identity), date,false);
            events = events.Where(x => x.Start.Value.Date == selectedDate).ToList();
            return PartialView("EventPartial", events);
        }

        public async Task<PartialViewResult> TeamDetailsPartial(int id)
        {
            Team team = await _getter.GetTeam(id);
            foreach (var member in team.Members)
            {
                member.Photo = _userLogic.GetUserImage(member.Id);
            }
            return PartialView("TeamDetailsPartial", team);
        }


        [HttpGet]
        public async Task<JsonResult> GetTeamMembers(int id)
        {
            Team team = await _getter.GetTeam(id);
            foreach (var member in team.Members)
            {
                member.Photo = _userLogic.GetUserImage(member.Id);
            }
            return Json(team.Members, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> TeamMembersPartial(int id)
        {
            Team team = await _getter.GetTeam(id);
            foreach (var member in team.Members)
            {
                member.Photo = _userLogic.GetUserImage(member.Id);
            }
            return PartialView("~/Views/Partials/TeamMembersPartial.cshtml", team.Members);
        }

        public async Task<JsonResult> GetTeamPitch(int id)
        {
            Team team = await _getter.GetTeam(id);
            if (team != null)
            {
                return Json(interfaceLogic.GetPitch(team.Sport.Id).Path, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}