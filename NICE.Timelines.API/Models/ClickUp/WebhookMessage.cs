using System;
using System.Text.Json.Serialization;
using NICE.Timelines.Common.Models;

namespace NICE.Timelines.API.Models.ClickUp
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
