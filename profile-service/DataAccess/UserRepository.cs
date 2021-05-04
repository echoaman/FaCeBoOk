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
    public class UserRepository : IUserRepository
    {
        private readonly IUserCache _userCache;
        private readonly IMongoCollection<User> _userCollection;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IDatabaseSettings settings, IUserCache userCache, ILogger<UserRepository> logger)
        {
            _userCache = userCache;
            _logger = logger;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }

        //Updates entire cache
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> users = null;
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var query = await _userCollection.FindAsync<User>(user => true, _filter);
                users = await query.ToListAsync();

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

        public async Task<User> GetUser(string uid)
        {
            try
            {
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.uid == uid, _filter);
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

        public async Task<Events> UpdateUser(User updatedUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq("uid", updatedUser.uid);
                var update = Builders<User>.Update
                    .Set("name", updatedUser.name)
                    .Set("password", updatedUser.password);

                UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
                if (updateResult.ModifiedCount == 0)
                {
                    return Events.ERROR;
                }

                // Save to cache
                List<User> users = await GetAllUsers();
                User currUser = users.Find(user => user.uid == updatedUser.uid);
                bool setSuccesful = await _userCache.SetUser(currUser);
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
                var seachfilter = Builders<User>.Filter.Regex("name", regex);
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";

                var query = await _userCollection.FindAsync<User>(seachfilter, _filter);
                List<User> users = await query.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError("SearchUser in UserDataAccess: " + ex.Message);
                throw;
            }
        }

        public async Task<List<User>> GetFriends(string uid)
        {
            try
            {
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.uid == uid, _filter);
                User user = await userQuery.FirstOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }

                var filterDef = new FilterDefinitionBuilder<User>();
                var filter = filterDef.In(user => user.uid, user.friends);
                var friendsQuery = await _userCollection.FindAsync(filter, _filter);
                List<User> friends = await friendsQuery.ToListAsync();

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

        public async Task<Events> AddFriend(string uid, string newFriendId)
        {
            try
            {
                if (uid == newFriendId || !await UserExists(uid) || !await UserExists(newFriendId))
                {
                    return Events.INVALID;
                }
                var filter = Builders<User>.Filter.Eq(user => user.uid, uid);
                var update = Builders<User>.Update.Push<String>(user => user.friends, newFriendId);
                User user = await _userCollection.FindOneAndUpdateAsync(user => user.uid == uid, update);
                user.friends.Add(newFriendId);

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
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.email == email && user.password == password, _filter);
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
                    newUser.friends = new List<string>();
                    await _userCollection.InsertOneAsync(newUser);

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
            var userQuery = await _userCollection.FindAsync(user => user.email == newUser.email);
            var user = await userQuery.FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> UserExists(string uid)
        {
            var userQuery = await _userCollection.FindAsync(user => user.uid == uid);
            var user = await userQuery.FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}