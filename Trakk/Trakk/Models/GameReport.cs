using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trakk.Models;

namespace Trakk.Models
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