using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trakk.Models;

namespace Trakk.Viewmodels
{
    public class FixtureCreateViewModel
    {
        public Fixture Fixture { get; set; }
        public IEnumerable<SelectListItem> UserTeams { get; set; }
        public IEnumerable<SelectListItem> AllTeams { get; set; }
    }
}