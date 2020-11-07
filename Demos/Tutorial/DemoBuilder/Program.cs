using Microsoft.Azure.Cosmos;
using System;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;

namespace DemoBuilder
{
	class Program
	{

		private const string _userId = "6F9BD7D9-6336-4EF6-97BA-99F9ABFBC15E";

		static async System.Threading.Tasks.Task Main(string[] args)
		{

			Console.WriteLine("Press any key to generate initial demo data...");
			Console.ReadKey();

			CosmosClient cosmosClient = new CosmosClient(Settings.CosmosEndpoint, Settings.CosmosKey);
			MetadataService metadataService = new MetadataService(cosmosClient, Settings.CosmosDatabaseName, Settings.MetadataContainerName);
			TodoService todoService = new TodoService(cosmosClient, Settings.CosmosDatabaseName, Settings.ItemContainerName, Settings.StorageAccountName, Settings.StorageAccountKey, Settings.ArchiveContainerName);

			var newStatus = await metadataService.CreateMetadataAsync(new ItemStatus("New") { Id = "New" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Completed") { Id = "Completed" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Waiting on Others") { Id = "Waiting on Other" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Stalled") { Id = "Stalled" });

			await todoService.AddItemAsync(new Item() { Id = Guid.NewGuid().ToString(), Name = "Update Denormalized Data", Description = "Using the change feed, update item status values embedded within todo items when the master record is updated.", ItemStatus = newStatus, UserId = _userId });
			await todoService.AddItemAsync(new Item() { Id = Guid.NewGuid().ToString(), Name = "Update Cache", Description = "When an item status record is updated, update the cache", ItemStatus = newStatus, UserId = _userId });
			await todoService.AddItemAsync(new Item() { Id = Guid.NewGuid().ToString(), Name = "Archive Completed Tasks", Description = "When a task is completed, it should be archived after a certain amount of time.", ItemStatus = newStatus, UserId = _userId });
			await todoService.AddItemAsync(new Item() { Id = Guid.NewGuid().ToString(), Name = "Database Programmability", Description = "Come up with a scenario for a stored procedure, trigger, and / or UDF.", ItemStatus = newStatus, UserId = _userId });
			await todoService.AddItemAsync(new Item() { Id = Guid.NewGuid().ToString(), Name = "Convert MVC to Blazor", Description = "Covert the MVC application to a Blazor application.Might also make sense to have a Razor version of the site.", ItemStatus = newStatus, UserId = _userId });

		}
	}
}