using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Trakk.Models;

namespace Trakk.Models
{
    public class TeamMembership
    {
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int TeamId { get; set; }
        public bool Accepted { get; set; }
    }
}