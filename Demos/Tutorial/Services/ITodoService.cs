using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Services
{
	public interface ITodoService
	{
		Task AddItemAsync(Item item);
		Task DeleteItemAsync(string id);
		Task<Item> GetItemAsync(string id);
		Task<IEnumerable<Item>> GetItemsAsync(string queryString);
		Task UpdateItemAsync(string id, Item item);
	}
}