using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using API.Helpers;
using Newtonsoft.Json;

namespace API.Models
{
    public enum CardColour
    {
        Yellow = 1,
        Red = 2,
        TwoYellow = 3,
        Green = 4
    }
    public class Card
    {
        public int Id { get; set; }
        public CardColour CardColour { get; set; }
        [ScriptIgnore]
        public virtual TeamMember Player { get; set; }
        public int FixtureId { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }
}