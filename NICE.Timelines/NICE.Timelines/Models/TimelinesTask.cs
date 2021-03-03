using System;

namespace NICE.Timelines.Models
{
	public class TimelinesTask
	{
		public TimelinesTask(Guid clickUpId, int acid, int? dateTypeId, string dateTypeDescription, DateTime? dueDate, DateTime? actualDate)
		{
			ClickUpId = clickUpId;
			ACID = acid;
			DateTypeId = dateTypeId;
			DateTypeDescription = dateTypeDescription;
			DueDate = dueDate;
			ActualDate = actualDate;
		}

		public Guid ClickUpId { get; private set; }
		public int ACID { get; private set; }
		public int? DateTypeId { get; private set; }
		public string DateTypeDescription { get; private set; }

		public DateTime? DueDate { get; private set; }
		public DateTime? ActualDate { get; private set; }
	}
}
