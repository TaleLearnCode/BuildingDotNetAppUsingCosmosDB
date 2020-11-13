using Newtonsoft.Json;
using System;

namespace TaleLearnCode.Todo.Domain
{

	/// <summary>
	/// Abstract class for types representing metadata types.
	/// </summary>
	/// <seealso cref="IMetadata" />
	public abstract class Metadata : IMetadata
	{

		/// <summary>
		/// Gets or sets the identifier of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item identifier.
		/// </value>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Gets or sets the name of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item name.
		/// </value>
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the metadata item.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the metadata item type.
		/// </value>
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets the name of the metadata type.
		/// </summary>
		/// <param name="metadataType">Type of the metadata.</param>
		/// <returns></returns>
		/// <exception cref="Exception">Invalid metadata type</exception>
		public static string GetMetadataTypeName(Type metadataType)
		{
			switch (metadataType.Name)
			{
				case nameof(ItemStatus):
					return MetadataTypes.ItemStatus;
				default:
					throw new Exception("Invalid metadata type");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Metadata"/> class.
		/// </summary>
		/// <param name="type">The type of the metadata item.</param>
		protected Metadata(string type)
		{
			Type = type;
		}

	}

}