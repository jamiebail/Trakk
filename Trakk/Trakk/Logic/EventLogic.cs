using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trakk.Models;

namespace Trakk.Logic
{
    public class EventLogic : IEventLogic
    {
        public List<Event> GetPrimaryEvents(List<Event> events)
        {
            List<Event> primaries = new List<Event>();
            Event positionEvent = new Fixture() { Id = -1, Start = DateTime.Now };
            events.Add(positionEvent);
            events = events.OrderBy(x => x.Start).ToList();
            int index = events.FindIndex(a => a.Id == positionEvent.Id);
            if (events.Count > 1)
            {
                // Last Element
                if (index + 1 == events.Count)
                {
                  if(events.Count == 2) { 
                        primaries.Add(events[index - 1]);
                }
                   else
                   {
                        primaries.Add(events[index - 1]);
                        primaries.Add(events[index - 2]);
                        primaries.Add(events[index - 3]);
                    }
                        return primaries;
                }
                // next event
                if (index != 0)
                {
                    primaries.Add(events[index - 1]);
                }

                primaries.Add(events[index + 1]);
                if (index + 2 < events.Count)
                {
                    primaries.Add(events[index + 2]);
                    return primaries;
                }
                return primaries;

            }
            return null;
        }
    }
}