using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	public class TodoService : ITodoService
	{

		private Container _container;

		public TodoService(CosmosClient cosmosClient, string databaseName, string containerName)
		{
			_container = cosmosClient.GetContainer(databaseName, containerName);
		}

		public async Task AddItemAsync(Item item)
		{
			await Common.CreateItemAsync<Item>(_container, item, item.Id);
		}

		public async Task DeleteItemAsync(string id)
		{
			await Common.DeleteItemAsync<Item>(_container, id, id);
		}

		public async Task<Item> GetItemAsync(string id)
		{
			return await Common.GetItemAsync<Item>(_container, id, id);
		}

		public async Task<IEnumerable<Item>> GetItemsAsync()
		{
			return await ExecuteQueryAsync(new QueryDefinition("SELECT * FROM c"));
		}

		public async Task<IEnumerable<Item>> ExecuteQueryAsync(QueryDefinition queryDefinition)
		{
			return await Common.ExecuteQueryAsync<Item>(_container, queryDefinition);
		}

		public async Task UpdateItemAsync(string id, Item item)
		{
			await _container.UpsertItemAsync<Item>(item, new PartitionKey(id));
		}

	}

}