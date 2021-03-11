using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpFolder
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("lists")]
		public IList<ClickUpList> Lists { get; set; }
	}
}
