using System.Collections.Generic;
using System.Text.Json.Serialization;
using NICE.Timelines.Common.Models;

namespace NICE.Timelines.ConsoleApp.Models.ClickUp
{
	public class ClickUpTasks
	{
		[JsonPropertyName("tasks")]
		public IEnumerable<ClickUpTask> Tasks { get; set; }

	}
}
