using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NICE.TimelinesCommon.Models
{
	public class ClickUpFolders
	{
		[JsonPropertyName("folders")]
		public IList<ClickUpFolder> Folders { get; set; }
	}
}
