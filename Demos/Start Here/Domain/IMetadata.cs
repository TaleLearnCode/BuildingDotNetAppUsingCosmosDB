namespace TaleLearnCode.Todo.Domain
{

	/// <summary>
	/// Interface for types representing metadata items.
	/// </summary>
	public interface IMetadata
	{

		/// <summary>
		/// Gets or sets the identifier of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item identifier.
		/// </value>
		string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item type.
		/// </value>
		string Type { get; set; }

	}

}