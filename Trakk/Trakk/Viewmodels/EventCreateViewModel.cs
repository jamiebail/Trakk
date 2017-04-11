using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trakk.Models;

namespace Trakk.Viewmodels
{
    public class EventCreateViewModel
    {
        public IEnumerable<SelectListItem> UserTeams { get; set; }
        public Event Event { get; set; }
    }
    public class EventEditViewModel
    {
        public List<TeamMember> Members { get; set; }
        public List<TeamMember> AllMembers { get; set; } 
        public Event Event { get; set; }
    }

    public class EventReturnCreateViewModel
    {

        [Required]
        public int TeamId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public List<int> UserIds { get; set; }
        [Required]
        public string Comments { get; set; }
    }


    public class EventReturnEditViewModel
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public List<int> UserIds { get; set; }
        [Required]
        public string Comments { get; set; }
    }
}