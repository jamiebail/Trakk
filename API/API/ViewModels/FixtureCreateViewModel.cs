﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Models;

namespace API.Viewmodels
{
    public class FixtureCreateViewModel
    {
        public Fixture Fixture { get; set; }
        public IEnumerable<SelectListItem> UserTeams { get; set; }
        public IEnumerable<SelectListItem> AllTeams { get; set; }
    }

    public class FixtureCreateReturnViewModel
    {
        public int Id { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public string Positions { get; set; }
        public string Location { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }
}