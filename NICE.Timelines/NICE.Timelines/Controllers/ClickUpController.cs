using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NICE.TimelinesAPI.Models.ClickUp;
using NICE.TimelinesDB.Services;

namespace NICE.TimelinesAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ClickUpController : ControllerBase
	{
		
		private readonly ILogger<ClickUpController> _logger;
		private readonly IDatabaseService _databaseService;

		public ClickUpController(ILogger<ClickUpController> logger, IDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		/// <summary>
		/// this endpoint is hit by the clickup webhook call when a task's Due date or actual date are changed, and that task is listed as a "Key date"
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost("SaveOrUpdate")]
		[Consumes("application/json")]
		public async Task<ActionResult> SaveOrUpdate([FromBody] WebhookMessage clickUpMessage) 
		{
			// todo: authenticate the request header "X-Signature"

			//logging the received json for debug purposes only
			//string taskInJSON;
			//using (var stream = new StreamReader(HttpContext.Request.Body))
			//{
			//	taskInJSON = await stream.ReadToEndAsync();
			//}
			//_logger.LogInformation(taskInJSON);
			
			await _databaseService.SaveOrUpdateTimelineTask(clickUpMessage.ClickUpTask);

			return Ok();
		}

		/// <summary>
		/// This endpoint is hit by the clickup webhook call when a task's "key date" field is set to false. In which case the matching entry should be removed from our DB.
		/// </summary>
		/// <param name="clickUpMessage"></param>
		/// <returns></returns>
		[HttpPost("Delete")]
		[Consumes("application/json")]
		public async Task<ActionResult> Delete([FromBody] WebhookMessage clickUpMessage)
		{
			// todo: authenticate the request header "X-Signature" (even more important here)

			//logging the received json for debug purposes only
			string taskInJSON;
			using (var stream = new StreamReader(HttpContext.Request.Body))
			{
				taskInJSON = await stream.ReadToEndAsync();
			}
			_logger.LogInformation(taskInJSON);


			await _databaseService.DeleteTimelineTask(clickUpMessage.ClickUpTask);

			return Ok();
		}
	}
}
