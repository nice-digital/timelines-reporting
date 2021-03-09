using NICE.TimelinesDB.Models;
using NICE.TimelinesDB.Services;
using NICE.TimelinesSync.Configuration;
using NICE.TimelinesSync.Models.ClickUp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace NICE.TimelinesSync.Services
{
	public interface IClickUpService
	{
		Task SaveAndUpdateTasks();
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

		public async Task SaveAndUpdateTasks()
		{
			var httpClient = _httpClientFactory.CreateClient();

			//do //TODO: paging
			//{

			var requestUri = new Uri(new Uri("https://api.clickup.com/api/v2/"),
				$"list/{_clickUpConfig.ListId}/task?"
				+ "custom_fields=[{\"field_id\":\"" + TimelinesCommon.Constants.ClickUp.Fields.KeyDateId + "\",\"operator\":\"=\",\"value\":true}]"
				+ "&include_closed=true&page=0" //todo: paging
				);

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

			httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(_clickUpConfig.AccessToken);

			string responseJson;
			using (var response = await httpClient.SendAsync(httpRequestMessage))
			{
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new Exception($"Non-200 received from clickup: {(int)response.StatusCode}");
				}
				responseJson = await response.Content.ReadAsStringAsync();
			}
			var clickUpTasks = JsonSerializer.Deserialize<ClickUpTasks>(responseJson);

			foreach (var clickUpTask in clickUpTasks.Tasks)
			{
				await _databaseService.SaveOrUpdateTimelineTask(clickUpTask);
			}

			//} while (b);
		}
	}
}
