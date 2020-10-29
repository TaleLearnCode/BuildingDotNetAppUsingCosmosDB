using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaleLearnCode.Todo.Services
{

	internal static class Common
	{

		internal static async Task<T> CreateItemAsync<T>(Container container, T item, string partitionKey)
		{
			ItemResponse<T> itemResponse = await container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
			return itemResponse.Resource;
		}

		internal static async Task DeleteItemAsync<T>(Container container, string id, string partitionKey)
		{
			await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
		}

		internal static async Task<T> GetItemAsync<T>(Container container, string id, string partitionKey)
		{
			try
			{
				ItemResponse<T> response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
				return response.Resource;
			}
			catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				return default;
			}
		}

		internal static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(Container container, string queryString)
		{
			return await ExecuteQueryAsync<T>(container, new QueryDefinition(queryString));
		}

		internal static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(Container container, QueryDefinition queryDefinition)
		{
			var query = container.GetItemQueryIterator<T>(queryDefinition);
			List<T> results = new List<T>();
			while (query.HasMoreResults)
				results.AddRange((await query.ReadNextAsync()).ToList());
			return results;
		}

		internal static async Task UpdateItemAsync<T>(Container container, string partitionKey, T item)
		{
			await container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey));
		}

	}

}