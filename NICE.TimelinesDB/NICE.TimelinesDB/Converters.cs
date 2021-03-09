using NICE.TimelinesCommon;
using NICE.TimelinesCommon.Models;
using NICE.TimelinesDB.Models;
using System;
using System.Linq;

namespace NICE.TimelinesDB
{
	public static class Converters
	{
		/// <summary>
		/// TODO: this needs changing for the beta!! it needs to gracefully handle errors / missing data, with logging
		/// TODO: also it should probably live somewhere else.
		/// </summary>
		/// <param name="clickUpTask"></param>
		/// <returns></returns>
		public static TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask)
		{
			var acid = int.Parse(clickUpTask.CustomFields.First(field => field.Name.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>()); //TODO: ACID is a string in clickup. needs to be a number.

			var dateTypeId = 0;
			string dateTypeDescription = "Not found";
			var dateTypeField = clickUpTask.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.Fields.DateType, StringComparison.InvariantCultureIgnoreCase));
			if (dateTypeField != null && dateTypeField.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
			{
				var index = dateTypeField.Value.ToObject<int>();
				dateTypeId = int.Parse(dateTypeField.ClickUpTypeConfig.Options[index].Name);

				dateTypeDescription = clickUpTask.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.Fields.DateTypeDescription, StringComparison.InvariantCultureIgnoreCase)).ClickUpTypeConfig.Options[index].Name;
			}

			DateTime? actualDate = null;
			var actualDateStringJsonElement = clickUpTask.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.Fields.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

			if (actualDateStringJsonElement.HasValue)
			{
				var actualDateString = actualDateStringJsonElement.Value.ToStringObject();
				if (!string.IsNullOrEmpty(actualDateString))
				{
					actualDate = (double.Parse(actualDateString)).ToDateTime();
				}
			}

			var dueDate = string.IsNullOrEmpty(clickUpTask.DueDateSecondsSinceUnixEpochAsString) ? null : double.Parse(clickUpTask.DueDateSecondsSinceUnixEpochAsString).ToDateTime();

			return new TimelineTask(acid, clickUpTask.ClickUpTaskId, dateTypeId, dateTypeDescription, dueDate, actualDate);
		}
	}
}
