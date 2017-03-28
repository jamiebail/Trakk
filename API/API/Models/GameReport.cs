using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual List<Card> Cards { get; set; }
        public virtual List<Goal> Goals { get; set; }  
    }
}