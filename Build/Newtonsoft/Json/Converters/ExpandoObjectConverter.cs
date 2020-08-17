using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002FB RID: 763
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpandoObjectConverter : JsonConverter
	{
		// Token: 0x060017DC RID: 6108 RVA: 0x00009B58 File Offset: 0x00007D58
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000174DE File Offset: 0x000156DE
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			return this.ReadValue(reader);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00073F90 File Offset: 0x00072190
		[return: Nullable(2)]
		private object ReadValue(JsonReader reader)
		{
			if (!reader.MoveToContent())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
			}
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadObject(reader);
			}
			if (tokenType == JsonToken.StartArray)
			{
				return this.ReadList(reader);
			}
			if (JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
			{
				return reader.Value;
			}
			throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00074004 File Offset: 0x00072204
		private object ReadList(JsonReader reader)
		{
			IList<object> list = new List<object>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						return list;
					}
					object item = this.ReadValue(reader);
					list.Add(item);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00074050 File Offset: 0x00072250
		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndObject)
						{
							return dictionary;
						}
					}
				}
				else
				{
					string key = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					object value = this.ReadValue(reader);
					dictionary[key] = value;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x000174E7 File Offset: 0x000156E7
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
