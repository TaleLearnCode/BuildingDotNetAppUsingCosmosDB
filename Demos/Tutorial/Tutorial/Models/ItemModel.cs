using System.Collections.Generic;
using TaleLearnCode.Todo.Domain;

namespace Tutorial.Models
{

	public class ItemModel
	{

		public Item Item { get; set; }

		public IEnumerable<ItemStatus> ItemStatuses { get; set; }

	}

}