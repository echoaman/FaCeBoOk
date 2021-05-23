using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;
using profile_service.Entities;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace profile_service.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserCache _userCache;
        private readonly IUserRepository _userRepo;

        private readonly IJwtSettings _jwtSettings;

        public UserService(ILogger<UserService> logger, IUserCache cache, IUserRepository userDataAccess, IJwtSettings jwtSettings)
        {
            _logger = logger;
            _userCache = cache;
            _userRepo = userDataAccess;
            _jwtSettings = jwtSettings;
        }

        public async Task<Events> AddFriend(string uid, string newFriendId)
        {
            try
            {
                // update db
                User user = await _userRepo.AddFriend(uid, newFriendId);
                if (user == null)
                {
                    return Events.INVALID;
                }

                //cache updated user
                bool cached = await _userCache.SetUser(user);
                if (cached)
                {
                    return Events.ADDED;
                }
                return Events.INVALID;
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
                List<User> users = await _userRepo.GetAllUsers();
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
                List<string> friends = null;

                //check cache
                friends = await _userCache.GetFriends(uid);
                if (friends != null && friends.Count > 0)
                {
                    return friends;
                }

                //get from db
                User user = await GetUser(uid);
                return user.friends;
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
                User user = null;

                //check cache
                user = await _userCache.GetUser(uid);
                if (user != null)
                {
                    return user;
                }

                //get from db
                user = await _userRepo.GetUser(uid);

                //cache user
                bool cached = await _userCache.SetUser(user);
                if (cached)
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<string> Login(string email, string password)
        {
            try
            {
                string encodedPassword = hashPassword(password);
                User user = await _userRepo.Login(email, encodedPassword);
                if(user == null) {
                    return null;
                }

                return GetJwtToken(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw new InvalidOperationException("UserService - Error in login");
            }
        }

        public async Task<List<User>> SearchUser(string name)
        {
            try
            {
                return await _userRepo.SearchUser(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<string> Signup(SignupRequest signupRequest)
        {
            try
            {
                string hashedPassword = hashPassword(signupRequest.password);

                //save to db
                User user = await _userRepo.Signup(signupRequest.name, signupRequest.email, hashedPassword);
                if(user == null) {
                    return null;
                }

                return GetJwtToken(user);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }

        public async Task<Events> UpdateUser(User updatedUser)
        {
            try
            {
                if (string.IsNullOrEmpty(updatedUser.name) || string.IsNullOrEmpty(updatedUser.password))
                {
                    return Events.INVALID;
                }

                updatedUser.password = hashPassword(updatedUser.password);

                //update in db
                User user = await _userRepo.UpdateUser(updatedUser);

                //cache user
                bool cached = await _userCache.SetUser(updatedUser);
                if (cached)
                {
                    return Events.UPDATED;
                }

                return Events.INVALID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Data);
                throw;
            }
        }
        private string hashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GetJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.name),
                    new Claim(ClaimTypes.NameIdentifier, user.uid)
                }),
                
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}