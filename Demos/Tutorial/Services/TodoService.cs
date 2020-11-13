using Microsoft.Azure.Cosmos;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	/// <summary>
	/// Provides services for working with to do <see cref="Item" />
	/// </summary>
	/// <seealso cref="ITodoService" />
	public class TodoService : ITodoService
	{

		private readonly Container _container;
		private readonly CloudBlobContainer _blobContainer;

		/// <summary>
		/// Initializes a new instance of the <see cref="TodoService"/> class.
		/// </summary>
		/// <param name="cosmosClient">A <see cref="CosmosClient"/> reference to use when communicating with the Cosmos DB database.</param>
		/// <param name="databaseName">Name of the Cosmos DB database to connect to.</param>
		/// <param name="containerName">Name of the Cosmos DB container to connect to.</param>
		/// <param name="storageAccountName">Name of the storage account to connect to for the archive.</param>
		/// <param name="storageAccountKey">The storage account key to use when connecting to the archive storage.</param>
		/// <param name="blobContainerName">Name of the archive BLOB container.</param>
		public TodoService(
			CosmosClient cosmosClient,
			string databaseName,
			string containerName,
			string storageAccountName,
			string storageAccountKey,
			string blobContainerName)
		{
			_container = cosmosClient.GetContainer(databaseName, containerName);

			var storageAccount = new CloudStorageAccount(
				new StorageCredentials(
					storageAccountName,
					storageAccountKey),
				useHttps: true);

			var blobClient = storageAccount.CreateCloudBlobClient();

			_blobContainer = blobClient.GetContainerReference(blobContainerName);

		}

		/// <summary>
		/// Adds the to do item in the database.
		/// </summary>
		/// <param name="item">The to do item to be added to the database.</param>
		public async Task AddItemAsync(Item item)
		{
			await Common.CreateItemAsync(_container, item, item.UserId);
		}

		/// <summary>
		/// Deletes the to do item from the database.
		/// </summary>
		/// <param name="id">Identifier of the item to be deleted.</param>
		/// <param name="userId">Identifier of the user owning the to do item.</param>
		public async Task DeleteItemAsync(string id, string userId)
		{
			await Common.DeleteItemAsync<Item>(_container, id, userId);
		}

		/// <summary>
		/// Updates the to do item in the database.
		/// </summary>
		/// <param name="item">The to do item to be updated.</param>
		public async Task UpdateItemAsync(Item item)
		{
			await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.UserId));
		}

		/// <summary>
		/// Gets the to do item from the database.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be retrieved from the database.</param>
		/// <param name="userId">Identifier of the user owning the to do item.</param>
		/// <returns>
		/// A <see cref="Item" /> representing the to do item retrieved from the database.
		/// </returns>
		public async Task<Item> GetItemAsync(string id, string userId)
		{
			return await Common.GetItemAsync<Item>(_container, id, userId);
		}

		/// <summary>
		/// Gets all of the to do items for the specified user.
		/// </summary>
		/// <param name="userId">Identifier of the user to filter the to do items on.</param>
		/// <returns>
		/// An <see cref="IEnumerable{Item}" /> list representing the list of to do items for the user.
		/// </returns>
		public async Task<IEnumerable<Item>> GetItemsAsync(string userId)
		{
			return await ExecuteQueryAsync(
				new QueryDefinition("SELECT * FROM c WHERE c.userId = @UserId")
					.WithParameter("@UserId", userId));
		}

		/// <summary>
		/// Executes the query asynchronous.
		/// </summary>
		/// <param name="queryDefinition">The query definition.</param>
		/// <returns></returns>
		public async Task<IEnumerable<Item>> ExecuteQueryAsync(QueryDefinition queryDefinition)
		{
			return await Common.ExecuteQueryAsync<Item>(_container, queryDefinition);
		}

		/// <summary>
		/// Gets the to do items of the marked with the specified item status.
		/// </summary>
		/// <param name="itemStatusId">The item status identifier.</param>
		/// <returns>
		/// A <see cref="IEnumerable{Item}" /> list representing the list of to items marked with specified item status.
		/// </returns>
		public async Task<IEnumerable<Item>> GetItemsOfStatusAsync(string itemStatusId)
		{
			return await ExecuteQueryAsync(
				new QueryDefinition("SELECT * FROM c WHERE c.itemStatus.id = @ItemStatusId")
				.WithParameter("@ItemStatusId", itemStatusId));
		}

		/// <summary>
		/// Archives the supplied to do item.
		/// </summary>
		/// <param name="item">The to do item to be archived.</param>
		public async Task ArchiveItemAsync(Item item)
		{
			var blobName = $"{item.UserId}-{item.ItemStatus.Name}-{item.Id}";
			var blob = _blobContainer.GetBlockBlobReference(blobName);
			var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(item));
			await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
		}

	}

}