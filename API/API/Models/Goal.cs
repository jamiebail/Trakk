using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public int ScorerId { get; set; }
        [NotMapped]
        public TeamMember Scorer { get; set; }
        public int Minute { get; set; }
        public TrakkEnums.Side Side { get; set; }
        public int ReportId { get; set; }
    }
}