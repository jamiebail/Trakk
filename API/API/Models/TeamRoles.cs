using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class TeamRoles
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public TrakkEnums.TeamRole Role { get; set; }
    }
}