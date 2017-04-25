using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}