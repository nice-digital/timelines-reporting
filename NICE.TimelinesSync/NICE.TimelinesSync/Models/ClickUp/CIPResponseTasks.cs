namespace NICE.TimelinesSync.Models.ClickUp
{
    public class CIPResponseTasks
    {
        public CIPResponseTasks(CIPTasks[] tasks)
        {
            CIPTasks = tasks;
        }
        public CIPTasks[] CIPTasks { get; set; }
    }
}
