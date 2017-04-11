using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.ViewModels
{ 
    public class TeamReturnCreateViewModel
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public int SportId { get; set; }
        [Required]
        public List<int> PlayerIDs { get; set; }
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
        public List<int> PlayerIDs { get; set; }
    }
}
