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

        public async Task<User> AddFriend(string uid, string newFriendId)
        {
            try
            {
                if(uid == newFriendId || !await UserExists(uid) || !await UserExists(uid))
                {
                    return null;
                }

                var filter = Builders<User>.Filter.Eq(user => user.uid, uid);
                var update = Builders<User>.Update.Push<String>(user => user.friends, newFriendId);
                User user = await _userCollection.FindOneAndUpdateAsync(user => user.uid == uid, update);
                user.friends.Add(newFriendId);
                return user;
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
                List<User> users = null;
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var query = await _userCollection.FindAsync<User>(user => true, _filter);
                users = await query.ToListAsync();
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
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'friends' : 0, 'uid' : 0, 'name' : 0, 'name' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.uid == uid, _filter);
                User user = await userQuery.FirstOrDefaultAsync();
                
                if(user != null) {
                    return user.friends;
                }

                return null;
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
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.uid == uid, _filter);
                User user = await userQuery.FirstOrDefaultAsync();
                return user;
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
                FindOptions<User> _filter = new FindOptions<User>();
                _filter.Projection = "{'password' : 0}";
                var userQuery = await _userCollection.FindAsync(user => user.email == email && user.password == password, _filter);
                User user = await userQuery.FirstOrDefaultAsync();
                return user;
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
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<bool> Signup(User newUser)
        {
            try
            {
                if(! await UserExists(newUser))
                {
                    newUser.friends = new List<string>();
                    await _userCollection.InsertOneAsync(newUser);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<User> UpdateUser(User updatedUser)
        {
            try
            {
                if(await UserExists(updatedUser.uid))
                {
                    var filter = Builders<User>.Filter.Eq("uid", updatedUser.uid);
                    var update = Builders<User>.Update
                        .Set("name", updatedUser.name)
                        .Set("password", updatedUser.password);

                    UpdateResult updateResult = await _userCollection.UpdateOneAsync(filter, update);
                    return await GetUser(updatedUser.uid);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
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