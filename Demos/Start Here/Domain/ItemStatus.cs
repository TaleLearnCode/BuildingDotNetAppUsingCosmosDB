namespace TaleLearnCode.Todo.Domain
{

	/// <summary>
	/// Represents the status of a to do item.
	/// </summary>
	/// <seealso cref="Metadata" />
	public class ItemStatus : Metadata
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemStatus"/> class.
		/// </summary>
		public ItemStatus() : base(MetadataTypes.ItemStatus) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemStatus"/> class.
		/// </summary>
		/// <param name="name">The name of the item status.</param>
		public ItemStatus(string name) : base(MetadataTypes.ItemStatus)
		{
			Id = name;
			Name = name;
		}

	}

}