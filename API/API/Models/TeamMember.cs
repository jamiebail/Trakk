using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using API.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace API.Models
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public List<Team> Teams { get; set; } 
        public int Score { get ; set; }
        public virtual Position Position { get; set; }
    }
}