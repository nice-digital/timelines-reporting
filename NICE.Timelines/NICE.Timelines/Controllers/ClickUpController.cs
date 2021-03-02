using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NICE.Timelines.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ClickUpController : ControllerBase
	{
		
		private readonly ILogger<ClickUpController> _logger;

		public ClickUpController(ILogger<ClickUpController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public ActionResult Post()
		{
			return Ok();
		}
	}
}
