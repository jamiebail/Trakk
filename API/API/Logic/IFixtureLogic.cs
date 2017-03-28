using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;

namespace API.Logic
{
    interface IFixtureLogic
    {
        List<Fixture> GetFixtures();
        Fixture GetFixture(int id);
        List<Fixture> GetTeamFixtures(int id);
        EntityResponse CreateFixture(Fixture fixture);
        EntityResponse UpdateFixture(Fixture fixture);
        EntityResponse DeleteFixture(int id);
        List<Fixture> GetUserFixtures(int id);
    }
}
