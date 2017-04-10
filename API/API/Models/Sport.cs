using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json;

namespace API.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public List<Team> Teams { get; set; }
    }
}