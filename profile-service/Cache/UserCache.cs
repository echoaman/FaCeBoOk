using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;

namespace profile_service.Cache
{
	public class UserCache : IUserCache
	{
		private readonly IDistributedCache _cache;
		private const string all_users = "all_users";
		private const string single_user = "user_";
		private readonly ILogger<UserCache> _logger;
		public UserCache(IDistributedCache cache, ILogger<UserCache> logger)
		{
			_cache = cache;
			_logger = logger;
		}

		public async Task<bool> SetAllUsers(List<User> users)
		{
			try
			{
				string json = JsonSerializer.Serialize(users);
				await _cache.SetStringAsync(all_users, json);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("SetAllUsers in cache: " + ex.Message);
				throw;
			}
		}

		public async Task<List<User>> GetAllUsers()
		{
			try
			{
				string json = await _cache.GetStringAsync(all_users);
				if (string.IsNullOrEmpty(json))
				{
					return null;
				}
				List<User> users = JsonSerializer.Deserialize<List<User>>(json);
				return users;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetAllUsers in cache: " + ex.Message);
				return null;
			}
		}

		public async Task<bool> SetUser(User user)
		{
			try
			{
				string key = single_user + user.UId;
				string json = JsonSerializer.Serialize(user);
				await _cache.SetStringAsync(key, json);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetAllUsers in cache: " + ex.Message);
				return false;
			}
		}

		public async Task<User> GetUser(string UId)
		{
			try
			{
				string key = single_user + UId;
				string json = await _cache.GetStringAsync(key);
				if (string.IsNullOrEmpty(json))
				{
					return null;
				}

				User user = JsonSerializer.Deserialize<User>(json);
				return user;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetUser in cache: " + ex.Message);
				throw;
			}
		}

		public async Task<List<User>> GetFriends(string UId)
		{
			try
			{
				User user = await GetUser(UId);
				if (user == null || user.Friends.Count == 0)
				{
					return null;
				}

				List<User> friends = new List<User>();
				foreach (string friendUId in user.Friends)
				{
					User friend = await GetUser(friendUId);
					if (friend == null)
					{
						return null;
					}

					friends.Add(friend);
				}

				return friends.Count == user.Friends.Count ? friends : null;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetFriends in cache: " + ex.Message);
				throw;
			}
		}
	}
}