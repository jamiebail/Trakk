using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Trakk.Models;

namespace Trakk.Viewmodels
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public int FixtureId { get; set; }
        [Required]
        public int HomeScore { get; set; }
        [Required]
        public int AwayScore { get; set; }
        public List<Goal> Goals  { get; set; }
        public List<Card> Cards  { get; set; }
    }
}