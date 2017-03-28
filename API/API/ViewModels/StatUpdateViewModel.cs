using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;

namespace API.ViewModels
{
    public class StatUpdateViewModel
    {
        public int Id { get; set; }
        public int Result { get; set; }
        public int Goals { get; set; }
        public int Conceded { get; set; }
        public List<Card> Cards { get; set; }

    }
}