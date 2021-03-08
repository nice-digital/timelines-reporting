using NICE.TimelinesDB.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NICE.TimelinesDB.Services
{
	public interface IDatabaseService
	{
		Task SaveOrUpdateTimelineTask(TimelineTask timelineTask);
	}

	public class DatabaseService : IDatabaseService
	{
		private readonly TimelinesContext _dbContext;

		public DatabaseService(TimelinesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SaveOrUpdateTimelineTask(TimelineTask timelineTaskToSaveOrUpdate)
		{
			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpId.Equals(timelineTaskToSaveOrUpdate.ClickUpId, StringComparison.OrdinalIgnoreCase));

			if (existingTimelineTask != null) //it's an update
			{
				existingTimelineTask.Acid = timelineTaskToSaveOrUpdate.Acid;
				existingTimelineTask.DateTypeId = timelineTaskToSaveOrUpdate.DateTypeId;
				existingTimelineTask.DateTypeDescription = timelineTaskToSaveOrUpdate.DateTypeDescription;
				existingTimelineTask.ActualDate = timelineTaskToSaveOrUpdate.ActualDate;
				existingTimelineTask.DueDate = timelineTaskToSaveOrUpdate.DueDate;

				_dbContext.TimelineTasks.Update(existingTimelineTask);
			}
			else //save new task
			{
				_dbContext.Add(timelineTaskToSaveOrUpdate);
			}

			await _dbContext.SaveChangesAsync();
		}
	}
}
