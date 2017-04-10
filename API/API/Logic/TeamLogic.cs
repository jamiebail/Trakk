using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Helpers;
using API.Models;
using API.Repositories;
using API.ViewModels;
using Microsoft.Ajax.Utilities;

namespace API.Logic
{
    public class TeamLogic : ITeamLogic
    {
        readonly IRepository<Team> _teamRepository = new Repository<Team>();
        readonly IRepository<TeamStatistics> _statisticsRepository = new Repository<TeamStatistics>();
        readonly IRepository<TeamMembership> _membershipRepository = new Repository<TeamMembership>();
        readonly IRepository<Position> _positionRepository = new Repository<Position>();

        readonly IUserLogic _userLogic = new UserLogic();

        public List<Team> GetAllTeams()
        {
            return _teamRepository.GetAll();
        }

        public Team GetTeamById(int id)
        {
             Team team = _teamRepository.FindBy(x => x.Id == id).FirstOrDefault();
             //if(team != null)
             //   team.Statistics = GetTeamStats(team.Id);
            return team;
        }

        //public TeamStatistics GetTeamStats(int id)
        //{
        //     return _statisticsRepository.FindBy(x => x.TeamId == id).FirstOrDefault();
        //}

        public List<TeamMember> GetTeamMembersByTeamId(int id)
        {
            List<TeamMembership> teamMemberships = _membershipRepository.FindBy(x => x.TeamId == id);
            return teamMemberships.Select(membership => _userLogic.GetUser(membership.MemberId)).ToList();
        }

        public List<Team> GetTeamsByUserId(int id)
        {
            List<TeamMembership> teamMemberships = _membershipRepository.FindBy(x => x.MemberId == id);
            return teamMemberships.Select(membership => GetTeamById(membership.TeamId)).ToList();
        }

        public EntityResponse UpdateTeam(Team team)
        {
            try
            {
                _teamRepository.Update(team);
                _teamRepository.Save();
                return new EntityResponse(true, team.Name + " updated successfully");
            }
            catch(Exception e)
            {
                return new EntityResponse(false, team.Name + " updating failed: " + e.Message);
            }
        }

        public EntityResponse CreateTeam(Team team)
        {
            try
            {
                _teamRepository.Add(team);
                _teamRepository.Save();
                return new EntityResponse(true, team.Name + " created successfully.");
            }
            catch(Exception e)
            {
                return new EntityResponse(false, team.Name + " creation failed: " + e.Message);
            }

        }

        public EntityResponse UpdateTeamStatistics(StatUpdateViewModel update)
        {
            TeamStatistics teamStatistics = _statisticsRepository.FindBy(x => x.Id == update.Id).FirstOrDefault();
            if(teamStatistics != null) { 
                switch (update.Result)
                {
                    case (int)TrakkEnums.Result.Win:
                        teamStatistics.Won++;
                        teamStatistics.Points += (int) TrakkEnums.Points.Win;
                        break;
                    case (int)TrakkEnums.Result.Loss:
                        teamStatistics.Lost++;
                        teamStatistics.Points += (int)TrakkEnums.Points.Loss;
                        break;
                    case (int)TrakkEnums.Result.Draw:
                        teamStatistics.Drawn++;
                        teamStatistics.Points += (int)TrakkEnums.Points.Draw;
                        break;
                }

                teamStatistics.Conceded += update.Conceded;
                teamStatistics.Goals += update.Goals;
                teamStatistics.Played++;

            teamStatistics.Cards.ToList().AddRange(update.Cards);
            try
                {
                    // Save statistics back to database
                    _statisticsRepository.Update(teamStatistics);
                    _statisticsRepository.Save();
                    return new EntityResponse(true, "Update of team statistics successful.");
                }
                catch(Exception e)
                {
                    return new EntityResponse(false, "Updating of team statistics failed: " + e.Message);
                }
            }
            return new EntityResponse(false, "No statistics found for provided team id." );
        }


        public EntityResponse ArchiveTeam(Team Team)
        {
            try
            {   
                // TODO ADD ARCHIVING
                     
                //_userRepository.(user);
                return new EntityResponse(true, Team.Name + " deletion successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, Team.Name + " deletion failed: " + e.Message);
            }
        }



    }
}