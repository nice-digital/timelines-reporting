using NICE.Timelines.Common;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using NICE.Timelines.Models.Database;

namespace NICE.Timelines.Models
{
	public class Message
	{
		[JsonPropertyName("id")]
		public string WebhookMessageId { get; set; }

		[JsonPropertyName("date")]
		public DateTime WebHookCallDateTime { get; set; }

		public Payload Payload { get; set; }


		/// <summary>
		/// This converts clickup's model to our TimelinesTask type, which is a lot closer to how we'll store it in the DB.
		///
		/// TODO: This function needs revisiting in the beta, to handle nulls gracefully and logging.
		/// TODO: Also the fields are currently picked by the field name. they should probably be changed id (a Guid)
		/// TODO: and move this function somewhere else.
		/// </summary>
		/// <returns></returns>
		public TimelineTask ToTimelinesTask()
		{
			var acid = int.Parse(Payload.CustomFields.First(field => field.Name.Equals(Constants.ClickUp.FieldNames.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>()); //TODO: ACID is a string in clickup. needs to be a number.

			var dateTypeId = 0;
			string dateTypeDescription = null;
			var dateTypeField = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.DateType, StringComparison.InvariantCultureIgnoreCase));
			if (dateTypeField != null)
			{
				var index = dateTypeField.Value.ToObject<int>();
				dateTypeId = int.Parse(dateTypeField.TypeConfig.Options[index].Name);

				dateTypeDescription = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.DateTypeDescription, StringComparison.InvariantCultureIgnoreCase)).TypeConfig.Options[index].Name;
			}

			DateTime? actualDate = null;
			var actualDateStringJsonElement = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value;

			if (actualDateStringJsonElement.HasValue)
			{
				var actualDateString = actualDateStringJsonElement.Value.ToStringObject();
				if (!string.IsNullOrEmpty(actualDateString))
				{
					actualDate = (double.Parse(actualDateString)).ToDateTime();
				}
			}

			var dueDate = string.IsNullOrEmpty(Payload.DueDateSecondsSinceUnixEpochAsString)? null : double.Parse(Payload.DueDateSecondsSinceUnixEpochAsString).ToDateTime();

			return new TimelineTask(acid, Payload.ClickUpTaskId, dateTypeId, dateTypeDescription, dueDate, actualDate);
		}
	}
}
