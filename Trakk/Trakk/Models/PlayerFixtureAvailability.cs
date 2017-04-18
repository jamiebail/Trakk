using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Helpers;
using Trakk.Models;

namespace API.Models
{
    public class PlayerFixtureAvailability
    {
        [Key]
        public int Id { get; set; }
        public TrakkEnums.UserAvailability Availability { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}