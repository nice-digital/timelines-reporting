using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpTask
	{
		[JsonPropertyName("id")]
		public string ClickUpTaskId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("due_date")]
		public string DueDateSecondsSinceUnixEpochAsString { get; set; }

		[JsonPropertyName("custom_fields")]
		public IEnumerable<ClickUpCustomField> CustomFields { get; set; }
	}
}