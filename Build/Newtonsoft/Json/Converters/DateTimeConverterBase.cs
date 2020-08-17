using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002F4 RID: 756
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x00073810 File Offset: 0x00071A10
		[NullableContext(1)]
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?);
		}
	}
}
