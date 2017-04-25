using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class PlayerFixtureAvailability : IAvailability
    {
        [Key]
        public int Id { get; set; }
        public TrakkEnums.UserAvailability Availability { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TeamId { get; set; }

        [ForeignKey("EventId")]
        public Fixture Fixture { get; set; }

        [ForeignKey("UserId")]
        public TeamMember User { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}