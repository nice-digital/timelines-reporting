using NICE.TimelinesCommon.Models;
using System;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
	public class WebhookMessage
	{
		[JsonPropertyName("id")]
		public string WebhookMessageId { get; set; }

		[JsonPropertyName("date")]
		public DateTime WebHookCallDateTime { get; set; }


		[JsonPropertyName("payload")]
		public ClickUpTask ClickUpTask { get; set; }
	}
}
