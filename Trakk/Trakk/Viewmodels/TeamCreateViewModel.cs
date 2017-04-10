using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trakk.Models;

namespace Trakk.Viewmodels
{
    public class TeamCreateViewModel
    {
        public IEnumerable<SelectListItem> Sports { get; set; }
        public Team Team { get; set; }
        public List<TeamMember> Users { get; set; } 
    }

    public class TeamReturnCreateViewModel
    {
        public string TeamName { get; set; }
        public int SportId { get; set; }
        public List<int> PlayerIDs { get; set; } 
    }
}