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
        private readonly ILogger<UserCache> _logger;
        public UserCache(IDistributedCache cache, ILogger<UserCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }


        public async Task<bool> SetUser(User user)
        {
            try
            {
                string key = "uid:" + user.uid;
                string json = JsonSerializer.Serialize(user);
                await _cache.SetStringAsync(key, json, getExpiration(10));
                return true;
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
                string key = "uid:" + uid;
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
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<List<string>> GetFriends(string uid)
        {
            try
            {
                User user = await GetUser(uid);
                if(user == null)
                {
                    return null;
                }

                return user.friends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }
        private DistributedCacheEntryOptions getExpiration(double minutes)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes)
            };
        }
    }
}