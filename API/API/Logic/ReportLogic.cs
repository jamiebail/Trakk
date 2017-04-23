using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;
using API.ViewModels;

namespace API.Logic
{
    public class ReportLogic : IReportLogic
    {
        readonly IRepository<GameReport> _reportRepository = new Repository<GameReport>();
        readonly IRepository<TeamMember> _userRepository = new Repository<TeamMember>();
        readonly IRepository<Card> _cardRepository = new Repository<Card>();
        readonly IRepository<Goal> _goalRepository = new Repository<Goal>();

        readonly ITeamLogic _teamLogic = new TeamLogic();
        readonly IRepository<Fixture> _fixtureRepository = new Repository<Fixture>();

        public List<GameReport> GetAllReports()
        {
            return _reportRepository.GetAll();
        }

        public GameReport GetReport(int reportId)
        {

                GameReport report = _reportRepository.FindBy(x => x.Id == reportId).FirstOrDefault();
                if (report != null)
                {
                    report.Goals = _goalRepository.FindBy(x => x.ReportId == reportId);
                    foreach (var goal in report.Goals)
                    {
                        goal.Scorer = _userRepository.FindBy(x => x.Id == goal.ScorerId).FirstOrDefault();
                    }
                    report.Cards = _cardRepository.FindBy(y => y.ReportId == reportId);
                    foreach (var card in report.Cards)
                    {
                        card.Player = _userRepository.FindBy(x => x.Id == card.PlayerId).FirstOrDefault();
                    }
                    return report;
                }
            return null;
        }

        public GameReport GetFixtureReport(int fixtureId)
        {
           GameReport report = _reportRepository.FindBy(x => x.FixtureId == fixtureId).FirstOrDefault();
            if(report != null)
                return GetReport(report.Id);
            return null;
        }

        public EntityResponse CreateReport(GameReport report)
        {
            try
            {
                GameReport entity = _reportRepository.FindBy(x => x.FixtureId == report.FixtureId).FirstOrDefault();
                if (entity != null)
                {
                    report.Id = entity.Id;
                    _reportRepository.Update(report);
                }
                else
                {
                    _reportRepository.Add(report);
                }
                _reportRepository.Save();

                List<int> allCards = _cardRepository.FindBy(x => x.ReportId == report.Id).Select(x => x.Id).ToList();
                foreach (var card in report.Cards)
                {
                    Card current =
                        _cardRepository.FindBy(x => x.PlayerId == card.PlayerId && x.ReportId == report.Id)
                            .FirstOrDefault();
                    if (current != null)
                    {
                        current.CardColour = card.CardColour;
                        _cardRepository.Update(current);
                        allCards.Remove(current.Id);
                    }
                    else
                    {
                        card.ReportId = report.Id;
                        _cardRepository.Add(card);
                    }

                }
                //foreach (int c in allCards)
                //{
                //    _cardRepository.Remove(_cardRepository.FindBy(x => x.Id == c).FirstOrDefault());
                //}
                _cardRepository.Save();

                List<int> allGoals = _goalRepository.FindBy(x => x.ReportId == report.Id).Select(x => x.Id).ToList();
                foreach (var goal in report.Goals)
                {

                        goal.ReportId = report.Id;
       
                        _goalRepository.Add(goal);

                }
                //foreach (int g in allGoals)
                //{
                //    _goalRepository.Remove(_goalRepository.FindBy(x => x.Id == g).FirstOrDefault());
                //}
                _goalRepository.Save();

                Fixture fixture = _fixtureRepository.FindBy(x => x.Id == report.FixtureId).FirstOrDefault();
                List<StatUpdateViewModel> statupdates = DetermineWinner(report);
                if (entity == null)
                {
                    statupdates[0].Id = fixture.HomeId;
                    statupdates[1].Id = fixture.AwayId;
                    _teamLogic.UpdateTeamStatistics(statupdates[0]);
                    _teamLogic.UpdateTeamStatistics(statupdates[1]);
                }

                return new EntityResponse(true, "Report created successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Report creation failed" + e.Message);
            }
        }


        public List<StatUpdateViewModel> DetermineWinner(GameReport report)
        {
            StatUpdateViewModel homeUpdate = new StatUpdateViewModel();
            StatUpdateViewModel awayUpdate = new StatUpdateViewModel();
            if (report.HomeScore > report.AwayScore)
            {
                homeUpdate.Result = TrakkEnums.Result.Win;
                awayUpdate.Result = TrakkEnums.Result.Loss;
            }
            else if(report.HomeScore < report.AwayScore)
            {
                awayUpdate.Result = TrakkEnums.Result.Win;
                homeUpdate.Result = TrakkEnums.Result.Loss;
            }
            else
            {
                awayUpdate.Result = TrakkEnums.Result.Draw;
                homeUpdate.Result = TrakkEnums.Result.Draw;
            }
            homeUpdate.Conceded = report.AwayScore;
            awayUpdate.Conceded = report.HomeScore;
            awayUpdate.Goals = report.AwayScore;
            homeUpdate.Goals = report.HomeScore;
            homeUpdate.Id = report.FixtureId;
            awayUpdate.Id = report.FixtureId;
            return new List<StatUpdateViewModel>(){ homeUpdate, awayUpdate};
        }



        public EntityResponse UpdateReport(GameReport report)
        {
            try
            {
                _reportRepository.Update(report);
                _reportRepository.Save();
                foreach (var card in report.Cards)
                {
                    card.ReportId = report.Id;
                    card.Side = TrakkEnums.Side.Home;
                    _cardRepository.Update(card);
                }
                _cardRepository.Save();
                foreach (var goal in report.Goals)
                {
                    goal.ReportId = report.Id;
                    goal.Side = TrakkEnums.Side.Home;
                    _goalRepository.Update(goal);
                }
                _goalRepository.Save();
                return new EntityResponse(true, "Report updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "Report update failed" + e.Message);
            }
        }
    }
}