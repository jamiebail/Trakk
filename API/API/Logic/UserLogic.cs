using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Helpers;
using API.Models;
using API.Repositories;
using Newtonsoft.Json;

namespace API.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly IRepository<TeamMember> _userRepository = new Repository<TeamMember>();
        private readonly IRepository<Team> _teamRepository = new Repository<Team>(); 
        private readonly IRepository<TeamMembership> _membershipRepository = new Repository<TeamMembership>();
        private readonly IRepository<TeamRoles> _rolesRepository = new Repository<TeamRoles>();
        private readonly IRepository<PlayerEventAvailability> _eventAvailabilityRepository = new Repository<PlayerEventAvailability>();
        private readonly IRepository<PlayerFixtureAvailability> _fixtureAvailabilityRepository = new Repository<PlayerFixtureAvailability>();

        public List<TeamMember> GetUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public TeamMember GetUser(int id)
        {
          return  _userRepository.FindBy(x => x.Id == id).FirstOrDefault();
        }

        public EntityResponse SetUserTeam(int userId, int teamId)
        {
            TeamMembership membership = new TeamMembership(){ MemberId = userId,TeamId = teamId, Accepted = false};
            try
            {
                TeamMembership existingMembership =
                    _membershipRepository.FindBy(x => x.TeamId == membership.TeamId && x.MemberId == membership.MemberId)
                        .FirstOrDefault();
                if(existingMembership == null)
                     _membershipRepository.Add(membership);
                return new EntityResponse(true, "membership created successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "membership create failed: " + e.Message);
            }
        }

        public EntityResponse AcceptTeamInvite(TeamMembership membership)
        {
            try
            {
                TeamMembership existingMembership =
                    _membershipRepository.FindBy(x => x.TeamId == membership.TeamId && x.MemberId == membership.MemberId)
                        .FirstOrDefault();
                existingMembership.Accepted = true;
                _membershipRepository.Update(membership);
                _membershipRepository.Save();
                return new EntityResponse(false, "team membership updating success: ");
            }
            catch (Exception e)
            {
                return new EntityResponse(false,"team membership updating failed: " + e.Message);
            }
        }
        public EntityResponse RejectTeamInvite(TeamMembership membership)
        {
            try
            {
                TeamMembership existingMembership =
                    _membershipRepository.FindBy(x => x.TeamId == membership.TeamId && x.MemberId == membership.MemberId)
                        .FirstOrDefault();
                _membershipRepository.Remove(existingMembership);
                _membershipRepository.Save();
                return new EntityResponse(false, "team membership updating success: ");
            }
            catch (Exception e)
            {
                return new EntityResponse(false,"team membership updating failed: " + e.Message);
            }
        }

        public TeamMembership GetUserMembership(int userId, int teamId)
        {
            return _membershipRepository.FindBy(x => x.MemberId == userId && x.TeamId == teamId).FirstOrDefault();
        }

        public EntityResponse SetUserRole(TeamRoles role)
        {
            try
            {
                _rolesRepository.Add(role);
                _rolesRepository.Save();
                return new EntityResponse(true, role.UserId + " role set successfully");
            }
            catch(Exception e)
            {
                return new EntityResponse(false, role.UserId + " updating failed: " + e.Message);
            }

        }


        public EntityResponse UpdateUserRole(TeamRoles role)
        {
            try
            {
                _rolesRepository.Update(role);
                _rolesRepository.Save();
                return new EntityResponse(true, role.UserId + " role set successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, role.UserId + " updating failed: " + e.Message);
            }

        }

        public EntityResponse UpdateUser(TeamMember user)
        {
            try
            {
                _userRepository.Update(user);
                return new EntityResponse(true, user.Name + " updated successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, user.Name + " updating failed: " + e.Message);
            }
        }

        public EntityResponse CreateUser(TeamMember user)
        {
            try
            {
                _userRepository.Add(user);
                return new EntityResponse(true, user.Name + " created successfully", user.Id);
            }
            catch (Exception e)
            {
                return new EntityResponse(false, user.Name + " create failed: " + e.Message);
            }
        }


        public EntityResponse DeleteUser(TeamMember user)
        {
            try
            {
                _userRepository.Remove(user);
                return new EntityResponse(true, user.Name + " deletion successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, user.Name + " deletion failed: " + e.Message);
            }
        }

        public EntityResponse UpdateAvailability(PlayerEventAvailability availability)
        {
            try
            {
                PlayerEventAvailability currentAvailability =
                    _eventAvailabilityRepository.FindBy(
                        x => x.EventId == availability.EventId && x.UserId == availability.UserId)
                        .FirstOrDefault();
                EntityResponse response;
                if (currentAvailability != null)
                {
                    currentAvailability.Availability = availability.Availability;
                    _eventAvailabilityRepository.Update(currentAvailability);
                    response = new EntityResponse(true, "Availability update success");
                }
                else
                {
                    _eventAvailabilityRepository.Add(availability);
                    response = new EntityResponse(true, "Availability add success");
                }
                _eventAvailabilityRepository.Save();
                return response;
            }
            catch (Exception e)
            {
                return new EntityResponse(false,"Availability updated failed: " + e.Message);
            }
        }

        public EntityResponse UpdateFixtureAvailability(PlayerFixtureAvailability availability)
        {
            try
            {
                PlayerFixtureAvailability currentAvailability =
                    _fixtureAvailabilityRepository.FindBy(
                        x => x.EventId == availability.EventId && x.UserId == availability.UserId)
                        .FirstOrDefault();
                EntityResponse response;
                if (currentAvailability != null)
                {
                    currentAvailability.Availability = availability.Availability;
                    currentAvailability.TeamId = availability.TeamId;
                    _fixtureAvailabilityRepository.Update(currentAvailability);
                    response = new EntityResponse(true, " availability update success");
                }
                else
                {
                    _fixtureAvailabilityRepository.Add(availability);
                    response = new EntityResponse(true, " availability add success");
                }
                _fixtureAvailabilityRepository.Save();
                return response;
            }
            catch (Exception e)
            {
                return new EntityResponse(false, " deletion failed: " + e.Message);
            }
        }

    }
}