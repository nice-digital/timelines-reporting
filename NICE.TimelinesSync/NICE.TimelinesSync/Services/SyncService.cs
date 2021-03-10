using System;
using System.Threading.Tasks;
using NICE.TimelinesSync.Configuration;

namespace NICE.TimelinesSync.Services
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
				Console.WriteLine($"Started with space:{spaceId}");
				await _clickUpService.ProcessSpace(spaceId);
			}

			Console.WriteLine("Ended processing");
		}

	}
}
