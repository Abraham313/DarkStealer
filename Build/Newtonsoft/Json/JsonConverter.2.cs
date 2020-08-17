using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001BA RID: 442
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x06000B93 RID: 2963 RVA: 0x0004EC0C File Offset: 0x0004CE0C
		public sealed override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (!((value != null) ? (value is T) : ReflectionUtils.IsNullable(typeof(T))))
			{
				throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			this.WriteJson(writer, (T)((object)value), serializer);
		}

		// Token: 0x06000B94 RID: 2964
		public abstract void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer);

		// Token: 0x06000B95 RID: 2965 RVA: 0x0004EC68 File Offset: 0x0004CE68
		[return: Nullable(2)]
		public sealed override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			bool flag;
			if (!(flag = (existingValue == null)) && !(existingValue is T))
			{
				throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith(CultureInfo.InvariantCulture, typeof(T)));
			}
			return this.ReadJson(reader, objectType, flag ? default(T) : ((T)((object)existingValue)), !flag, serializer);
		}

		// Token: 0x06000B96 RID: 2966
		public abstract T ReadJson(JsonReader reader, Type objectType, [AllowNull] T existingValue, bool hasExistingValue, JsonSerializer serializer);

		// Token: 0x06000B97 RID: 2967 RVA: 0x0000EC62 File Offset: 0x0000CE62
		public sealed override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}
	}
}
