using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpFolder
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public int Name { get; set; }

		[JsonPropertyName("lists")]
		public IList<ClickUpList> Lists { get; set; }
	}
}
