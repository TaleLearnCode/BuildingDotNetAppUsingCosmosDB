using Newtonsoft.Json;

namespace TaleLearnCode.Todo.Domain
{

	public class Item
	{

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "userId")]
		public string UserId { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "isComplete")]
		public bool Completed { get; set; }

	}

}