using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;

namespace profile_service.Services
{
	public class UserService : IUserService
	{
		private readonly IMongoCollection<User> _mongo;
		private readonly ILogger<UserService> _logger;

		public UserService(IProfileDatabaseSettings settings, ILogger<UserService> logger)
		{
			_logger = logger;
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			_mongo = database.GetCollection<User>(settings.UsersCollectionName);
		}

		public async Task<UserEvents> AddFriend(string UId, string NewFriend)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(User => User.UId == UId);
				User user = await userQuery.FirstOrDefaultAsync();
				user.Friends.Add(NewFriend);
				await _mongo.ReplaceOneAsync(user => user.UId == UId, user);
				return UserEvents.ADDED;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in adding friend: " + ex.Message);
				return UserEvents.ERROR;
			}
		}

		public async Task<List<User>> GetAllUsers()
		{
			try
			{
				var query = await _mongo.FindAsync<User>(user => true);
				List<User> users = query.ToList();
				return users;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in GetAllUsers: " + ex.Message);
				return null;
			}
		}

		public async Task<List<User>> GetFriends(string UId)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.UId == UId);
				User user = await userQuery.FirstOrDefaultAsync();
				var friends = user.Friends;
				
				var filterDef = new FilterDefinitionBuilder<User>();
				var filter = filterDef.In(user => user.UId, friends);
				var friendsQuery = await _mongo.FindAsync(filter);
				return friendsQuery.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in GetFriends: " + ex.Message);
				return null;
			}
		}

		public async Task<User> GetUser(string UId)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.UId == UId);
				return await userQuery.FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error for user " + UId + ": " + ex.Message);
				return null;
			}
		}

		public async Task<User> Login(string email, string password)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.Email == email && user.Password == password);
				return await userQuery.FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in login: " + ex.Message);
				return null;
			}
		}

		public async Task<UserEvents> Signup(User newUser)
		{
			try
			{
				if(!await UserExists(newUser))
				{
					await _mongo.InsertOneAsync(newUser);
					return UserEvents.CREATED;
				}
				else
				{
					return UserEvents.EXISTS;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Error in signup: " + ex.Message);
				return UserEvents.ERROR;
			}
		}

		private async Task<bool> UserExists(User newUser)
		{
			var userQuery = await _mongo.FindAsync(user => user.Email == newUser.Email);
			var user = await userQuery.FirstOrDefaultAsync();
			if(user == null)
			{
				return false;
			}

			return true;
		}
	}
}