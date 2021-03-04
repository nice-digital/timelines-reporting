using System.Linq;
using System.Threading.Tasks;
using NICE.Timelines.Models;
using NICE.Timelines.Models.Database;

namespace NICE.Timelines.Services
{
	public interface IDataAccessService
	{
		Task SaveOrUpdateTask(Message clickUpMessage);
		Task DeleteTask(Message clickUpMessage);
	}

	public class DataAccessService : IDataAccessService
	{
		private readonly TimelinesContext _dbContext;

		public DataAccessService(TimelinesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SaveOrUpdateTask(Message clickUpMessage)
		{
			var timelineTaskFromWebhookMessage = clickUpMessage.ToTimelinesTask();
			
			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpId.Equals(timelineTaskFromWebhookMessage.ClickUpId));

			if (existingTimelineTask != null) //it's an update
			{
				existingTimelineTask.Acid = timelineTaskFromWebhookMessage.Acid;
				existingTimelineTask.DateTypeId = timelineTaskFromWebhookMessage.DateTypeId;
				existingTimelineTask.DateTypeDescription = timelineTaskFromWebhookMessage.DateTypeDescription;
				existingTimelineTask.ActualDate = timelineTaskFromWebhookMessage.ActualDate;
				existingTimelineTask.DueDate = timelineTaskFromWebhookMessage.DueDate;

				_dbContext.TimelineTasks.Update(existingTimelineTask);
			}
			else //save new task
			{
				_dbContext.Add(timelineTaskFromWebhookMessage);
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteTask(Message clickUpMessage)
		{
			var timelineTaskFromWebhookMessage = clickUpMessage.ToTimelinesTask();

			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpId.Equals(timelineTaskFromWebhookMessage.ClickUpId));

			if (existingTimelineTask != null)
			{
				_dbContext.TimelineTasks.Remove(existingTimelineTask);
				await _dbContext.SaveChangesAsync();
			}

		}
	}
}
