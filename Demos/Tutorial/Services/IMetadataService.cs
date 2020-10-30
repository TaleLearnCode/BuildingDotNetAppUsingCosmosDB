using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{
	public interface IMetadataService
	{
		Task<T> CreateMetadataAsync<T>(T metadata) where T : IMetadata;
		Task<IEnumerable<T>> GetMetadataAsync<T>() where T : IMetadata, new();
		Task<IEnumerable<T>> GetMetadataFromCacheAsync<T>() where T : IMetadata, new();
	}
}