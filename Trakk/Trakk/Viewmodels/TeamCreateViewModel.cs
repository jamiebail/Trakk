using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<TeamRoles> Roles { get; set; }
        public int UserId { get; set; }
    }

    public class TeamReturnCreateViewModel
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public int SportId { get; set; }
        [Required]
        public List<TeamRoles> Roles { get; set; } 
    }

    public class TeamReturnEditViewModel
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public int SportId { get; set; }
        [Required]
        public List<TeamRoles> Roles { get; set; }
    }
}