using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;

namespace API.Logic
{
    public class EventLogic : IEventLogic
    {
        private readonly IRepository<Event> _eventRepository = new Repository<Event>();
        private readonly IRepository<TeamEvent> _teamEventRepository = new Repository<TeamEvent>();
        private readonly IRepository<TeamMember> _teamMemberRepository = new Repository<TeamMember>(); 
        private readonly IRepository<PlayerEventAvailability> _availabilitiesRepository = new Repository<PlayerEventAvailability>(); 
        private readonly ITeamLogic _teamLogic = new TeamLogic();
         
        public List<Event> GetUserEvents(int userId)
        {
            List<Event> userEvents = new List<Event>();
            TeamMember user = _teamMemberRepository.FindBy(x => x.Id == userId).FirstOrDefault();
            user.Teams = _teamLogic.GetTeamsByUserId(userId);
            foreach (var team in user.Teams)
            {
                userEvents.AddRange(GetTeamEvents(team.Id));
            }
            foreach (var e in userEvents)
            {
                PlayerEventAvailability pev = _availabilitiesRepository.FindBy(x => x.EventId == e.Id).FirstOrDefault();
                if (pev != null)
                    e.AttendanceState = pev.Availability;
            }
            return userEvents;
        }

        public List<Event> GetTeamEvents(int teamId)
        {
            List<TeamEvent> teamEvents = _teamEventRepository.FindBy(x => x.TeamId == teamId);
            List<Event> events = new List<Event>();
            foreach(var teamEvent in teamEvents)
                events.AddRange(_eventRepository.FindBy(x => x.Id == teamEvent.EventId));
            return events;
        }

        public Event GetEvent(int eventId)
        {
            return _eventRepository.FindBy(x => x.Id == eventId).FirstOrDefault();
        }

        public EntityResponse CreateEvent(Event newEvent)
        {
           try
            {
                _eventRepository.Add(newEvent);
                return new EntityResponse(true, "Event : " + newEvent.Title + " created successfully.");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Event : " + newEvent.Title + " creation failed: " + e.Message);
            }
        }

        public EntityResponse UpdateEvent(Event eventUpdate)
        {
            try
            {
                _eventRepository.Update(eventUpdate);
                return new EntityResponse(true, "Event : " + eventUpdate.Title + " updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Event : " + eventUpdate.Title + " update failed" + e.Message);
            }
        }

        public EntityResponse DeleteEvent(int id)
        {
            _eventRepository.Remove(_eventRepository.FindBy(x=>x.Id == id).FirstOrDefault());
            return new EntityResponse(true, "Event deleted successfully");
        }
    }
}