using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace profile_service.Controllers
{
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly ILogger<ProfileController> _logger;

		public ProfileController(ILogger<ProfileController> logger)
		{
			_logger = logger;
		}
	}
}