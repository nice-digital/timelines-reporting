using System;
using System.Threading.Tasks;
using NICE.Timelines.ConsoleApp.Configuration;

namespace NICE.Timelines.ConsoleApp.Services
{
	public interface ISyncService
	{
		public Task Process();
	}

	public class SyncService : ISyncService
	{
		private readonly ClickUpConfig _clickUpConfig;
		private readonly IClickUpService _clickUpService;

		public SyncService(ClickUpConfig clickUpConfig, IClickUpService clickUpService)
		{
			_clickUpConfig = clickUpConfig;
			_clickUpService = clickUpService;
		}

		public async Task Process()
		{
			Console.WriteLine("Started processing");

			foreach (var spaceId in _clickUpConfig.SpaceIds)
			{
				Console.WriteLine($"Started with space: {spaceId}");

				var recordsSaveOrUpdated = await _clickUpService.ProcessSpace(spaceId);

				Console.WriteLine($"finished with space: {spaceId} records saved or updated: {recordsSaveOrUpdated}");
			}

			Console.WriteLine("Ended processing");
		}

	}
}
