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
                if (e.Type == TrakkEnums.EventType.Fixture)
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

        public async Task<PartialViewResult> UserDayEvents(string date)
        {
            DateTime selectedDate = Convert.ToDateTime(date).Date;
            List<Event> events = await _getter.GetUserEvents(_userLogic.GetPlayerId(User.Identity), false);
            events = events.Where(x => x.Start.Date == selectedDate).ToList();
            return PartialView("EventPartial", events);
        }
             
    }
}