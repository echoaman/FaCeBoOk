using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using profile_service.Interfaces;
using profile_service.Models;

namespace profile_service.Services
{
	public class UserService : IUserService
	{
		private readonly IMongoCollection<User> _userService;
		private readonly ILogger<UserService> _logger;

		public UserService(IProfileDatabaseSettings settings, ILogger<UserService> logger)
		{
			_logger = logger;
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			_userService = database.GetCollection<User>(settings.UsersCollectionName);
		}
		public List<User> GetUsers()
		{
			List<User> users = new List<User>();
			try
			{
				var result = _userService.Find(user => true).ToList();
				users.AddRange(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return users;
		}
	}
}