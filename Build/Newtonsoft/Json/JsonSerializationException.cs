using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001CA RID: 458
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0000F274 File Offset: 0x0000D474
		public int LineNumber { get; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0000F27C File Offset: 0x0000D47C
		public int LinePosition { get; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0000F284 File Offset: 0x0000D484
		[Nullable(2)]
		public string Path { [NullableContext(2)] get; }

		// Token: 0x06000C34 RID: 3124 RVA: 0x0000F20F File Offset: 0x0000D40F
		public JsonSerializationException()
		{
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0000F217 File Offset: 0x0000D417
		public JsonSerializationException(string message) : base(message)
		{
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0000F220 File Offset: 0x0000D420
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0000F22A File Offset: 0x0000D42A
		public JsonSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0000F28C File Offset: 0x0000D48C
		public JsonSerializationException(string message, string path, int lineNumber, int linePosition, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0000F2AD File Offset: 0x0000D4AD
		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return JsonSerializationException.Create(reader, message, null);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0000F2B7 File Offset: 0x0000D4B7
		internal static JsonSerializationException Create(JsonReader reader, string message, [Nullable(2)] Exception ex)
		{
			return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00050098 File Offset: 0x0004E298
		internal static JsonSerializationException Create([Nullable(2)] IJsonLineInfo lineInfo, string path, string message, [Nullable(2)] Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			int lineNumber;
			int linePosition;
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				lineNumber = lineInfo.LineNumber;
				linePosition = lineInfo.LinePosition;
			}
			else
			{
				lineNumber = 0;
				linePosition = 0;
			}
			return new JsonSerializationException(message, path, lineNumber, linePosition, ex);
		}
	}
}
