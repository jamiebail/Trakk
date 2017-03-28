using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Trakk.Extensions
{
        public static class IdentityExtensions
        {
            public static string GetTeamId(this IIdentity identity)
            {
                var claim = ((ClaimsIdentity)identity).FindFirst("TeamId");
                return (claim != null) ? claim.Value : string.Empty;
            }

        public static string SetTeamId(this IIdentity identity)
            {
                var claim = ((ClaimsIdentity)identity).FindFirst("TeamId");
                return (claim != null) ? claim.Value : string.Empty;
            }
        }  
}