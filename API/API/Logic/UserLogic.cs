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
                _membershipRepository.Add(membership);
                return new EntityResponse(true, "membership created successfully");
            }
            catch (Exception e)
            {
                return new EntityResponse(false, "membership create failed: " + e.Message);
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

    }
}