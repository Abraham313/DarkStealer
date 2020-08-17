using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002FA RID: 762
	[NullableContext(1)]
	[Nullable(0)]
	public class EntityKeyMemberConverter : JsonConverter
	{
		// Token: 0x060017D6 RID: 6102 RVA: 0x00073D80 File Offset: 0x00071F80
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			string value2 = (string)EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
			object value3 = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
			Type type = (value3 != null) ? value3.GetType() : null;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			writer.WriteValue(value2);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Type") : "Type");
			writer.WriteValue((type != null) ? type.FullName : null);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			if (type != null)
			{
				string value4;
				if (JsonSerializerInternalWriter.TryConvertToString(value3, type, out value4))
				{
					writer.WriteValue(value4);
				}
				else
				{
					writer.WriteValue(value3);
				}
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteEndObject();
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00073E88 File Offset: 0x00072088
		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			reader.ReadAndAssert();
			if (reader.TokenType == JsonToken.PropertyName)
			{
				object value = reader.Value;
				if (string.Equals((value != null) ? value.ToString() : null, propertyName, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00073ED8 File Offset: 0x000720D8
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			EntityKeyMemberConverter.EnsureReflectionObject(objectType);
			object obj = EntityKeyMemberConverter._reflectionObject.Creator(new object[0]);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
			reader.ReadAndAssert();
			ReflectionObject reflectionObject = EntityKeyMemberConverter._reflectionObject;
			object target = obj;
			string member = "Key";
			object value = reader.Value;
			reflectionObject.SetValue(target, member, (value != null) ? value.ToString() : null);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
			reader.ReadAndAssert();
			object value2 = reader.Value;
			Type type = Type.GetType((value2 != null) ? value2.ToString() : null);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
			reader.ReadAndAssert();
			EntityKeyMemberConverter._reflectionObject.SetValue(obj, "Value", serializer.Deserialize(reader, type));
			reader.ReadAndAssert();
			return obj;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000174A6 File Offset: 0x000156A6
		private static void EnsureReflectionObject(Type objectType)
		{
			if (EntityKeyMemberConverter._reflectionObject == null)
			{
				EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, new string[]
				{
					"Key",
					"Value"
				});
			}
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000174D0 File Offset: 0x000156D0
		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember", false);
		}

		// Token: 0x04000CE0 RID: 3296
		private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

		// Token: 0x04000CE1 RID: 3297
		private const string KeyPropertyName = "Key";

		// Token: 0x04000CE2 RID: 3298
		private const string TypePropertyName = "Type";

		// Token: 0x04000CE3 RID: 3299
		private const string ValuePropertyName = "Value";

		// Token: 0x04000CE4 RID: 3300
		[Nullable(2)]
		private static ReflectionObject _reflectionObject;
	}
}
