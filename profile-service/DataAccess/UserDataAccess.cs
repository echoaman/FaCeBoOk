using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using profile_service.Interfaces;
using profile_service.Models;

namespace profile_service.DataAccess
{
	public class UserDataAccess : IUserDataAccess
	{
		private readonly IUserCache _userCache;
		private readonly IMongoCollection<User> _mongo;
		private readonly ILogger<UserDataAccess> _logger;
		public UserDataAccess(IDatabaseSettings settings, IUserCache userCache, ILogger<UserDataAccess> logger)
		{
			_userCache = userCache;
			_logger = logger;

			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			_mongo = database.GetCollection<User>(settings.UsersCollectionName);
		}

		public async Task<List<User>> GetAllUsers()
		{
			try
			{
				List<User> users = null;
				var query = await _mongo.FindAsync<User>(user => true);
				users = query.ToList();

				// Save in cache
				bool setSuccesful = await _userCache.SetAllUsers(users);
				if (setSuccesful)
				{
					return users;
				}
				else return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetAllUsers in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<User> GetUser(string UId)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.UId == UId);
				User user = await userQuery.FirstOrDefaultAsync();
				if (user == null)
				{
					return null;
				}

				// Save to cache
				bool setSuccesful = await _userCache.SetUser(user);
				if (setSuccesful)
				{
					return user;
				}

				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetUser in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<Events> UpdateUser(User updateDetails)
		{
			try
			{
				var filter = Builders<User>.Filter.Eq("UId", updateDetails.UId);
				var update = Builders<User>.Update
					.Set("Uname", updateDetails.Uname)
					.Set("Password", updateDetails.Password);

				UpdateResult updateResult = await _mongo.UpdateOneAsync(filter, update);
				if (updateResult.ModifiedCount == 0)
				{
					return Events.ERROR;
				}

				// Save to cache
				List<User> users = await GetAllUsers();
				User updatedUser = users.Find(user => user.UId == updateDetails.UId);
				bool setSuccesful = await _userCache.SetUser(updatedUser);
				if (setSuccesful)
				{
					return Events.UPDATED;
				}

				return Events.ERROR;
			}
			catch (Exception ex)
			{
				_logger.LogError("UpdateUser in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<List<User>> SearchUser(string name)
		{
			try
			{
				BsonRegularExpression regex = new BsonRegularExpression(new Regex(name, RegexOptions.None));
				var filter = Builders<User>.Filter.Regex("Uname", regex);
				var query = await _mongo.FindAsync<User>(filter);
				List<User> users = query.ToList();
				return users;
			}
			catch (Exception ex)
			{
				_logger.LogError("SearchUser in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<List<User>> GetFriends(string UId)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.UId == UId);
				User user = await userQuery.FirstOrDefaultAsync();
				if (user == null)
				{
					return null;
				}

				var filterDef = new FilterDefinitionBuilder<User>();
				var filter = filterDef.In(user => user.UId, user.Friends);
				var friendsQuery = await _mongo.FindAsync(filter);
				List<User> friends = friendsQuery.ToList();

				// Save to cache
				foreach (User _user in friends)
				{
					await _userCache.SetUser(_user);
				}

				return friends;
			}
			catch (Exception ex)
			{
				_logger.LogError("GetFriends in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<Events> AddFriend(string UId, string NewFriendId)
		{
			try
			{
				if (!await UserExists(UId) || !await UserExists(UId))
				{
					return Events.INVALID;
				}
				var filter = Builders<User>.Filter.Eq(user => user.UId, UId);
				var update = Builders<User>.Update.Push<String>(user => user.Friends, NewFriendId);
				User user = await _mongo.FindOneAndUpdateAsync(user => user.UId == UId, update);
				user.Friends.Add(NewFriendId);

				// Save in cache
				await GetAllUsers();
				bool setSuccesful = await _userCache.SetUser(user);

				if (setSuccesful)
				{
					return Events.ADDED;
				}
				else return Events.ERROR;
			}
			catch (Exception ex)
			{
				_logger.LogError("AddFriend in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<User> Login(string email, string password)
		{
			try
			{
				var userQuery = await _mongo.FindAsync(user => user.Email == email && user.Password == password);
				User user = await userQuery.FirstOrDefaultAsync();
				if (user == null)
				{
					return null;
				}

				// Save to cache
				bool setSuccesful = await _userCache.SetUser(user);
				if (setSuccesful)
				{
					return user;
				}

				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Login in UserDataAccess: " + ex.Message);
				throw;
			}
		}

		public async Task<Events> Signup(User newUser)
		{
			try
			{
				if (!await UserExists(newUser))
				{
					await _mongo.InsertOneAsync(newUser);

					// Save to cache
					await GetAllUsers();
					bool setSuccesful = await _userCache.SetUser(newUser);

					if (setSuccesful)
					{
						return Events.CREATED;
					}

					return Events.ERROR;
				}
				else
				{
					return Events.EXISTS;
				}
			}
			catch (System.Exception)
			{

				throw;
			}
		}

		private async Task<bool> UserExists(User newUser)
		{
			var userQuery = await _mongo.FindAsync(user => user.Email == newUser.Email);
			var user = await userQuery.FirstOrDefaultAsync();
			if (user == null)
			{
				return false;
			}

			return true;
		}

		private async Task<bool> UserExists(string id)
		{
			var userQuery = await _mongo.FindAsync(user => user.UId == id);
			var user = await userQuery.FirstOrDefaultAsync();
			if (user == null)
			{
				return false;
			}

			return true;
		}
	}
}