using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Services;
using System.IO;
using System.Threading.Tasks;
using NICE.Timelines.Models;

namespace NICE.Timelines.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ClickUpController : ControllerBase
	{
		
		private readonly ILogger<ClickUpController> _logger;
		private readonly IDataAccessService _dataAccessService;

		public ClickUpController(ILogger<ClickUpController> logger, IDataAccessService dataAccessService)
		{
			_logger = logger;
			_dataAccessService = dataAccessService;
		}

		/// <summary>
		/// this endpoint is hit by the clickup webhook call.
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Consumes("application/json")]
		public async Task<ActionResult> Post([FromBody] ClickUpMessage clickUpMessage) 
		{
			// todo: authenticate the request header "X-Signature"

			string taskInJSON;
			using (var stream = new StreamReader(HttpContext.Request.Body))
			{
				taskInJSON = await stream.ReadToEndAsync();
			}
			_logger.LogInformation(taskInJSON);

			//var clickupTask = _parsingService.GetTask(taskInJSON);
			
			_dataAccessService.SaveTask(clickUpMessage.ToTimelinesTask());

			return Ok();
		}


		/// <summary>
		/// This endpoint is just for local debug purposes.
		/// </summary>
		/// <param name="test"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Get(string test)
		{
			_logger.LogInformation(test);
			return Ok();
		}
	}
}
