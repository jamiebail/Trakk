using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }
        public virtual TeamMember Scorer { get; set; }
        public int Minute { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }
}