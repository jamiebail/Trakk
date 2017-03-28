using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trakk.Models
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
    }
}