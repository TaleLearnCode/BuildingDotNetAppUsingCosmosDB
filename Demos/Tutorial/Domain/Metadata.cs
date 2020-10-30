using Newtonsoft.Json;
using System;

namespace TaleLearnCode.Todo.Domain
{

	public abstract class Metadata : IMetadata
	{

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		public static string GetMetadataTypeNameByType(Type metadataType)
		{
			switch (metadataType.Name)
			{
				case nameof(ItemStatus):
					return MetadataTypes.ItemStatus;
				default:
					throw new Exception("Invalid metadata type");
			}
		}

		protected Metadata(string type)
		{
			Type = type;
		}

	}

}