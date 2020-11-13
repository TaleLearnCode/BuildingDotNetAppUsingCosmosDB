using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	/// <summary>
	/// Interface for the type providing services for working with metadata.
	/// </summary>
	public interface IMetadataService
	{

		/// <summary>
		/// Creates a metadata object in the database.
		/// </summary>
		/// <typeparam name="T">The type of metadata being created.</typeparam>
		/// <param name="metadata">The metadata to be created in the database.</param>
		/// <returns>A type implementing the <see cref="IMetadata"/> interface representing the metadata saved in the database.</returns>
		Task<T> CreateMetadataAsync<T>(T metadata) where T : IMetadata;

		/// <summary>
		/// Gets the metadata from the database.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be retrieved.</typeparam>
		/// <returns>A type implementing the <see cref="IMetadata"/> interface representing the metadata returned from the database.</returns>
		Task<IEnumerable<T>> GetMetadataAsync<T>() where T : IMetadata, new();

		/// <summary>
		/// Gets the metadata from the Redis cache.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be retrieved.</typeparam>
		/// <returns>A type implementing the <see cref="IMetadata"/> interface representing the metadata returned from the Redis cache.</returns>
		Task<IEnumerable<T>> GetMetadataFromCacheAsync<T>() where T : IMetadata, new();

		/// <summary>
		/// Clears the metadata of the specified type from the Redis cache.
		/// </summary>
		/// <typeparam name="T">The type of the metadata to be cleared.</typeparam>
		void ClearMetadataTypeFromCache<T>() where T : IMetadata;

	}

}