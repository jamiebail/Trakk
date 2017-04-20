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
        public List<Card> Cards { get; set; }
        public List<Goal> Goals { get; set; }
        public int FixtureId { get; set; }

        public GameReport()
        {
            Cards = new List<Card>();
            Goals = new List<Goal>();
        }
    }

}