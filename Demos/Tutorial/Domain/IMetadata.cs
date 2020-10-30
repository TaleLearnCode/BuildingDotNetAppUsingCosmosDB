namespace TaleLearnCode.Todo.Domain
{
	public interface IMetadata
	{
		string Id { get; set; }
		string Name { get; set; }
		string Type { get; set; }
	}
}