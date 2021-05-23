using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;
using profile_service.Entities;
using Microsoft.AspNetCore.Authorization;

namespace profile_service.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            string token = await _userService.Login(loginRequest.email, loginRequest.password);

            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(404, new { message = "Invalid Email or Password" });
            }

            return StatusCode(200, token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/signup")]
        public async Task<ActionResult> Signup(SignupRequest signupRequest)
        {
            string token = await _userService.Signup(signupRequest);
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(400, new { message = "User exists" });
            }

            return StatusCode(201, token);
        }

        [HttpGet]
        [Route("/users/{uid}")]
        public async Task<ActionResult> GetUser(string uid)
        {
            User user = await _userService.GetUser(uid);
            if (user == null)
            {
                return StatusCode(404, user);
            }

            return StatusCode(200, user);
        }


        [HttpGet]
        [Route("/friends/{uid}")]
        public async Task<ActionResult> GetFriends(string uid)
        {
            List<string> friends = await _userService.GetFriends(uid);
            if (friends == null || friends.Count == 0)
            {
                return StatusCode(404, null);
            }

            return StatusCode(200, friends);
        }

        [HttpPut]
        [Route("/friends")]
        public async Task<ActionResult> AddFriend(AddFriendRequest addFriendRequest)
        {
            Events userEvents = await _userService.AddFriend(addFriendRequest.uid, addFriendRequest.newFriendId);
            if (userEvents == Events.ADDED)
            {
                return StatusCode(204);
            }

            return StatusCode(400);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/users")]
        public async Task<ActionResult> GetAllUsers()
        {
            List<User> users = await _userService.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return StatusCode(404, null);
            }

            return StatusCode(200, users);
        }

        [HttpPut]
        [Route("/users")]
        public async Task<ActionResult> UpdateUser(User updatedUser)
        {
            Events userEvents = await _userService.UpdateUser(updatedUser);
            if (userEvents == Events.UPDATED)
            {
                return StatusCode(204);
            }

            return StatusCode(400);
        }

        [HttpGet]
        [Route("/search")]
        public async Task<ActionResult> SearchUser([FromQuery] string name)
        {
            List<User> users = await _userService.SearchUser(name);
            if (users == null || users.Count == 0)
            {
                return StatusCode(404, null);
            }
            return StatusCode(200, users);
    }
    }
}