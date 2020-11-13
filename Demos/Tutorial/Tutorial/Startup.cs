using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Services;

namespace Tutorial
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
			//services.AddSingleton<ITodoService>(InitializeCosmosClientInstanceAsync(//		Configuration.GetSection("CosmosDb"),
			//		Configuration.GetSection("ArchiveStorage")).GetAwaiter().GetResult();


			services.AddSingleton<ITodoService>(
				InitializeCosmosClientInstanceAsync(
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

		private static async Task<TodoService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection, IConfigurationSection archiveConfiguration)
		{

			string databaseName = configurationSection.GetSection("DatabaseName").Value;
			string containerName = configurationSection.GetSection("ContainerName").Value;
			string account = configurationSection.GetSection("Account").Value;
			string key = configurationSection.GetSection("Key").Value;

			CosmosClient cosmosClient = new CosmosClient(account, key);

			DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
			await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerName, "/userId");

			TodoService todoService = new TodoService(
				cosmosClient,
				databaseName,
				containerName,
				archiveConfiguration.GetSection("AccountName").Value,
				archiveConfiguration.GetSection("AccountKey").Value,
				archiveConfiguration.GetSection("ContainerName").Value);

			return todoService;
		}

		// TODO: Encapsulate
		private static async Task<MetadataService> InitializeMetadataServiceAsync(IConfigurationSection configurationSection, IConfigurationSection azureCacheConfigurationSession)
		{

			string databaseName = configurationSection.GetSection("DatabaseName").Value;
			string containerName = configurationSection.GetSection("MetadataContainer").Value;
			string account = configurationSection.GetSection("Account").Value;
			string key = configurationSection.GetSection("Key").Value;

			string azureCacheConnectionString = azureCacheConfigurationSession.GetSection("ConnectionString").Value;

			CosmosClient cosmosClient = new CosmosClient(account, key);

			DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
			await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerName, "/type");

			MetadataService metadataService = new MetadataService(cosmosClient, databaseName, containerName, azureCacheConnectionString);

			return metadataService;

		}


	}

}