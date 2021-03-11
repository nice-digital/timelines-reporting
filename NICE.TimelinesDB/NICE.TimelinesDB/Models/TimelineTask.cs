using System;
using System.Collections.Generic;

#nullable disable

namespace NICE.TimelinesDB.Models
{
    public partial class TimelineTask
    {
	    public TimelineTask() { }
	    public TimelineTask(int acid, string clickUpId, int dateTypeId, string dateTypeDescription, DateTime? dueDate, DateTime? actualDate)
	    {
		    Acid = acid;
		    ClickUpId = clickUpId;
		    DateTypeId = dateTypeId;
		    DateTypeDescription = dateTypeDescription;
		    DueDate = dueDate;
		    ActualDate = actualDate;
	    }

        public int TimelineTaskId { get; set; }
        public int Acid { get; set; }
        public string ClickUpId { get; set; }
        public int DateTypeId { get; set; }
        public string DateTypeDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ActualDate { get; set; }

		//TODO: add ClickUpSpaceId, ClickUpListId, rename DateType to Step and add StageId + StageDescription
    }
}
