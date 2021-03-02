using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace TaleLearnCode.Todo.Domain
{

	/// <summary>
	/// Converts an object implementing the <see cref="IMetadata"/> interface to and from JSON.
	/// </summary>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	public class MetadataConverter : JsonConverter
	{

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(IMetadata));
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		/// <exception cref="Exception">Invalid Metadata type found in the supplied JSON; unable to deserialize.</exception>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{

			JObject jObject = JObject.Load(reader);
			var metadataType = (string)jObject["type"];

			object target;
			switch (metadataType)
			{
				case MetadataTypes.ItemStatus:
					target = new ItemStatus();
					break;
				default:
					throw new Exception("Invalid Metadata type found in the supplied JSON; unable to deserialize.");
			}

			serializer.Populate(jObject.CreateReader(), target);
			return target;

		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <exception cref="Exception">Unrecognizable metadata type; unable to serialize.</exception>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{

			Type writeType;
			if (value is ItemStatus)
				writeType = typeof(ItemStatus);
			else
				throw new Exception("Unrecognizable metadata type; unable to serialize.");

			serializer.Serialize(writer, value, writeType);

		}

	}

}