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
        private readonly IRepository<TeamFixtureSetup> _setupRepository = new Repository<TeamFixtureSetup>(); 
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
                var homeSetup = _setupRepository.FindBy(x => x.FixtureId == fixture.Id && x.TeamId == fixture.HomeId).FirstOrDefault();
                var awaySetup = _setupRepository.FindBy(x => x.FixtureId == fixture.Id && x.TeamId == fixture.AwayId).FirstOrDefault();
                if (homeSetup != null)
                    fixture.TeamSetups.Add(homeSetup);
                if (awaySetup != null)
                    fixture.TeamSetups.Add(awaySetup);

                fixture.HomeTeam = _teamRepository.FindBy(x => x.Id == fixture.HomeId).FirstOrDefault();
                fixture.AwayTeam = _teamRepository.FindBy(x => x.Id == fixture.AwayId).FirstOrDefault();
                fixture.Available = GetAvailableForFixture(fixture.Id);
                fixture.Result = _reportLogic.GetFixtureReport(fixture.Id);
                return fixture;
            }
            else return null;
            
        }

        public TeamFixtureSetup GetTeamSetup(int fixtureId, int teamId)
        {
            return _setupRepository.FindBy(x => x.FixtureId == fixtureId && x.TeamId == teamId).FirstOrDefault();
        }

        public List<Fixture> GetTeamFixtures(int id, DateTime? month)
        {
            List<Fixture> fixtures = new List<Fixture>();
            if (month != null)
               fixtures = _fixtureRepository.FindBy(x => (x.HomeId == id || x.AwayId == id) && x.Start.Month == month.Value.Month && x.Start.Year == month.Value.Year);
            else
                fixtures = _fixtureRepository.FindBy(x => x.HomeId == id || x.AwayId == id);

            foreach (var fixture in fixtures)
            {
                fixture.TeamSetups = new List<TeamFixtureSetup>();
                var homeSetup =
                    _setupRepository.FindBy(x => x.FixtureId == fixture.Id && x.TeamId == fixture.HomeId).FirstOrDefault();
                var awaySetup =
                    _setupRepository.FindBy(x => x.FixtureId == fixture.Id && x.TeamId == fixture.AwayId).FirstOrDefault();
                if(homeSetup != null)
                    fixture.TeamSetups.Add(homeSetup);
                if(awaySetup != null)
                    fixture.TeamSetups.Add(awaySetup);

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

        public List<Fixture> GetUserFixtures(int id, DateTime? month)
        {
            List<Team> teams = _teamLogic.GetTeamsByUserId(id);
            List<Fixture> fixtures = new List<Fixture>();
            foreach (var team in teams)
            {
                if(month != null)
                    fixtures.AddRange(GetTeamFixtures(team.Id, month.Value));
                else
                    fixtures.AddRange(GetTeamFixtures(team.Id, null));
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
                    State = TrakkEnums.FixtureState.New,
                    Location = fixtureIn.Location
                };
                int teamId = 0;
                if (fixtureIn.Side == TrakkEnums.Side.Home)
                    teamId = fixtureIn.HomeId;
                else
                    teamId = fixtureIn.AwayId;
                _fixtureRepository.Add(fixture);
                _fixtureRepository.Save();
                TeamFixtureSetup setup = new TeamFixtureSetup()
                {
                    FixtureId = fixture.Id,
                    Comments = fixtureIn.Comments,
                    Positions = fixtureIn.Positions,
                    TeamId = teamId
                };
                _setupRepository.Add(setup);
                _setupRepository.Save();
                return new EntityResponse(true, "Fixture : " + fixture.HomeId + " v " + fixture.AwayTeam.Name + " created successfully.");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Fixture : creation failed: " + e.Message);
            }

        }

       

        public EntityResponse UpdateFixture(FixtureCreateReturnViewModel fixtureIn)
        {
            try
            {
                Fixture fixture = _fixtureRepository.FindBy( x => x.Id == fixtureIn.Id).FirstOrDefault();
                if (fixture.End > DateTime.Now)
                    fixture.State = TrakkEnums.FixtureState.New;
                else
                {
                    fixture.State = TrakkEnums.FixtureState.Finished;
                }
                fixture.Start = fixtureIn.Start;
                fixture.End = fixtureIn.End;
                fixture.Location = fixtureIn.Location;
                int teamId;
                if (fixtureIn.Side == TrakkEnums.Side.Home)
                    teamId = fixtureIn.HomeId;
                else
                    teamId = fixtureIn.AwayId;
                TeamFixtureSetup setup = _setupRepository.FindBy(x => x.FixtureId == fixture.Id && x.TeamId == teamId).FirstOrDefault();
                if (setup == null)
                {
                    TeamFixtureSetup newSetup = new TeamFixtureSetup()
                    {
                        Comments = fixtureIn.Comments,
                        FixtureId = fixture.Id,
                        Positions = fixtureIn.Positions,
                        TeamId = teamId
                    };
                    _setupRepository.Add(newSetup);
                }
                else
                {
                    setup.Comments = fixtureIn.Comments;
                    setup.Positions = fixtureIn.Positions;
                    _setupRepository.Update(setup);
                }
                _setupRepository.Save();
                _fixtureRepository.Update(fixture);
                return new EntityResponse(true, "Fixture updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Fixture update failed" + e.Message);
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