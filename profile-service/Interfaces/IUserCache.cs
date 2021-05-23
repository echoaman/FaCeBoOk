using System.Collections.Generic;
using System.Threading.Tasks;
using profile_service.Entities;

namespace profile_service.Interfaces
{
    public interface IUserCache
    {
        public Task<bool> SetUser(User user);
        public Task<User> GetUser(string uid);
        public Task<List<string>> GetFriends(string uid);
    }
}