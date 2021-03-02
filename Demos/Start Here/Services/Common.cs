using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}


	}


}