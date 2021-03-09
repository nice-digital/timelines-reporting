using System.Collections.Generic;
using System.Text.Json.Serialization;
using NICE.TimelinesCommon.Models;

namespace NICE.TimelinesSync.Models.ClickUp
{
	public class ClickUpTasks
	{
		[JsonPropertyName("tasks")]
		public IEnumerable<ClickUpTask> Tasks { get; set; }

	}
}
