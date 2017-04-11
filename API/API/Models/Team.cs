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
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public List<TeamMember> Members { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual TeamStatistics Statistics { get; set; }
        public string FbFeed { get; set; }
        [NotMapped]
        public int Position { get; set; }

    }
}