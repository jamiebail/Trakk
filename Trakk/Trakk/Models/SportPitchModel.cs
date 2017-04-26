using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trakk.Models
{
    public class SportPitchModel
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public string Path { get; set; }
    }
}