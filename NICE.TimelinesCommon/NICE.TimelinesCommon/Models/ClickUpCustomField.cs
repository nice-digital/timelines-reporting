using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpCustomField
	{
		[JsonPropertyName("id")]
		public Guid FieldId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("value")]
		public JsonElement Value { get; set; }

		[JsonPropertyName("type_config")]
		public ClickUpTypeConfig ClickUpTypeConfig { get; set; }
	}
}