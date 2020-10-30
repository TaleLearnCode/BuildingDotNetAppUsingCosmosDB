using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	public class MetadataService : IMetadataService
	{

		private Container _container;

		public MetadataService(CosmosClient cosmosClient, string databaseName, string containerName)
		{
			_container = cosmosClient.GetContainer(databaseName, containerName);
		}

		public async Task<T> CreateMetadataAsync<T>(T metadata) where T : IMetadata
		{
			return await Common.CreateItemAsync<T>(_container, metadata, metadata.Type);
		}

		public async Task<IEnumerable<T>> GetMetadataAsync<T>() where T : IMetadata, new()
		{
			return await Common.ExecuteQueryAsync<T>(
				_container,
				new QueryDefinition("SELECT * FROM c WHERE c.type = @Type")
					.WithParameter("@Type", Metadata.GetMetadataTypeNameByType(typeof(T))));
		}

	}

}