using System.Web.Script.Serialization;
using API.Helpers;
using Trakk.Models;

namespace Trakk.Models
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
        public int PlayerId { get; set; }
        [ScriptIgnore]
        public TeamMember Player { get; set; }
        public int ReportId { get; set; }
        public TrakkEnums.Side Side { get; set; }
    }
}