using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace API.Models
{
    public class TeamStatistics
    {
        [Key]
        public int Id { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Played { get; set; }
        public int Drawn { get; set; }
        public int Points { get; set; }
        public int Goals { get; set; }
        public int Conceded { get; set; }
        public virtual ICollection<Card> Cards { get; set; }

    }
}