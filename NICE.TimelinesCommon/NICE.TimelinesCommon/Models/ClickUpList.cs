using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpList
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public int Name { get; set; }

		[JsonPropertyName("content")]
		public int Content { get; set; }
	}
}