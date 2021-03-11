using System;
using System.Collections.Generic;

#nullable disable

namespace NICE.TimelinesDB.Models
{
    public partial class TimelineTask
    {
	    public TimelineTask() { }

	    public TimelineTask(int acid, int stepId, string stepDescription, int stageId, string stageDescription, string clickUpSpaceId, string clickUpFolderId, string clickUpTaskId, DateTime? dueDate, DateTime? actualDate)
	    {
		    ACID = acid;
		    StepId = stepId;
		    StepDescription = stepDescription;
		    StageId = stageId;
		    StageDescription = stageDescription;
		    ClickUpSpaceId = clickUpSpaceId;
		    ClickUpFolderId = clickUpFolderId;
		    ClickUpTaskId = clickUpTaskId;
		    DueDate = dueDate;
		    ActualDate = actualDate;
	    }

	    public int TimelineTaskId { get; set; }
        public int ACID { get; set; }
		
        public int StepId { get; set; }
        public string StepDescription { get; set; }

        public int StageId { get; set; }
        public string StageDescription { get; set; }

		public string ClickUpSpaceId { get; set; }
        public string ClickUpFolderId { get; set; }
		public string ClickUpTaskId { get; set; }

		public DateTime? DueDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }
}
