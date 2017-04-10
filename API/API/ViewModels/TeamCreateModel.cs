using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{ 
    public class TeamReturnCreateViewModel
    {
        public string TeamName { get; set; }
        public int SportId { get; set; }
        public List<int> PlayerIDs { get; set; }
    }
}
