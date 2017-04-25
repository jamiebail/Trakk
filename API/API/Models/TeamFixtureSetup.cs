using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class TeamFixtureSetup
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        public int FixtureId { get; set; }
        [ForeignKey("FixtureId")]
        public Fixture Fixture { get; set; }
        public string Comments { get; set; }
        public string Positions { get; set; }
    }
}