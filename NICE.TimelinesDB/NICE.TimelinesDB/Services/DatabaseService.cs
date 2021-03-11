using NICE.TimelinesCommon.Models;
using NICE.TimelinesDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NICE.TimelinesDB.Services
{
	public interface IDatabaseService
	{
		Task SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
		Task DeleteTimelineTask(ClickUpTask clickUpTask);
		void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase);
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
				if (!TimelineTasksDiffer(existingTimelineTask, timelineTaskToSaveOrUpdate)) //task matches the task in the database, so don't bother updating it.
				{
					return;
				}

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

		public void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase)
		{
			var allTasksInDatabase = _dbContext.TimelineTasks.Where(tt => tt.Acid.Equals(acid)).ToList();

			var tasksThatNeedDeleting = allTasksInDatabase.Where(t => !clickUpIdsThatShouldExistInTheDatabase.Contains(t.ClickUpId));

			_dbContext.RemoveRange(tasksThatNeedDeleting);
		}

		private static bool TimelineTasksDiffer(TimelineTask task1, TimelineTask task2)
		{
			if ((!task1.Acid.Equals(task2.Acid) || 
				(!task1.DateTypeId.Equals(task2.DateTypeId)) ||
				(!task1.DateTypeDescription.Equals(task2.DateTypeDescription)) ||
				(!task1.ActualDate.Equals(task2.ActualDate)) ||
				(!task1.DueDate.Equals(task2.DueDate)))){
				return true;
			}

			return false;
		}
	}
}
