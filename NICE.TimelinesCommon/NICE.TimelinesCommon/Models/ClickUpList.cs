using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpList
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("content")]
		public string Content { get; set; }
	}
}