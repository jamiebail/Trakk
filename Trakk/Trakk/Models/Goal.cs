using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Helpers;
using Trakk.Models;

namespace Trakk.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public int ScorerId { get; set; }
        public TeamMember Scorer { get; set; }
        public int Minute { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }
}