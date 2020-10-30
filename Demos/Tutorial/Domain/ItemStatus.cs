namespace TaleLearnCode.Todo.Domain
{

	public class ItemStatus : Metadata
	{

		public ItemStatus() : base(MetadataTypes.ItemStatus) { }

		public ItemStatus(string name) : base(MetadataTypes.ItemStatus)
		{
			Name = name;
		}

	}

}