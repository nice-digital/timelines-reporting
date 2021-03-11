using NICE.TimelinesCommon;
using NICE.TimelinesCommon.Models;
using NICE.TimelinesDB.Models;
using System;
using System.Linq;

namespace NICE.TimelinesDB
{
	public static class Converters
	{
		public static int GetACIDFromClickUpTask(ClickUpTask clickUpTask)
		{
			return int.Parse(clickUpTask.CustomFields.First(field => field.FieldId.Equals(Constants.ClickUp.Fields.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>()); //TODO: ACID is a string in clickup. needs to be a number.
		}

		/// <summary>
		/// TODO: this needs changing for the beta!! it needs to gracefully handle errors / missing data, with logging
		/// TODO: also it should probably live somewhere else too
		/// </summary>
		/// <param name="clickUpTask"></param>
		/// <returns></returns>
		public static TimelineTask ConvertToTimelineTask(ClickUpTask clickUpTask)
		{
			var acid = GetACIDFromClickUpTask(clickUpTask);

			var stepId = 0;
			var stepDescription = "Not found";
			var stepField = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.StepId, StringComparison.InvariantCultureIgnoreCase));
			if (stepField != null && stepField.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
			{
				var index = stepField.Value.ToObject<int>();
				stepId = int.Parse(stepField.ClickUpTypeConfig.Options[index].Name);

				stepDescription = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.StepDescription, StringComparison.InvariantCultureIgnoreCase)).ClickUpTypeConfig.Options[index].Name;
			}

			var stageId = 0;
			var stageDescription = "Not found";
			var stageField = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.StageId, StringComparison.InvariantCultureIgnoreCase));
			if (stageField != null && stageField.Value.ValueKind != System.Text.Json.JsonValueKind.Undefined)
			{
				var index = stageField.Value.ToObject<int>();
				stageId = int.Parse(stageField.ClickUpTypeConfig.Options[index].Name);

				stageDescription = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.StageDescription, StringComparison.InvariantCultureIgnoreCase)).ClickUpTypeConfig.Options[index].Name;
			}

			DateTime? actualDate = null;
			var actualDateStringJsonElement = clickUpTask.CustomFields.FirstOrDefault(field => field.FieldId.Equals(Constants.ClickUp.Fields.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

			if (actualDateStringJsonElement.HasValue)
			{
				var actualDateString = actualDateStringJsonElement.Value.ToStringObject();
				if (!string.IsNullOrEmpty(actualDateString))
				{
					actualDate = (double.Parse(actualDateString)).ToDateTime();
				}
			}

			var dueDate = string.IsNullOrEmpty(clickUpTask.DueDateSecondsSinceUnixEpochAsString) ? null : double.Parse(clickUpTask.DueDateSecondsSinceUnixEpochAsString).ToDateTime();

			return new TimelineTask(acid, stepId, stepDescription, stageId, stageDescription, clickUpTask.Space.Id, clickUpTask.Folder.Id, clickUpTask.ListId.Id, clickUpTask.ClickUpTaskId, dueDate, actualDate);
		}
	}
}
