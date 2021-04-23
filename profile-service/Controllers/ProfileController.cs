using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using profile_service.Interfaces;
using profile_service.Models;

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

		[Route("/users")]
		public List<User> GetAllUsers()
		{
			return _userService.GetUsers();
		}
	}
}