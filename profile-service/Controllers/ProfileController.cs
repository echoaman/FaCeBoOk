using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using profile_service.Interfaces;
using profile_service.Models;
using System.Threading.Tasks;
using System.Net.Http;

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
		[Route("/login/{email}/{password}")]
		public async Task<ActionResult> Login(string email, string password)
		{
			User user = await _userService.Login(email, password);
			
			if(user == null)
			{
				return StatusCode(404, user);
			}

			return StatusCode(200, user);
		}

		[HttpPost]
		[Route("/signup")]
		public async Task<ActionResult> Signup(User NewUser)
		{
			UserEvents userEvents = await _userService.Signup(NewUser);
			if(userEvents == UserEvents.CREATED)
			{
				return StatusCode(201);
			}
			else if(userEvents == UserEvents.EXISTS)
			{
				return StatusCode(204);
			}
			else 
			{
				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("/users/{UId}")]
		public async Task<ActionResult> GetUser(string UId)
		{
			User user = await _userService.GetUser(UId);
			if(user == null)
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
			if(friends == null)
			{
				return StatusCode(500, friends);
			}

			return StatusCode(200, friends);
		}

		[HttpPut]
		[Route("/friends/{UId}/{NewFriend}")]
		public async Task<ActionResult> AddFriend(string UId, string NewFriend)
		{
			UserEvents userEvents = await _userService.AddFriend(UId, NewFriend);
			if(userEvents == UserEvents.ADDED)
			{
				return StatusCode(204);
			}

			return StatusCode(500);
		}

		[HttpGet]
		[Route("/users")]
		public async Task<ActionResult> GetAllUsers()
		{
			List<User> users = await _userService.GetAllUsers();
			if(users == null)
			{
				return StatusCode(500, users);
			}

			return StatusCode(200, users);
		}
	}
}