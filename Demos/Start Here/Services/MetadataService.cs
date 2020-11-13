using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	/// <summary>
	/// Provides services for working with metadata.
	/// </summary>
	/// <seealso cref="IMetadataService" />
	public class MetadataService : IMetadataService
	{

		private Container _cosmosContainer;
		private Lazy<ConnectionMultiplexer> _redisConnection;

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataService"/> class.
		/// </summary>
		/// <param name="cosmosClient">A <see cref="CosmosClient"/> reference to use when communicating with the Cosmos DB database.</param>
		/// <param name="cosmosDatabaseName">Name of the Cosmos DB database to connect to.</param>
		/// <param name="cosmosContainerName">Name of the Cosmos DB container to connect to.</param>
		public MetadataService(
			CosmosClient cosmosClient,
			string cosmosDatabaseName,
			string cosmosContainerName)
		{
			_cosmosContainer = cosmosClient.GetContainer(cosmosDatabaseName, cosmosContainerName);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataService"/> class.
		/// </summary>
		/// <param name="cosmosClient">A <see cref="CosmosClient"/> reference to use when communicating with the Cosmos DB database.</param>
		/// <param name="cosmosDatabaseName">Name of the cosmos database to connect to.</param>
		/// <param name="cosmosContainerName">Name of the cosmos container to connect to.</param>
		/// <param name="redisConnectionString">The connection string to the Redis cache database.</param>
		public MetadataService(
			CosmosClient cosmosClient,
			string cosmosDatabaseName,
			string cosmosContainerName,
			string redisConnectionString)
		{
			_cosmosContainer = cosmosClient.GetContainer(cosmosDatabaseName, cosmosContainerName);
			_redisConnection = new Lazy<ConnectionMultiplexer>(() =>
			{
				return ConnectionMultiplexer.Connect(redisConnectionString);
			});
		}

		/// <summary>
		/// Creates a metadata object in the database.
		/// </summary>
		/// <typeparam name="T">The type of metadata being created.</typeparam>
		/// <param name="metadata">The metadata to be created in the database.</param>
		/// <returns>
		/// A type implementing the <see cref="IMetadata" /> interface representing the metadata saved in the database.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task<T> CreateMetadataAsync<T>(T metadata) where T : IMetadata
		{
			return await Common.CreateItemAsync<T>(_cosmosContainer, metadata, metadata.Type);
		}

		/// <summary>
		/// Gets the metadata from the database.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be retrieved.</typeparam>
		/// <returns>
		/// A type implementing the <see cref="IMetadata" /> interface representing the metadata returned from the database.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task<IEnumerable<T>> GetMetadataAsync<T>() where T : IMetadata, new()
		{
			return await Common.ExecuteQueryAsync<T>(
				_cosmosContainer,
				new QueryDefinition("SELECT * FROM c WHERE c.type = @Type")
					.WithParameter("@Type", Metadata.GetMetadataTypeName(typeof(T))));
		}

		/// <summary>
		/// Gets the metadata from the Redis cache.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be retrieved.</typeparam>
		/// <returns>
		/// A type implementing the <see cref="IMetadata" /> interface representing the metadata returned from the Redis cache.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task<IEnumerable<T>> GetMetadataFromCacheAsync<T>() where T : IMetadata, new()
		{
			if (_redisConnection is null) throw new Exception("The Redis connection must be configured before trying to read from Redis.");
			IDatabase cache = _redisConnection.Value.GetDatabase();
			var metadataTypeName = Metadata.GetMetadataTypeName(typeof(T));
			if (!cache.KeyExists(metadataTypeName))
				cache.StringSet(
					metadataTypeName,
					JsonConvert.SerializeObject(await GetMetadataAsync<T>()));
			return JsonConvert.DeserializeObject<IEnumerable<T>>(cache.StringGet(metadataTypeName));
		}

		/// <summary>
		/// Clears the metadata of the specified type from the Redis cache.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be cleared.</typeparam>
		/// <exception cref="System.NotImplementedException"></exception>
		public void ClearMetadataTypeFromCache<T>() where T : IMetadata
		{
			if (_redisConnection is null) throw new Exception("The Redis connection must be configured before trying to read from Redis.");
			IDatabase cache = _redisConnection.Value.GetDatabase();
			cache.KeyDelete(Metadata.GetMetadataTypeName(typeof(T)));
		}

	}

}