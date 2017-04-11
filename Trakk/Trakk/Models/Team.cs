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
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public List<TeamMember> Members { get; set; }
        [Required]
        public Sport Sport { get; set; }
        public TeamStatistics Statistics { get; set; }
        public int Position { get; set; }

    }
}