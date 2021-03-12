using NICE.TimelinesDB.Services;
using NICE.TimelinesSync.Configuration;
using NICE.TimelinesSync.Models.ClickUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using NICE.TimelinesCommon.Models;
using NICE.TimelinesDB;

namespace NICE.TimelinesSync.Services
{
	public interface IClickUpService
	{
		Task ProcessSpace(string spaceId);
	}

	public class ClickUpService : IClickUpService
	{
		private readonly ClickUpConfig _clickUpConfig;
		private readonly IDatabaseService _databaseService;
		private readonly IHttpClientFactory _httpClientFactory;

		public ClickUpService(ClickUpConfig clickUpConfig, IDatabaseService databaseService, IHttpClientFactory httpClientFactory)
		{
			if (string.IsNullOrEmpty(clickUpConfig.AccessToken))
				throw new ApplicationException("Access token cannot be null or empty");
			
			_clickUpConfig = clickUpConfig;
			_databaseService = databaseService;
			_httpClientFactory = httpClientFactory;
		}


		public async Task ProcessSpace(string spaceId)
		{
			var allListsInSpace = new List<ClickUpList>();

			var allFoldersInSpace = (await GetFoldersInSpace(spaceId)).Folders;

			if (allFoldersInSpace.Any())
			{
				foreach (var folder in allFoldersInSpace)
				{
					var lists = (await GetListsInFolder(folder.Id)).Lists;
					if (lists.Any())
					{
						allListsInSpace.AddRange(lists);
					}
				}
			}

			var folderlessLists = (await GetListsInSpaceThatAreNotInFolders(spaceId)).Lists;
			if (folderlessLists.Any())
			{
				allListsInSpace.AddRange(folderlessLists);
			}

			foreach (var list in allListsInSpace) //a list should have a unique ACID
			{
				var tasks = (await GetTasksInList(list.Id)).Tasks;

				int? acid = null;
				foreach (var task in tasks) //TODO: for the beta: batching reduce the number of database hits - ideally to 1 - if we don't need to update anything. currently there's minimum 1 db hit per task
				{
					acid = Converters.GetACIDFromClickUpTask(task); //TODO: get the ACID from the list, not from a task.
					await _databaseService.SaveOrUpdateTimelineTask(task);
				}

				var clickUpIdsThatShouldExistInTheDatabase = tasks.Select(task => task.ClickUpTaskId);
				_databaseService.DeleteTasksAssociatedWithThisACIDExceptForTheseClickUpTaskIds(acid.Value, clickUpIdsThatShouldExistInTheDatabase);
			}
		}

		private async Task<ClickUpFolders> GetFoldersInSpace(string spaceId)
		{
			var relativeUri = $"space/{spaceId}/folder?archived=false";
			return await ReturnClickUpData<ClickUpFolders>(relativeUri);
		}

		private async Task<ClickUpLists> GetListsInSpaceThatAreNotInFolders(string spaceId)
		{
			var relativeUri = $"space/{spaceId}/list?archived=false";
			return await ReturnClickUpData<ClickUpLists>(relativeUri);
		}

		private async Task<ClickUpLists> GetListsInFolder(string folderId)
		{
			var relativeUri = $"folder/{folderId}/list?archived=false";
			return await ReturnClickUpData<ClickUpLists>(relativeUri);
		}

		private async Task<ClickUpTasks> GetTasksInList(string listId)
		{
			var relativeUri = $"list/{listId}/task?"
			                  + "custom_fields=[{\"field_id\":\"" + TimelinesCommon.Constants.ClickUp.Fields.MasterScheduleReportId + "\",\"operator\":\"=\",\"value\":true}]"
			                  + "&include_closed=true&page=0"; //todo: paging
			return await ReturnClickUpData<ClickUpTasks>(relativeUri);
		}

		private async Task<T> ReturnClickUpData<T>(string relativeUri)
		{
			var requestUri = new Uri(new Uri("https://api.clickup.com/api/v2/"), relativeUri);
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(_clickUpConfig.AccessToken);
			var httpClient = _httpClientFactory.CreateClient();
			using var response = await httpClient.SendAsync(httpRequestMessage);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception($"Non-200 received from ClickUp: {(int)response.StatusCode}");
			}
			var responseJson = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(responseJson);
		}
	}
}