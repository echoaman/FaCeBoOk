using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;
using StackExchange.Redis;

namespace profile_service.Cache
{
	public class UserCache : IUserCache
	{
		private readonly IDatabase _cache;
		private const string all_users = "all_users";
		private readonly ILogger<UserCache> _logger;
		public UserCache(IDatabase cache, ILogger<UserCache> logger)
		{
			_cache = cache;
			_logger = logger;
		}

		public async Task<bool> SetAllUsers(List<User> users, TimeSpan expiration)
		{
			try
			{
				string json = JsonSerializer.Serialize(users);
				await _cache.StringSetAsync(all_users, json, expiration);
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
				RedisValue json = await _cache.StringGetAsync(all_users);
				if (json.IsNullOrEmpty)
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

		public async Task<bool> SetUser(User user, TimeSpan expiration)
		{
			try
			{
				string json = JsonSerializer.Serialize(user);
				return await _cache.StringSetAsync(user.UId, json, expiration);
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
				RedisValue json = await _cache.StringGetAsync(UId);
				if (json.IsNullOrEmpty)
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