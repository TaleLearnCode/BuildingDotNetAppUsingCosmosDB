using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{

	/// <summary>
	/// Interface for the type providing services for working with to do <see cref="Item"/>s.
	/// </summary>
	public interface ITodoService
	{

		/// <summary>
		/// Adds the to do item in the database.
		/// </summary>
		/// <param name="item">The to do item to be added to the database.</param>
		Task AddItemAsync(Item item);

		/// <summary>
		/// Deletes the to do item from the database.
		/// </summary>
		/// <param name="id">Identifier of the item to be deleted.</param>
		/// <param name="userId">Identifier of the user owning the to do item.</param>
		Task DeleteItemAsync(string id, string userId);

		/// <summary>
		/// Gets the to do item from the database.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be retrieved from the database.</param>
		/// <param name="userId">Identifier of the user owning the to do item.</param>
		/// <returns>A <see cref="Item"/> representing the to do item retrieved from the database.</returns>
		Task<Item> GetItemAsync(string id, string userId);

		/// <summary>
		/// Gets all of the to do items for the specified user.
		/// </summary>
		/// <param name="userId">Identifier of the user to filter the to do items on.</param>
		/// <returns>An <see cref="IEnumerable{Item}"/> list representing the list of to do items for the user.</returns>
		Task<IEnumerable<Item>> GetItemsAsync(string userId);

		/// <summary>
		/// Updates the to do item in the database.
		/// </summary>
		/// <param name="item">The to do item to be updated.</param>
		Task UpdateItemAsync(Item item);

		/// <summary>
		/// Gets the to do items of the marked with the specified item status.
		/// </summary>
		/// <param name="itemStatusId">The item status identifier.</param>
		/// <returns>A <see cref="IEnumerable{Item}"/> list representing the list of to items marked with specified item status.</returns>
		Task<IEnumerable<Item>> GetItemsOfStatusAsync(string itemStatusId);

		/// <summary>
		/// Archives the supplied to do item.
		/// </summary>
		/// <param name="item">The to do item to be archived.</param>
		Task ArchiveItemAsync(Item item);

	}

}