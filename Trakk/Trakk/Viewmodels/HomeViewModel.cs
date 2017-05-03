using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trakk.Models;

namespace Trakk.Viewmodels
{
    public class HomeViewModel
    {
        public TeamMember Player { get; set; }
        public List<Event> Events { get; set; } 

    }
}