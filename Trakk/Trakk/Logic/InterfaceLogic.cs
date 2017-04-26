using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trakk.Models;

namespace Trakk.Logic
{
    public class InterfaceLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public SportPitchModel GetPitch(int sportId)
        {
            return db.PitchModels.FirstOrDefault(x => x.SportId == sportId);
        }
    }
}