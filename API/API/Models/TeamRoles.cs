using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.Models
{
    public class TeamRoles
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public TeamMember User { get; set; }
        public TrakkEnums.TeamRole Role { get; set; }
    }
}