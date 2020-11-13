using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;

namespace TaleLearnCode.Todo.Functions
{

	public class ItemChangeFeed
	{

		private readonly ITodoService _todoService;

		public ItemChangeFeed(ITodoService todoService)
		{
			_todoService = todoService;
		}

		[FunctionName("ItemChangeFeed")]
		public async Task Run([CosmosDBTrigger(
			databaseName: Settings.DatabaseName,
			collectionName: Settings.ItemContainerName,
			ConnectionStringSetting = "CosmosConnectionString",
			LeaseCollectionName = "leases",
			LeaseCollectionPrefix = "ItemChangeFeed",
			CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
		{

			if (documents != null && documents.Count > 0)
				foreach (var document in documents)
					await _todoService.ArchiveItemAsync(JsonConvert.DeserializeObject<Item>(document.ToString()));

		}

	}

}