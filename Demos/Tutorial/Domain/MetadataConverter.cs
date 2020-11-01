using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace TaleLearnCode.Todo.Domain
{
	public class MetadataConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(IMetadata));
		}

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