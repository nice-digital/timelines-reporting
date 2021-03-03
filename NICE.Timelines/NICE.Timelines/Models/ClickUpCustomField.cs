using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
	public class ClickUpCustomField
	{
		[JsonPropertyName("id")]
		public Guid FieldId { get; set; }

		public string Name { get; set; }

		public JsonElement Value { get; set; }

		[JsonPropertyName("type_config")]
		public ClickUpTypeConfig TypeConfig { get; set; }
	}
}