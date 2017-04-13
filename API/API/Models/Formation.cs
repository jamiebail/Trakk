using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace API.Models
{
    public class Formation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public string FormationJson { get; set; }
    }
}