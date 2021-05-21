using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;
using profile_service.Entities;
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

        public async Task<Events> AddFriend(string uid, string newFriendId)
        {
            try
            {
                // update db
                User user = await _userRepo.AddFriend(uid, newFriendId);
                if(user == null)
                {
                    return Events.INVALID;
                }

                //cache updated user
                bool cached = await _userCache.SetUser(user);
                if(cached)
                {
                    return Events.ADDED;
                }
                return Events.INVALID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }


        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> users = await _userRepo.GetAllUsers();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<List<string>> GetFriends(string uid)
        {
            try
            {
                List<string> friends = null;

                //check cache
                friends = await _userCache.GetFriends(uid);
                if(friends != null && friends.Count > 0)
                {
                    return friends;
                }

                //get from db
                User user = await GetUser(uid);
                return user.friends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<User> GetUser(string uid)
        {
            try
            {
                User user = null;

                //check cache
                user = await _userCache.GetUser(uid);
                if(user != null)
                {
                    return user;
                }

                //get from db
                user = await _userRepo.GetUser(uid);

                //cache user
                bool cached = await _userCache.SetUser(user);
                if(cached)
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
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
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<List<User>> SearchUser(string name)
        {
            try
            {
                return await _userRepo.SearchUser(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<Events> Signup(User newUser)
        {
            try
            {
                newUser.password = EncodePassword(newUser.password);
                
                //save to db
                bool added = await _userRepo.Signup(newUser);
                if(added)
                {
                    return Events.CREATED;
                }

                return Events.INVALID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<Events> UpdateUser(User updatedUser)
        {
            try
            {
                if (string.IsNullOrEmpty(updatedUser.name) || string.IsNullOrEmpty(updatedUser.password))
                {
                    return Events.INVALID;
                }

                updatedUser.password = EncodePassword(updatedUser.password);

                //update in db
                User user = await _userRepo.UpdateUser(updatedUser);

                //cache user
                bool cached = await _userCache.SetUser(updatedUser);
                if(cached)
                {
                    return Events.UPDATED;
                }

                return Events.INVALID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }
        private string EncodePassword(string password)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            return System.Convert.ToBase64String(bytes);
        }
    }
}