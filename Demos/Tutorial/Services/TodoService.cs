using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
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
			await Common.CreateItemAsync(_container, item, item.UserId);
		}

		public async Task DeleteItemAsync(string id, string userId)
		{
			await Common.DeleteItemAsync<Item>(_container, id, userId);
		}

		public async Task<Item> GetItemAsync(string id, string userId)
		{
			return await Common.GetItemAsync<Item>(_container, id, userId);
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(string userId)
		{
			return await ExecuteQueryAsync(
				new QueryDefinition("SELECT * FROM c WHERE c.userId = @UserId")
					.WithParameter("@UserId", userId));
		}

		public async Task<IEnumerable<Item>> ExecuteQueryAsync(QueryDefinition queryDefinition)
		{
			return await Common.ExecuteQueryAsync<Item>(_container, queryDefinition);
		}

		public async Task UpdateItemAsync(Item item)
		{
			await _container.UpsertItemAsync(item, new PartitionKey(item.UserId));
		}

		public async Task<IEnumerable<Item>> GetItemsOfStatusAsync(string itemStatusId)
		{
			return await ExecuteQueryAsync(
				new QueryDefinition("SELECT * FROM c WHERE c.itemStatus.id = @ItemStatusId")
				.WithParameter("@ItemStatusId", itemStatusId));
		}

	}

}