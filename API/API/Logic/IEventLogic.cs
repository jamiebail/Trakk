using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;
using API.ViewModels;

namespace API.Logic
{
    interface IEventLogic
    {
        List<Event> GetUserEvents(int userId);
        List<Event> GetTeamEvents(int teamId);
        Event GetEvent(int eventId);
        EntityResponse CreateEvent(EventReturnCreateViewModel newEvent);
        EntityResponse UpdateEvent(Event eventUpdate);
        EntityResponse DeleteEvent(int id);
    }
}
