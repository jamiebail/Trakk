using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class TeamEvent
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int EventId { get; set; }
    }
}