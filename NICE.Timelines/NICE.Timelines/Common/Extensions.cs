using System;
using System.Text.Json;

namespace NICE.Timelines.Common
{
	public static class Extensions
	{
		public static T ToObject<T>(this JsonElement element)
		{
			var json = element.GetRawText();
			return JsonSerializer.Deserialize<T>(json);
		}
		public static T ToObject<T>(this JsonDocument document)
		{
			var json = document.RootElement.GetRawText();
			return JsonSerializer.Deserialize<T>(json);
		}

		public static DateTime? ToDateTime(this double millisecondsSinceUnixEpoch)
		{
			var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dtDateTime = dtDateTime.AddMilliseconds(millisecondsSinceUnixEpoch).ToLocalTime();
			return dtDateTime;
		}
		public static DateTime? ToDateTime(this double? secondsSinceUnixEpoch)
		{
			if (!secondsSinceUnixEpoch.HasValue)
				return null;

			return ToDateTime(secondsSinceUnixEpoch.Value);
		}

	}
}
