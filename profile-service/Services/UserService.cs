using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;

namespace profile_service.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserCache _userCache;
        private readonly IUserRepository _userRepo;

        public UserService(ILogger<UserService> logger, IUserCache cache, IUserRepository userDataAccess)
        {
            _logger = logger;
            _userCache = cache;
            _userRepo = userDataAccess;
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> users = null;

                // Get from cache
                users = await _userCache.GetAllUsers();
                if (users != null)
                {
                    return users;
                }

                // Get from database
                users = await _userRepo.GetAllUsers();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllUsers: " + ex.Message);
                return null;
            }
        }

        public async Task<User> GetUser(string uid)
        {
            try
            {
                User user = null;

                // Get from cache
                user = await _userCache.GetUser(uid);
                if (user != null)
                {
                    return user;
                }

                // Get from database
                user = await _userRepo.GetUser(uid);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error for user " + uid + ": " + ex.Message);
                return null;
            }
        }

        public async Task<Events> UpdateUser(User updatedUser)
        {
            try
            {
                if (string.IsNullOrEmpty(updatedUser.name) || string.IsNullOrEmpty(updatedUser.password))
                {
                    return Events.ERROR;
                }
                updatedUser.password = EncodePassword(updatedUser.password);
                return await _userRepo.UpdateUser(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during update: " + ex.Message);
                return Events.UPDATED;
            }
        }

        public async Task<List<User>> SearchUser(string name)
        {
            return await _userRepo.SearchUser(name);
        }

        public async Task<List<string>> GetFriends(string uid)
        {
            try
            {
                List<string> friends = null;

                // Get from cache
                friends = await _userCache.GetFriends(uid);
                if (friends != null)
                {
                    return friends;
                }

                // Get from databae
                friends = await _userRepo.GetFriends(uid);
                return friends;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetFriends: " + ex.Message);
                return null;
            }
        }

        public async Task<Events> AddFriend(string uid, string newFriendId)
        {
            try
            {
                return await _userRepo.AddFriend(uid, newFriendId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding friend: " + ex.Message);
                return Events.ERROR;
            }
        }

        public async Task<User> Login(string email, string password)
        {
            try
            {
                password = EncodePassword(password);
                return await _userRepo.Login(email, password);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in login: " + ex.Message);
                return null;
            }
        }

        public async Task<Events> Signup(User newUser)
        {
            try
            {
                newUser.password = EncodePassword(newUser.password);
                return await _userRepo.Signup(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in signup: " + ex.Message);
                return Events.ERROR;
            }
        }

        private string EncodePassword(string password)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(bytes);
        }
    }
}