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
		private readonly IUserDataAccess _userDataAccess;

		public UserService(ILogger<UserService> logger, IUserCache cache, IUserDataAccess userDataAccess)
		{
			_logger = logger;
			_userCache = cache;
			_userDataAccess = userDataAccess;
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
				users = await _userDataAccess.GetAllUsers();
				return users;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in GetAllUsers: " + ex.Message);
				return null;
			}
		}

		public async Task<User> GetUser(string UId)
		{
			try
			{
				User user = null;

				// Get from cache
				user = await _userCache.GetUser(UId);
				if (user != null)
				{
					return user;
				}

				// Get from database
				user = await _userDataAccess.GetUser(UId);
				return user;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error for user " + UId + ": " + ex.Message);
				return null;
			}
		}

		public async Task<Events> UpdateUser(User updateDetails)
		{
			try
			{
				return await _userDataAccess.UpdateUser(updateDetails);
			}
			catch (Exception ex)
			{
				_logger.LogError("Error during update: " + ex.Message);
				return Events.UPDATED;
			}
		}

		public async Task<List<User>> SearchUser(string name)
		{
			return await _userDataAccess.SearchUser(name);
		}

		public async Task<List<User>> GetFriends(string UId)
		{
			try
			{
				List<User> friends = null;

				// Get from cache
				friends = await _userCache.GetFriends(UId);
				if (friends != null)
				{
					return friends;
				}

				// Get from databae
				friends = await _userDataAccess.GetFriends(UId);
				return friends;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in GetFriends: " + ex.Message);
				return null;
			}
		}

		public async Task<Events> AddFriend(string UId, string NewFriendId)
		{
			try
			{
				return await _userDataAccess.AddFriend(UId, NewFriendId);
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
				return await _userDataAccess.Login(email, password);
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
				return await _userDataAccess.Signup(newUser);
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in signup: " + ex.Message);
				return Events.ERROR;
			}
		}
	}
}