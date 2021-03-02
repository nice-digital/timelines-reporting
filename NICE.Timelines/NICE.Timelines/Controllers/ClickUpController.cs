using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
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
		public async Task<ActionResult> Post()
		{
			using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
			{
				
				var body = await stream.ReadToEndAsync();
			}
			return Ok();
		}

		[HttpGet]
		public ActionResult Get(string body)
		{
			
			return Ok();
		}
	}
}
