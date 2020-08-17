using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002F1 RID: 753
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CustomCreationConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x060017B7 RID: 6071 RVA: 0x000173E2 File Offset: 0x000155E2
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00073254 File Offset: 0x00071454
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x060017B9 RID: 6073
		public abstract T Create(Type objectType);

		// Token: 0x060017BA RID: 6074 RVA: 0x0000EC62 File Offset: 0x0000CE62
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
