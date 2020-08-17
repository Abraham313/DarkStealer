using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000302 RID: 770
	[NullableContext(1)]
	[Nullable(0)]
	public class VersionConverter : JsonConverter
	{
		// Token: 0x06001813 RID: 6163 RVA: 0x00017707 File Offset: 0x00015907
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			if (!(value is Version))
			{
				throw new JsonSerializationException("Expected Version object value");
			}
			writer.WriteValue(value.ToString());
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00074CE8 File Offset: 0x00072EE8
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			if (reader.TokenType == JsonToken.String)
			{
				try
				{
					return new Version((string)reader.Value);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error parsing version string: {0}".FormatWith(CultureInfo.InvariantCulture, reader.Value), ex);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing version. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00017732 File Offset: 0x00015932
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Version);
		}
	}
}
