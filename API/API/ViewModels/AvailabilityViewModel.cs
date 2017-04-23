using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;

namespace API.ViewModels
{
    public class AvailabilityViewModel
    {
        public int Id { get; set; }
        public TrakkEnums.UserAvailability Availability { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TeamId { get; set; }
    }
}