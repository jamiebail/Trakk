using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Helpers
{
    public class TrakkEnums
    {
        public enum Result
        {
            Win = 1,
            Loss = 2,
            Draw = 3
        }

        public enum Points
        {
            Win = 3,
            Draw = 1,
            Loss = 0
        }


        public enum FixtureState
        {
            New = 1,
            Finished = 0
        }


        public enum EventType
        {
            Fixture = 1,
            Social = 2,
            Training = 3
        }

        public enum Side
        {
            Home = 1,
            Away = 2
        }

        public enum UserAvailability
        {
            Accepted = 1,
            Rejected = 2,
            Maybe = 3
        }
    }
}