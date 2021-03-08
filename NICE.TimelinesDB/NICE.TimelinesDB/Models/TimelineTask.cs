using System;
using System.Collections.Generic;

#nullable disable

namespace NICE.TimelinesDB.Models
{
    public partial class TimelineTask
    {
        public int TimelineTaskId { get; set; }
        public int Acid { get; set; }
        public string ClickUpId { get; set; }
        public int DateTypeId { get; set; }
        public string DateTypeDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }
}
