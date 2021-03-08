using System;
using System.Linq;
using System.Threading.Tasks;

namespace NICE.TimelinesSync.Services
{
	public interface ISyncService
	{
		public Task Process();
	}

	public class SyncService : ISyncService
	{
		private readonly IClickUpService _clickUpService;

		public SyncService(IClickUpService clickUpService)
		{
			_clickUpService = clickUpService;
		}

		public async Task Process()
		{
			Console.WriteLine("Started processing");

			var tasks = await _clickUpService.GetTasks();

			Console.WriteLine($"task count:{tasks.Count()}");

			Console.WriteLine("Ended processing");
		}

	}
}
