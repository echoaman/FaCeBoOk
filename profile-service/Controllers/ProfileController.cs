using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;

namespace profile_service.Controllers
{
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly IUserService _userService;

		public ProfileController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		[Route("/login")]
		public async Task<ActionResult> Login([FromQuery] string email, [FromQuery] string password)
		{
			User user = await _userService.Login(email, password);

			if (user == null)
			{
				return StatusCode(404, user);
			}

			return StatusCode(200, user);
		}

		[HttpPost]
		[Route("/signup")]
		public async Task<ActionResult> Signup(User NewUser)
		{
			Events userEvents = await _userService.Signup(NewUser);
			if (userEvents == Events.CREATED)
			{
				return StatusCode(201);
			}
			
			return StatusCode(400);
		}

		[HttpGet]
		[Route("/users/{UId}")]
		public async Task<ActionResult> GetUser(string UId)
		{
			User user = await _userService.GetUser(UId);
			if (user == null)
			{
				return StatusCode(404, user);
			}

			return StatusCode(200, user);
		}


		[HttpGet]
		[Route("/friends/{UId}")]
		public async Task<ActionResult> GetFriends(string UId)
		{
			List<User> friends = await _userService.GetFriends(UId);
			if (friends == null || friends.Count == 0)
			{
				return StatusCode(404, null);
			}

			return StatusCode(200, friends);
		}

		[HttpPut]
		[Route("/addfriend")]
		public async Task<ActionResult> AddFriend([FromQuery] string UId, [FromQuery] string NewFriendId)
		{
			Events userEvents = await _userService.AddFriend(UId, NewFriendId);
			if (userEvents == Events.ADDED)
			{
				return StatusCode(204);
			}

			return StatusCode(400);
		}

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
		[Route("/updateUser")]
		public async Task<ActionResult> UpdateUser(User updateDetails)
		{
			Events userEvents = await _userService.UpdateUser(updateDetails);
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
			if(users == null || users.Count == 0)
			{
				return StatusCode(404, null);
			}
			return StatusCode(200, users);
		}
	}
}