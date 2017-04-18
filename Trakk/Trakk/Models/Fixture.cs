using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using API.Helpers;
using Trakk.Models;
using Newtonsoft.Json;

namespace Trakk.Models
{
    public class Fixture: Event
    {
        [Key]
        public override int Id { get; set; }
        public int HomeId { get; set; }
        [NotMapped]
        public Team HomeTeam { get; set; }
        public int AwayId { get; set; }
        [NotMapped]
        public Team AwayTeam { get; set; }
        public virtual GameReport Result { get; set; }
        public int Easting { get; set; }
        public int Northing { get; set; }
        public string Location { get; set; }
        public TrakkEnums.FixtureState State { get; set; }
        public string Positions { get; set; }
        public List<TeamMember> Available { get; set; }
    }
}