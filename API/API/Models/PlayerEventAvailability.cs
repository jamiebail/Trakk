using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class PlayerEventAvailability
    {
        [Key]
        public int Id { get; set; }
        public TrakkEnums.UserAvailability Availability { get; set; }
        public TeamMember TeamMember { get; set; }
        public int EventId { get; set; }
    }
}