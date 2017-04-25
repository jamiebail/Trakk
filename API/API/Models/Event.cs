using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public string Title { get; set; }
        public int Attending { get; set; }
        [NotMapped]
        public List<TeamMember> Available { get; set; }
        public int Invited { get; set; }
        public TrakkEnums.EventType Type { get; set; }
        [NotMapped]
        public TrakkEnums.UserAvailability AttendanceState { get; set; }
        [NotMapped]
        public List<TeamMember> Members { get; set; }
        [NotMapped]
        public List<PlayerEventAvailability> Availabilities { get; set; }
        [NotMapped]
        public int TeamId { get; set; }
        [NotMapped]
        public Team Team { get; set; }
    }
}