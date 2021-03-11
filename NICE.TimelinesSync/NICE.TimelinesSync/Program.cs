using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NICE.TimelinesSync.Configuration;
using NICE.TimelinesSync.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NICE.TimelinesDB.Models;
using NICE.TimelinesDB.Services;

namespace NICE.TimelinesSync
{
	class Program
	{
		private static ServiceProvider _serviceProvider;

		static async Task Main(string[] args)
		{
			//unusually for a console app, using appsettings.json + secrets.json for configuration, for consistency with other projects and also for the secrets support - because it's a public repo.
			IConfiguration Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			
			var clickUpConfig = new ClickUpConfig();
			Configuration.Bind("ClickUp", clickUpConfig);
			
			RegisterServices(clickUpConfig, Configuration.GetConnectionString("DefaultConnection"));

			var scope = _serviceProvider.CreateScope();
			await scope.ServiceProvider.GetRequiredService<ISyncService>().Process(); //entry point

			DisposeServices();
		}


		private static void RegisterServices(ClickUpConfig clickUpConfig, string databaseConnectionString)
		{
			var services = new ServiceCollection(); //again unusually for a console app, setting up DI, in order to support testing.

			services.AddSingleton(serviceProvider => clickUpConfig);
			services.AddTransient<ISyncService, SyncService>();
			services.AddTransient<IClickUpService, ClickUpService>();
			services.AddHttpClient();

			services.AddDbContext<TimelinesContext>(options => options.UseSqlServer(databaseConnectionString));
			services.AddTransient<IDatabaseService, DatabaseService>();

			_serviceProvider = services.BuildServiceProvider(validateScopes: true);
		}

		private static void DisposeServices()
		{
			if (_serviceProvider == null)
			{
				return;
			}
			if (_serviceProvider is IDisposable)
			{
				((IDisposable)_serviceProvider).Dispose();
			}
		}
	}
}
