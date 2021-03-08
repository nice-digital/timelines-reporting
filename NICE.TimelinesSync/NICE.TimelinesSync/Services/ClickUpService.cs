using NICE.TimelinesSync.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NICE.TimelinesDB.Models;
using NICE.TimelinesDB.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using NICE.TimelinesSync.Models.ClickUp;
using NICE.TimelinesSync.Common;

namespace NICE.TimelinesSync.Services
{
	public interface IClickUpService
	{
		Task<IEnumerable<TimelineTask>> GetTasks();
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

		public async Task<IEnumerable<TimelineTask>> GetTasks()
		{
			var httpClient = _httpClientFactory.CreateClient(Constants.ClickUpHttpClientName);

			var requestUri = new Uri(new Uri("https://api.clickup.com/api/v2/"),
				$"team/{_clickUpConfig.TeamId}/task?page=0&list_ids%5B%5D=%5B%22{_clickUpConfig.ListId}22%5D&order_by=due_date&reverse=true&include_closed=true" + 
				"&custom_fields=[{\"field_id\":\"5bb24b9e-a86d-4ad5-b301-9ce08f431b1e\",\"operator\":\"=\",\"value\":true}]");

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
			var clickUpTasks = JsonSerializer.Deserialize<CIPResponseTasks>(responseJson);

			
			throw new NotImplementedException();
		}
	}
}
