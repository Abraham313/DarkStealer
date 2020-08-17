using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001BE RID: 446
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonException : Exception
	{
		// Token: 0x06000BA0 RID: 2976 RVA: 0x00009C87 File Offset: 0x00007E87
		public JsonException()
		{
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00009C8F File Offset: 0x00007E8F
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00009C98 File Offset: 0x00007E98
		public JsonException(string message, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0000A422 File Offset: 0x00008622
		public JsonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0000ECBF File Offset: 0x0000CEBF
		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}
