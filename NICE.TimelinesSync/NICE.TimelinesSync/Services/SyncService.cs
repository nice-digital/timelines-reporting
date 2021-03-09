using System;
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

			await _clickUpService.SaveAndUpdateTasks();

			Console.WriteLine("Ended processing");
		}

	}
}
