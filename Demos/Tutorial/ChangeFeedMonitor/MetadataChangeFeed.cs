using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;

namespace TaleLearnCode.Todo.Functions
{
	public class MetadataChangeFeed
	{

		private readonly ITodoService _todoService;

		public MetadataChangeFeed(ITodoService todoService)
		{

			if (todoService is null) throw new ArgumentNullException(nameof(todoService));
			_todoService = todoService;
		}

		[FunctionName("MetadataChangeFeed")]
		public async Task Run([CosmosDBTrigger(
			databaseName: Settings.DatabaseName,
			collectionName: Settings.MetadataContainerName,
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			LeaseCollectionPrefix = "ChangeFeedMonitor",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{
			if (documents != null && documents.Count > 0)
			{
				foreach (var document in documents)
				{

					var settings = new JsonSerializerSettings();
					settings.Converters.Add(new MetadataConverter());
					var metadata = JsonConvert.DeserializeObject<IMetadata>(document.ToString(), settings);

					if (metadata.GetType() == typeof(ItemStatus))
						await ProcessItemStatusAsync((ItemStatus)metadata, log);

				}

			}
		}

		private async Task ProcessItemStatusAsync(ItemStatus itemStatus, ILogger log)
		{
			var itemsToUpdate = await _todoService.GetItemsOfStatusAsync(itemStatus.Id);
			foreach (var itemToUpdate in itemsToUpdate)
			{
				itemToUpdate.ItemStatus = itemStatus;
				await _todoService.UpdateItemAsync(itemToUpdate);
				log.LogWarning($"Updated Item {itemToUpdate.Id}");
			}

		}

	}
}