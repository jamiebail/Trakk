using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Helpers;
using API.Models;
using API.Viewmodels;

namespace API.Logic
{
    interface IFixtureLogic
    {
        List<Fixture> GetFixtures();
        Fixture GetFixture(int id);
        List<Fixture> GetTeamFixtures(int id);
        EntityResponse CreateFixture(FixtureCreateReturnViewModel fixture);
        EntityResponse UpdateFixture(Fixture fixture);
        EntityResponse DeleteFixture(int id);
        List<Fixture> GetUserFixtures(int id);
        List<TeamMember> GetAvailableForFixture(int fixtureId);
        PlayerFixtureAvailability GetFixtureAvailability(int fixtureId, int userId);
    }
}
