using NICE.TimelinesDB.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using NICE.TimelinesCommon.Models;

namespace NICE.TimelinesDB.Services
{
	public interface IDatabaseService
	{
		Task SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
		Task DeleteTimelineTask(ClickUpTask clickUpTask);
	}

	public class DatabaseService : IDatabaseService
	{
		private readonly TimelinesContext _dbContext;

		public DatabaseService(TimelinesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SaveOrUpdateTimelineTask(ClickUpTask clickUpTask)
		{
			var timelineTaskToSaveOrUpdate = Converters.ConvertToTimelineTask(clickUpTask);

			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpId.Equals(timelineTaskToSaveOrUpdate.ClickUpId));

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

		public async Task DeleteTimelineTask(ClickUpTask clickUpTask)
		{
			var timelineTaskToDelete = Converters.ConvertToTimelineTask(clickUpTask);

			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpId.Equals(timelineTaskToDelete.ClickUpId));

			if (existingTimelineTask != null)
			{
				_dbContext.TimelineTasks.Remove(existingTimelineTask);
				await _dbContext.SaveChangesAsync();
			}
		}
	}
}
