using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trakk.Viewmodels
{
    public class FixtureAvailabilityViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class FixtureSetupModel
    {
        public int TeamId { get; set; }
        public int FixtureId { get; set; }
    }
}