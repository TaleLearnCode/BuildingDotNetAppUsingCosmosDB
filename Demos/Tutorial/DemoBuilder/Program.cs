using Microsoft.Azure.Cosmos;
using System;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;

namespace DemoBuilder
{
	class Program
	{

		static async System.Threading.Tasks.Task Main(string[] args)
		{

			Console.WriteLine("Press any key to generate initial demo data...");
			Console.ReadKey();


			CosmosClient cosmosClient = new CosmosClient(Settings.CosmosEndpoint, Settings.CosmosKey);
			MetadataService metadataService = new MetadataService(cosmosClient, Settings.CosmosDatabaseName, Settings.MetadataContainerName);

			await metadataService.CreateMetadataAsync(new ItemStatus("New") { Id = "New" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Completed") { Id = "Completed" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Waiting on Others") { Id = "Waiting on Other" });
			await metadataService.CreateMetadataAsync(new ItemStatus("Stalled") { Id = "Stalled" });

		}

	}

}