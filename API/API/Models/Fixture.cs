using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using API.Helpers;
using API.Models;
using Newtonsoft.Json;

namespace API.Models
{
    public class Fixture
    {
        [Key]
        public int Id { get; set; }
        public int HomeId { get; set; }
        [NotMapped]
        public Team HomeTeam { get; set; }
        public int AwayId { get; set; }
        [NotMapped]
        public Team AwayTeam { get; set; }
        [NotMapped]
        public GameReport Result { get; set; }
        public string Location { get; set; }
        public int Easting { get; set; }
        public int Northing { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [NotMapped]
        public TrakkEnums.UserAvailability AttendanceState { get; set; }
        public TrakkEnums.FixtureState State { get; set; }
        [NotMapped]
        public List<TeamMember> Available { get; set; }
        [NotMapped]
        public List<TeamFixtureSetup> TeamSetups { get; set; }

        public Fixture()
        {
            TeamSetups = new List<TeamFixtureSetup>();
        }
    }


}