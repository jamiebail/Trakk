using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace API.Models
{
    public class GameReport
    {
        [Key]
        public int Id { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        [NotMapped]
        public List<Card> Cards { get; set; }
        [NotMapped]
        public List<Goal> Goals { get; set; }  
        public int FixtureId { get; set; }
    }
}