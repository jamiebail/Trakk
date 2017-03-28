using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Trakk.Logic
{
    public class UserLogic : IUserLogic
    {
        public int GetPlayerId(IIdentity identityIn)
        {
            var identity = (ClaimsIdentity)identityIn;
            var playerId = (identity.FindFirst("PlayerId").ToString());
            int id = int.Parse(playerId.Replace("PlayerId: ", ""));
            return id;
        }
    }
}