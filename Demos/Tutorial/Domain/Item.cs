using Newtonsoft.Json;

namespace TaleLearnCode.Todo.Domain
{

	public class Item
	{

		/// <summary>
		/// Gets or sets the identifier of the to do item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the to do item identifier.
		/// </value>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the to do item owner's identifier.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the identifier of the user owning the to do item.
		/// </value>
		[JsonProperty(PropertyName = "userId")]
		public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the to do item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the to do item name.
		/// </value>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description of the to do item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the to do item description.
		/// </value>
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Item"/> is completed.
		/// </summary>
		/// <value>
		///   <c>true</c> if the to do item is completed; otherwise, <c>false</c>.
		/// </value>
		[JsonProperty(PropertyName = "isComplete")]
		public bool Completed { get; set; }

		/// <summary>
		/// Gets or sets the status of the to do item.
		/// </summary>
		/// <value>
		/// A <see cref="ItemStatus"/> representing the to do item status.
		/// </value>
		[JsonProperty(PropertyName = "itemStatus")]
		public ItemStatus ItemStatus { get; set; } = new ItemStatus("New");

	}

}