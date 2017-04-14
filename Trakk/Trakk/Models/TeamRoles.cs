using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;

namespace Trakk.Models
{
    public class TeamRoles
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public TrakkEnums.TeamRole Role { get; set; }
    }
}