using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;
using API.ViewModels;

namespace API.Logic
{
    public class EventLogic : IEventLogic
    {
        private readonly IRepository<Event> _eventRepository = new Repository<Event>();
        private readonly IRepository<TeamEvent> _teamEventRepository = new Repository<TeamEvent>();
        private readonly IRepository<TeamMember> _teamMemberRepository = new Repository<TeamMember>(); 
        private readonly IRepository<PlayerEventAvailability> _availabilitiesRepository = new Repository<PlayerEventAvailability>();
        private readonly IRepository<PrivateEvent> _privateEventRepository = new Repository<PrivateEvent>(); 
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
            List<PrivateEvent> privateEvent = _privateEventRepository.FindBy(x => x.UserId == user.Id);
            foreach (var pevent in privateEvent)
            {
                userEvents.Add(GetEvent(pevent.EventId));
            }
           
            foreach (Event e in userEvents)
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
            Event e = _eventRepository.FindBy(x => x.Id == eventId).FirstOrDefault();
            if (e != null)
            {

                List<PrivateEvent> privateInvites = _privateEventRepository.FindBy(x => x.EventId == e.Id);
                TeamEvent teamEvent = _teamEventRepository.FindBy(x => x.EventId == e.Id).FirstOrDefault();
                List<TeamMember> members = new List<TeamMember>();
                if (privateInvites == null || privateInvites.Count == 0)
                {
                    if (teamEvent != null)
                    {
                        members.AddRange(_teamLogic.GetTeamMembersByTeamId(teamEvent.TeamId));
                        e.Members = members;
                        e.TeamId = teamEvent.TeamId;
                        return e;
                    }
                }
                else
                {
                    foreach (var privateInv in privateInvites)
                    {
                        e.TeamId = privateInv.TeamId;
                        members.Add(_teamMemberRepository.FindBy(x => x.Id == privateInv.UserId).FirstOrDefault());
                    }

                    e.Members = members;
                    return e;
                }
            }
            return null;
        }

        public EntityResponse CreateEvent(EventReturnCreateViewModel newEvent)
        {
           try
           {
               Team team = _teamLogic.GetTeamById(newEvent.TeamId);
               team.Members = _teamLogic.GetTeamMembersByTeamId(newEvent.TeamId);

               Helpers.TrakkEnums.EventType type;
               Enum.TryParse(newEvent.Type, out type);
                Event eventEntity = new Event()
                {
                    Title = newEvent.Title,
                    Type = type,
                    Invited = newEvent.UserIds.Count,
                    Attending = 0,
                    Start = DateTime.Parse(newEvent.Start),
                    End = DateTime.Parse(newEvent.End),
                    Location = newEvent.Location,
                    Comments = newEvent.Comments
                };

                _eventRepository.Add(eventEntity);
                _eventRepository.Save();

               if (newEvent.UserIds.Count != team.Members.Count)
               {
                   foreach (int id in newEvent.UserIds)
                   {
                       _privateEventRepository.Add(new PrivateEvent() {EventId = eventEntity.Id, UserId = id, TeamId = team.Id});
                   }
                   _privateEventRepository.Save();
               }
               else
               {
                   _teamEventRepository.Add(new TeamEvent() {TeamId = newEvent.TeamId, EventId = eventEntity.Id});
                   _teamEventRepository.Save();
               }
               return new EntityResponse(true, "Event : " + newEvent.Title + " created successfully.");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Event : " + newEvent.Title + " creation failed: " + e.Message);
            }
        }

        public EntityResponse UpdateEvent(EventReturnEditViewModel eventUpdate)
        {
            try
            {
                Team team = _teamLogic.GetTeamById(eventUpdate.TeamId);
                team.Members = _teamLogic.GetTeamMembersByTeamId(eventUpdate.TeamId);

                Helpers.TrakkEnums.EventType type;
                Enum.TryParse(eventUpdate.Type, out type);
                Event eventEntity = new Event()
                {
                    Id = eventUpdate.EventId,
                    Title = eventUpdate.Title,
                    Type = type,
                    Invited = eventUpdate.UserIds.Count,
                    Start = DateTime.Parse(eventUpdate.Start),
                    End = DateTime.Parse(eventUpdate.End),
                    Location = eventUpdate.Location,
                    Comments = eventUpdate.Comments
                };

                _eventRepository.Update(eventEntity);
                _eventRepository.Save();

                // If a private event between a few team members, not all
                if (eventUpdate.UserIds.Count != team.Members.Count)
                {
                    TeamEvent teamEvent = _teamEventRepository.FindBy(x => x.EventId == eventUpdate.EventId && x.TeamId == eventUpdate.TeamId).FirstOrDefault();
                    if (teamEvent != null)
                    {
                        _teamEventRepository.Remove(teamEvent);
                        _teamEventRepository.Save();
                        
                    }

                    // Get Current Private Invites

                    List<PrivateEvent> privateInvites = _privateEventRepository.FindBy(x => x.EventId == eventEntity.Id);
                    if (privateInvites != null)
                    {
                        foreach (var invite in privateInvites)
                        {
                            // Delete all private event invites
                            _privateEventRepository.Remove(invite);
                        }
                        _privateEventRepository.Save();
                    }

                    // Create new private invites
                    foreach (int id in eventUpdate.UserIds)
                    {
                        PrivateEvent privateInv = _privateEventRepository.FindBy(x => x.EventId == eventUpdate.EventId && x.UserId == id).FirstOrDefault();
                        if(privateInv == null)
                            _privateEventRepository.Add(new PrivateEvent() { EventId = eventEntity.Id, UserId = id, TeamId = team.Id});
                    }
                    _privateEventRepository.Save();
                }
                // Team events, all members will receive. 
                else
                {
                    List<PrivateEvent> privateInvites = _privateEventRepository.FindBy(x => x.EventId == eventEntity.Id);
                    if (privateInvites != null)
                    {
                        foreach (var invite in privateInvites)
                        {
                            _privateEventRepository.Remove(invite);
                        }
                        _privateEventRepository.Save();
                    }
                    TeamEvent teamEvent = _teamEventRepository.FindBy(x => x.EventId == eventUpdate.EventId && x.TeamId == eventUpdate.TeamId).FirstOrDefault();
                    if (teamEvent == null)
                    {
                        _teamEventRepository.Add(new TeamEvent() {TeamId = eventUpdate.TeamId, EventId = eventEntity.Id});
                        _teamEventRepository.Save();
                    }
                }
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