using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	public class MetadataService : IMetadataService
	{

		private Container _cosmosContainer;
		private Lazy<ConnectionMultiplexer> _redisConnection;

		public MetadataService(
			CosmosClient cosmosClient,
			string cosmosDatabaseName,
			string cosmosContainerName)
		{
			_cosmosContainer = cosmosClient.GetContainer(cosmosDatabaseName, cosmosContainerName);
		}

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

		public async Task<T> CreateMetadataAsync<T>(T metadata) where T : IMetadata
		{
			return await Common.CreateItemAsync<T>(_cosmosContainer, metadata, metadata.Type);
		}

		public async Task<IEnumerable<T>> GetMetadataAsync<T>() where T : IMetadata, new()
		{
			return await Common.ExecuteQueryAsync<T>(
				_cosmosContainer,
				new QueryDefinition("SELECT * FROM c WHERE c.type = @Type")
					.WithParameter("@Type", Metadata.GetMetadataTypeNameByType(typeof(T))));
		}

		public async Task<IEnumerable<T>> GetMetadataFromCacheAsync<T>() where T : IMetadata, new()
		{
			if (_redisConnection is null) throw new Exception("The Redis connection must be configured before trying to read from Redis.");
			IDatabase cache = _redisConnection.Value.GetDatabase();
			var metadataTypeName = Metadata.GetMetadataTypeNameByType(typeof(T));
			if (!cache.KeyExists(metadataTypeName))
				cache.StringSet(
					metadataTypeName,
					JsonConvert.SerializeObject(await GetMetadataAsync<T>()));
			return JsonConvert.DeserializeObject<IEnumerable<T>>(cache.StringGet(metadataTypeName));
		}

		public void ClearMetadataTypeFromCache<T>() where T : IMetadata
		{
			if (_redisConnection is null) throw new Exception("The Redis connection must be configured before trying to read from Redis.");
			IDatabase cache = _redisConnection.Value.GetDatabase();
			cache.KeyDelete(Metadata.GetMetadataTypeNameByType(typeof(T)));
		}

	}

}