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
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }
        public int  TeamStatistics { get; set; }
        [ForeignKey("TeamStatistics")]
        public TeamStatistics  Statistics { get; set; }
        public string FbFeed { get; set; }
        [NotMapped]
        public int Position { get; set; }
        public List<Formation> Formations { get; set; }
        public List<TeamRoles> Roles { get; set; }

    }
}