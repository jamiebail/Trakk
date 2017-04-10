using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Trakk.Models;

namespace Trakk.Logic
{
    public class UserLogic : IUserLogic
    {
        public IAPIGetter _getter;

        public UserLogic(IAPIGetter getter)
        {
            _getter = getter;
        }

        public UserLogic()
        {
            _getter = new APIGetter();
        }

        public int GetPlayerId(IIdentity identityIn)
        {
            var identity = (ClaimsIdentity)identityIn;
            var playerId = (identity.FindFirst("PlayerId").ToString());
            int id = int.Parse(playerId.Replace("PlayerId: ", ""));
            return id;
        }

        public async Task<List<TeamMember>> GetAllUsers()
        {
            return await _getter.GetAllUsers();
        }
    }
}