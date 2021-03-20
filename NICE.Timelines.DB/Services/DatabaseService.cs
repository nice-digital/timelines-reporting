using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NICE.Timelines.Common.Models;
using NICE.Timelines.DB.Models;

namespace NICE.Timelines.DB.Services
{
	public interface IDatabaseService
	{
		Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask);
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

		public async Task<int> SaveOrUpdateTimelineTask(ClickUpTask clickUpTask)
		{
			var timelineTaskToSaveOrUpdate = Converters.ConvertToTimelineTask(clickUpTask);

			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpTaskId.Equals(timelineTaskToSaveOrUpdate.ClickUpTaskId));

			if (existingTimelineTask != null) //it's an update
			{
				if (!TimelineTasksDiffer(existingTimelineTask, timelineTaskToSaveOrUpdate)) //task matches the task in the database, so don't bother updating it.
				{
					return 0;
				}

				existingTimelineTask.ACID = timelineTaskToSaveOrUpdate.ACID;

				existingTimelineTask.StepId = timelineTaskToSaveOrUpdate.StepId;
				existingTimelineTask.StepDescription = timelineTaskToSaveOrUpdate.StepDescription;
				existingTimelineTask.StageId = timelineTaskToSaveOrUpdate.StageId;
				existingTimelineTask.StageDescription = timelineTaskToSaveOrUpdate.StageDescription;

				existingTimelineTask.ClickUpSpaceId = timelineTaskToSaveOrUpdate.ClickUpSpaceId;
				existingTimelineTask.ClickUpFolderId = timelineTaskToSaveOrUpdate.ClickUpFolderId;
				existingTimelineTask.ClickUpTaskId = timelineTaskToSaveOrUpdate.ClickUpTaskId;

				existingTimelineTask.ActualDate = timelineTaskToSaveOrUpdate.ActualDate;
				existingTimelineTask.DueDate = timelineTaskToSaveOrUpdate.DueDate;

				_dbContext.TimelineTasks.Update(existingTimelineTask);
			}
			else //save new task
			{
				_dbContext.Add(timelineTaskToSaveOrUpdate);
			}

			return await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteTimelineTask(ClickUpTask clickUpTask)
		{
			var timelineTaskToDelete = Converters.ConvertToTimelineTask(clickUpTask);

			var existingTimelineTask = _dbContext.TimelineTasks.SingleOrDefault(t => t.ClickUpTaskId.Equals(timelineTaskToDelete.ClickUpTaskId));

			if (existingTimelineTask != null)
			{
				_dbContext.TimelineTasks.Remove(existingTimelineTask);
				await _dbContext.SaveChangesAsync();
			}
		}

		public void DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(int acid, IEnumerable<string> clickUpIdsThatShouldExistInTheDatabase)
		{
			var allTasksInDatabase = _dbContext.TimelineTasks.Where(tt => tt.ACID.Equals(acid)).ToList();

			var tasksThatNeedDeleting = allTasksInDatabase.Where(t => !clickUpIdsThatShouldExistInTheDatabase.Contains(t.ClickUpTaskId));

			_dbContext.RemoveRange(tasksThatNeedDeleting);
		}

		private static bool TimelineTasksDiffer(TimelineTask task1, TimelineTask task2)
		{
			if ((!task1.ACID.Equals(task2.ACID) || 
				(!task1.StepId.Equals(task2.StepId)) ||
				(!task1.StepDescription.Equals(task2.StepDescription)) ||
				(!task1.StageId.Equals(task2.StageId)) ||
				(!task1.StageDescription.Equals(task2.StageDescription)) ||
				(!task1.ClickUpSpaceId.Equals(task2.ClickUpSpaceId)) ||
				(!task1.ClickUpFolderId.Equals(task2.ClickUpFolderId)) ||
				(!task1.ClickUpTaskId.Equals(task2.ClickUpTaskId)) ||
				(!task1.ActualDate.Equals(task2.ActualDate)) ||
				(!task1.DueDate.Equals(task2.DueDate)))){
				return true;
			}

			return false;
		}
	}
}
