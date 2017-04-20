using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;
using API.Viewmodels;

namespace API.Logic
{
    public class FixtureLogic: IFixtureLogic
    {
        private readonly IRepository<Fixture> _fixtureRepository = new Repository<Fixture>(); 
        private readonly IRepository<Team> _teamRepository = new Repository<Team>(); 
        private readonly IRepository<PlayerFixtureAvailability> _avilabilityRepository = new Repository<PlayerFixtureAvailability>(); 
        private readonly ITeamLogic _teamLogic = new TeamLogic();
        private readonly IReportLogic _reportLogic = new ReportLogic();
        private readonly IUserLogic _userLogic = new UserLogic();
        public List<Fixture> GetFixtures()
        {
            return _fixtureRepository.GetAll();
        }

        public Fixture GetFixture(int id)
        {
            Fixture fixture = _fixtureRepository.FindBy(x => x.Id == id).FirstOrDefault();
            if (fixture != null)
            {
                fixture.HomeTeam = _teamRepository.FindBy(x => x.Id == fixture.HomeId).FirstOrDefault();
                fixture.AwayTeam = _teamRepository.FindBy(x => x.Id == fixture.AwayId).FirstOrDefault();
                fixture.Available = GetAvailableForFixture(fixture.Id);
                fixture.Result = _reportLogic.GetFixtureReport(fixture.Id);
                return fixture;
            }
            else return null;
            
        }

        public List<Fixture> GetTeamFixtures(int id)
        {
            List<Fixture> fixtures = _fixtureRepository.FindBy(x => x.HomeId == id || x.AwayId == id);
            foreach (var fixture in fixtures)
            {
                fixture.HomeTeam = _teamLogic.GetTeamById(fixture.HomeId);
                fixture.AwayTeam = _teamLogic.GetTeamById(fixture.AwayId);
                fixture.Available = GetAvailableForFixture(fixture.Id);
                fixture.Result = _reportLogic.GetFixtureReport(fixture.Id);
            }
            return fixtures;
        }


        public List<TeamMember> GetAvailableForFixture(int fixtureId)
        {
            List<PlayerFixtureAvailability> availabiltiies = _avilabilityRepository.FindBy(x => x.EventId == fixtureId);
            List<TeamMember> available = availabiltiies.Select(availability => _userLogic.GetUser(availability.UserId)).ToList();
            return available;
        } 

        public List<Fixture> GetUserFixtures(int id)
        {
            List<Team> teams = _teamLogic.GetTeamsByUserId(id);
            List<Fixture> fixtures = new List<Fixture>();
            foreach (var team in teams)
            {
                fixtures.AddRange(GetTeamFixtures(team.Id));
            }

            foreach (var fixture in fixtures)
            {
                fixture.HomeTeam = _teamLogic.GetTeamById(fixture.HomeId);
                fixture.AwayTeam = _teamLogic.GetTeamById(fixture.AwayId);
                fixture.Available = GetAvailableForFixture(fixture.Id);
                fixture.Result = _reportLogic.GetFixtureReport(fixture.Id);
            }
            foreach (var e in fixtures)
            {
                PlayerFixtureAvailability pfv = _avilabilityRepository.FindBy(x => x.EventId == e.Id).FirstOrDefault();
                if (pfv != null)
                    e.AttendanceState = pfv.Availability;
            }
            return fixtures;
        } 

        public EntityResponse CreateFixture(FixtureCreateReturnViewModel fixtureIn)
        {
            try
            {
                Fixture fixture = new Fixture()
                {
                    HomeId = fixtureIn.HomeId,
                    AwayId = fixtureIn.AwayId,
                    Start = fixtureIn.Start,
                    End  = fixtureIn.End,
                    Comments = fixtureIn.Comments,
                    Positions = fixtureIn.Positions,
                    State = TrakkEnums.FixtureState.New,
                    Location = fixtureIn.Location
                };
                _fixtureRepository.Add(fixture);
                _fixtureRepository.Save();
                return new EntityResponse(true, "Fixture : " + fixture.HomeId + " v " + fixture.AwayTeam.Name + " created successfully.");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Fixture : creation failed: " + e.Message);
            }

        }

       

        public EntityResponse UpdateFixture(Fixture fixture)
        {
            try
            {
                if (fixture.End > DateTime.Now)
                    fixture.State = TrakkEnums.FixtureState.New;
                else
                {
                    fixture.State = TrakkEnums.FixtureState.Finished;
                }
                //fixture.Result = _reportLogic.GetReport()   TODO
                _fixtureRepository.Update(fixture);
                fixture.HomeTeam = _teamLogic.GetTeamById(fixture.HomeId);
                fixture.AwayTeam = _teamLogic.GetTeamById(fixture.AwayId);
                return new EntityResponse(true, "Fixture : " + fixture.HomeTeam.Name + " v " + fixture.AwayTeam.Name + " updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Fixture : " + fixture.HomeTeam.Name + " v " + fixture.AwayTeam.Name + " update failed" + e.Message);
            }
        }

        public EntityResponse DeleteFixture(int id)
        {
            _fixtureRepository.Remove(_fixtureRepository.FindBy(x => x.Id == id).FirstOrDefault());
            return new EntityResponse(true, "Fixture deleted successfully");
        }

        public PlayerFixtureAvailability GetFixtureAvailability(int fixtureId, int userId)
        {
            return _avilabilityRepository.FindBy(x => x.EventId == fixtureId && x.UserId == userId).FirstOrDefault();
        }


    }
}