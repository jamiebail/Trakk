using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModels
{
    public class EventReturnCreateViewModel
    {
        public int TeamId { get; set; }
        public List<int> UserIds { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
    }

}