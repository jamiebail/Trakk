using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Trakk.Models
{
    public class Formation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string FormationJson { get; set; }
        [Required]
        public string Name { get; set; }
    }
}