using System.Collections.Generic;

namespace NICE.TimelinesSync.Configuration
{
	public class ClickUpConfig
	{
		public string AccessToken { get; set; }
		public IEnumerable<string> SpaceIds { get; set; }
	}
}
