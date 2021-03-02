using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaleLearnCode.Todo.Services
{

	/// <summary>
	/// Provides a set of common Cosmos DB functionality for the rest of the library to use.
	/// </summary>
	internal static class Common
	{

		/// <summary>
		/// Creates an item in the Cosmos DB database.
		/// </summary>
		/// <typeparam name="T">The type being created in the database.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to save the item.</param>
		/// <param name="item">The item to be saved in the database.</param>
		/// <param name="partitionKey">The partition key of the item being saved.</param>
		/// <returns>The resulting database object.</returns>
		internal static async Task<T> CreateItemAsync<T>(Container container, T item, string partitionKey)
		{
			ItemResponse<T> itemResponse = await container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
			return itemResponse.Resource;
		}

		/// <summary>
		/// Deletes the item from the Cosmos DB database.
		/// </summary>
		/// <typeparam name="T">The type being deleted from the database.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to delete the item.</param>
		/// <param name="id">Identifier of the item to be deleted.</param>
		/// <param name="partitionKey">The partition key of the item to be deleted.</param>
		internal static async Task DeleteItemAsync<T>(Container container, string id, string partitionKey)
		{
			await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
		}

		/// <summary>
		/// Updates the specified item in the Cosmos DB database.
		/// </summary>
		/// <typeparam name="T">The type to be updated.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to update the item.</param>
		/// <param name="partitionKey">The partition key of the item to be updated.</param>
		/// <param name="item">The item to be updated.</param>
		internal static async Task UpdateItemAsync<T>(Container container, string partitionKey, T item)
		{
			await container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey));
		}

		/// <summary>
		/// Gets the item from the Cosmos DB database by its identifier.
		/// </summary>
		/// <typeparam name="T">The type to be retrieved.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to retrieve the item.</param>
		/// <param name="id">Identifier of the item to be retrieved.</param>
		/// <param name="partitionKey">The partition key of the item to be retrieved.</param>
		/// <returns>The requested item from the database.</returns>
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

		/// <summary>
		/// Executes the specified query against the database.
		/// </summary>
		/// <typeparam name="T">The type to be queried against.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to execute the query.</param>
		/// <param name="queryString">The query string to be executed against the database.</param>
		/// <returns>A <see cref="IEnumerable{T}"/> representing the results of the supplied query.</returns>
		internal static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(Container container, string queryString)
		{
			return await ExecuteQueryAsync<T>(container, new QueryDefinition(queryString));
		}

		/// <summary>
		/// Executes the specified query.
		/// </summary>
		/// <typeparam name="T">The type to be queried against.</typeparam>
		/// <param name="container">The <see cref="Container"/> where to execute the query.</param>
		/// <param name="queryDefinition">The <see cref="QueryDefinition"/> to be executed.</param>
		/// <returns>A <see cref="IEnumerable{T}"/> representing the results of the supplied query.</returns>
		internal static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(Container container, QueryDefinition queryDefinition)
		{
			var query = container.GetItemQueryIterator<T>(queryDefinition);
			List<T> results = new List<T>();
			while (query.HasMoreResults)
				results.AddRange((await query.ReadNextAsync()).ToList());
			return results;
		}

	}

}