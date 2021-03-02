using System.Collections.Generic;
using TaleLearnCode.Todo.Domain;

namespace TaleLearnCode.Todo.Web.Models
{

	/// <summary>
	/// Represents the model used for pages working with to do items.
	/// </summary>
	public class ItemModel
	{

		/// <summary>
		/// Gets or sets the to do <see cref="Item"/> to work with.
		/// </summary>
		/// <value>
		/// A <see cref="Item"/> representing the item being worked with.
		/// </value>
		public Item Item { get; set; }

		/// <summary>
		/// Gets or sets the list of possible to do item statuses.
		/// </summary>
		/// <value>
		/// A <see cref="IEnumerable{ItemStatus}"/> representing the list of item statuses.
		/// </value>
		public IEnumerable<ItemStatus> ItemStatuses { get; set; }

	}

}