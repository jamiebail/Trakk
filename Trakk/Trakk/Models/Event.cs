using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;

namespace Trakk.Models
{
    public class Event: IEvent
    {
        public virtual int Id { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public string Title { get; set; }
        public int Attending { get; set; }
        public int Invited { get; set; }
        public TrakkEnums.EventType Type { get; set; }
        public TrakkEnums.UserAvailability AttendanceState { get; set; }

    }
}