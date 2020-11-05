using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{
	public interface ITodoService
	{
		Task AddItemAsync(Item item);
		Task DeleteItemAsync(string id, string userId);
		Task<Item> GetItemAsync(string id, string userId);
		Task<IEnumerable<Item>> GetItemsAsync(string userId);
		Task UpdateItemAsync(Item item);
		Task<IEnumerable<Item>> GetItemsOfStatusAsync(string itemStatusId);
		Task ArchiveItemAsync(Item item);
	}
}