using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using Trakk.Logic;
using Trakk.Models;
using Trakk.Viewmodels;

namespace Trakk.Controllers
{
    public class PartialsController : Controller
    {
        public IAPIGetter _getter;
        public IUserLogic _userLogic;
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
        public async Task<ActionResult> UserCalendarEvents()
        {
            List<Event> events = await _getter.GetUserEvents(_userLogic.GetPlayerId(User.Identity), false);
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
        }

        // GET: Events
        public async Task<PartialViewResult> UserEventList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            EventsListViewModel vm = new EventsListViewModel(){ Events = await _getter.GetUserEvents(member.Id, false)};
            vm.Events = vm.Events.Where(x => x.Type == TrakkEnums.EventType.Social || x.Type == TrakkEnums.EventType.Training).ToList();
            return PartialView("~/Views/Partials/UserEventsPartial.cshtml", vm);
        }

        // GET: Fixtures
        public async Task<PartialViewResult> UserFixtureList()
        {
            TeamMember member = await _getter.GetUser(_userLogic.GetPlayerId(User.Identity));
            EventsListViewModel vm = new EventsListViewModel() { Events = await _getter.GetUserEvents(member.Id, false) };
            vm.Events = vm.Events.Where(x => x.Type != TrakkEnums.EventType.Social && x.Type != TrakkEnums.EventType.Training).ToList();
            return PartialView("~/Views/Partials/UserFixturePartial.cshtml", vm.Events);
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

        public async Task<PartialViewResult> UserDayEvents(string date)
        {
            DateTime selectedDate = Convert.ToDateTime(date).Date;
            List<Event> events = await _getter.GetUserEvents(_userLogic.GetPlayerId(User.Identity), false);
            events = events.Where(x => x.Start.Date == selectedDate).ToList();
            return PartialView("EventPartial", events);
        }

        public async Task<PartialViewResult> TeamDetailsPartial(int id)
        {
            Team team = await _getter.GetTeam(id);
            return PartialView("TeamDetailsPartial", team);
        }


        [HttpGet]
        public async Task<JsonResult> GetTeamMembers(int id)
        {
            Team team = await _getter.GetTeam(id);
            return Json(team.Members, JsonRequestBehavior.AllowGet);
        }

       
    }
}