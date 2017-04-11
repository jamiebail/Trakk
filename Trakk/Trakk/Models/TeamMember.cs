using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Trakk.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Trakk.Models
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
        public int AccountId { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TeamMember);
        }

        public bool Equals(TeamMember obj)
        {
            return obj != null && obj.Id == this.Id;
            // Or whatever you think qualifies as the objects being equal.
        }
    }
}