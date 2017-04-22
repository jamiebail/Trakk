using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using Trakk.Models;

namespace Trakk.Viewmodels
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
        public int UsersTeamId { get; set; }
        public int OpponentsId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public string Positions { get; set; }
        public string Location { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }

    public class FixtureEditViewModel
    {
        public Fixture Fixture { get; set; }
        public List<PlayerPositionViewModel> Positions { get; set; }
        public List<TeamMember> Members { get; set; }
    }

    public class FixtureViewModel
    {
        public int UserId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Fixture Fixture { get; set; }
        public List<PlayerPositionViewModel> Positions { get; set; }
        public List<TeamMember> Playing { get; set; } 
    }
}