using NICE.Timelines.Common;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace NICE.Timelines.Models
{
	public class ClickUpMessage
	{
		public Guid Id { get; set; }

		[JsonPropertyName("date")]
		public DateTime WebHookCallDateTime { get; set; }

		public ClickUpPayload Payload { get; set; }


		/// <summary>
		/// This converts clickup's model to our TimelinesTask type, which is a lot closer to how we'll store it in the DB.
		///
		/// TODO: This function needs revisiting in the beta, to handle nulls gracefully and logging.
		/// TODO: Also the fields are currently picked by the field name. they should probably be changed id (a Guid)
		/// </summary>
		/// <returns></returns>
		public TimelinesTask ToTimelinesTask()
		{
			var acid = int.Parse(Payload.CustomFields.First(field => field.Name.Equals(Constants.ClickUp.FieldNames.ACID, StringComparison.InvariantCultureIgnoreCase)).Value.ToObject<string>()); //TODO: ACID is a string in clickup. needs to be a number.

			int? dateTypeId = null;
			string dateTypeDescription = null;
			var dateTypeField = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.DateType, StringComparison.InvariantCultureIgnoreCase));
			if (dateTypeField != null)
			{
				var index = dateTypeField.Value.ToObject<int>();
				dateTypeId = int.Parse(dateTypeField.TypeConfig.Options[index].Name);

				dateTypeDescription = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.DateTypeDescription, StringComparison.InvariantCultureIgnoreCase)).TypeConfig.Options[index].Name;
			}

			DateTime? actualDate = null;
			var actualDateString = Payload.CustomFields.FirstOrDefault(field => field.Name.Equals(Constants.ClickUp.FieldNames.ActualDate, StringComparison.InvariantCultureIgnoreCase))?.Value.ToObject<string>();
			if (!string.IsNullOrEmpty(actualDateString))
			{
				actualDate = (double.Parse(actualDateString)).ToDateTime();
			}
			
			var dueDate = string.IsNullOrEmpty(Payload.DueDateSecondsSinceUnixEpochAsString)? null : double.Parse(Payload.DueDateSecondsSinceUnixEpochAsString).ToDateTime();

			return new TimelinesTask(Id, acid, dateTypeId, dateTypeDescription, dueDate, actualDate);
		}
	}
}
