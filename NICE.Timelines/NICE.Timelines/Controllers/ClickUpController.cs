using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Models;
using NICE.Timelines.Services;
using System.Threading.Tasks;

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
		/// this endpoint is hit by the clickup webhook call when a task's Due date or actual date are changed, and that task is listed as a "Key date"
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost("SaveOrUpdate")]
		[Consumes("application/json")]
		public async Task<ActionResult> SaveOrUpdate([FromBody] Message clickUpMessage) 
		{
			// todo: authenticate the request header "X-Signature"

			//logging the received json for debug purposes only
			//string taskInJSON;
			//using (var stream = new StreamReader(HttpContext.Request.Body))
			//{
			//	taskInJSON = await stream.ReadToEndAsync();
			//}
			//_logger.LogInformation(taskInJSON);


			await _dataAccessService.SaveOrUpdateTask(clickUpMessage);

			return Ok();
		}

		/// <summary>
		/// This endpoint is hit by the clickup webhook call when a task's "key date" field is set to false. In which case the matching entry should be removed from our DB.
		/// </summary>
		/// <param name="clickUpMessage"></param>
		/// <returns></returns>
		[HttpPost("Delete")]
		[Consumes("application/json")]
		public async Task<ActionResult> Delete([FromBody] Message clickUpMessage)
		{
			// todo: authenticate the request header "X-Signature" (even more important here)

			//logging the received json for debug purposes only
			string taskInJSON;
			using (var stream = new StreamReader(HttpContext.Request.Body))
			{
				taskInJSON = await stream.ReadToEndAsync();
			}
			_logger.LogInformation(taskInJSON);


			await _dataAccessService.DeleteTask(clickUpMessage);

			return Ok();
		}
	}
}
