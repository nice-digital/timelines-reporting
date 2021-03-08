﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
	public class Payload
	{
		[JsonPropertyName("id")]
		public string ClickUpTaskId { get; set; }

		public string Name { get; set; }

		[JsonPropertyName("due_date")]
		public string DueDateSecondsSinceUnixEpochAsString { get; set; }

		[JsonPropertyName("custom_fields")]
		public IEnumerable<CustomField> CustomFields { get; set; }
	}
}