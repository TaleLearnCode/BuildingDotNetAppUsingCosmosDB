using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Services;

namespace TaleLearnCode.Todo.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddSingleton<ITodoService>(
				InitializeTodoServiceAsync(
					Configuration.GetSection("CosmosDb"),
					Configuration.GetSection("ArchiveStorage")).GetAwaiter().GetResult());

			services.AddSingleton<IMetadataService>(
				InitializeMetadataServiceAsync(
					Configuration.GetSection("CosmosDb"),
					Configuration.GetSection("AzureCache")).GetAwaiter().GetResult());

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
									name: "default",
									pattern: "{controller=Item}/{action=Index}/{id?}");
			});
		}

		private static async Task<TodoService> InitializeTodoServiceAsync(IConfigurationSection cosmosConfiguration, IConfigurationSection archiveConfiguration)
		{

			var cosmosSettings = GetCosmosConfigurationValues(cosmosConfiguration);

			CosmosClient cosmosClient = new CosmosClient(cosmosSettings.endpoint, cosmosSettings.key);

			DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosSettings.databaseName);
			await databaseResponse.Database.CreateContainerIfNotExistsAsync(cosmosSettings.itemContainerName, "/userId");

			TodoService todoService = new TodoService(
				cosmosClient,
				cosmosSettings.databaseName,
				cosmosSettings.itemContainerName,
				archiveConfiguration.GetSection("AccountName").Value,
				archiveConfiguration.GetSection("AccountKey").Value,
				archiveConfiguration.GetSection("ContainerName").Value);

			return todoService;
		}

		private static async Task<MetadataService> InitializeMetadataServiceAsync(IConfigurationSection cosmosConfiguration, IConfigurationSection azureCacheConfigurationSession)
		{

			var cosmosSettings = GetCosmosConfigurationValues(cosmosConfiguration);

			string azureCacheConnectionString = azureCacheConfigurationSession.GetSection("ConnectionString").Value;

			CosmosClient cosmosClient = new CosmosClient(cosmosSettings.endpoint, cosmosSettings.key);

			DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosSettings.databaseName);
			await databaseResponse.Database.CreateContainerIfNotExistsAsync(cosmosSettings.metadataContainerName, "/type");

			MetadataService metadataService = new MetadataService(cosmosClient, cosmosSettings.databaseName, cosmosSettings.metadataContainerName, azureCacheConnectionString);

			return metadataService;

		}

		private static (string endpoint, string key, string databaseName, string metadataContainerName, string itemContainerName) GetCosmosConfigurationValues(IConfigurationSection cosmosConfiguration)
		{
			return (
				cosmosConfiguration.GetSection("Endpoint").Value,
				cosmosConfiguration.GetSection("AccountKey").Value,
				cosmosConfiguration.GetSection("DatabaseName").Value,
				cosmosConfiguration.GetSection("MetadataContainerName").Value,
				cosmosConfiguration.GetSection("ItemContainerName").Value);
		}

	}

}