using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001B9 RID: 441
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonConverter
	{
		// Token: 0x06000B8D RID: 2957
		public abstract void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer);

		// Token: 0x06000B8E RID: 2958
		[return: Nullable(2)]
		public abstract object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer);

		// Token: 0x06000B8F RID: 2959
		public abstract bool CanConvert(Type objectType);

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00009F16 File Offset: 0x00008116
		public virtual bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00009F16 File Offset: 0x00008116
		public virtual bool CanWrite
		{
			get
			{
				return true;
			}
		}
	}
}
