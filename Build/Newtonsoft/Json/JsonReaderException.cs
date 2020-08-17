using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001C8 RID: 456
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonReaderException : JsonException
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0000F1F7 File Offset: 0x0000D3F7
		public int LineNumber { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0000F1FF File Offset: 0x0000D3FF
		public int LinePosition { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0000F207 File Offset: 0x0000D407
		[Nullable(2)]
		public string Path { [NullableContext(2)] get; }

		// Token: 0x06000C28 RID: 3112 RVA: 0x0000F20F File Offset: 0x0000D40F
		public JsonReaderException()
		{
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0000F217 File Offset: 0x0000D417
		public JsonReaderException(string message) : base(message)
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0000F220 File Offset: 0x0000D420
		public JsonReaderException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0000F22A File Offset: 0x0000D42A
		public JsonReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0000F234 File Offset: 0x0000D434
		public JsonReaderException(string message, string path, int lineNumber, int linePosition, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0000F255 File Offset: 0x0000D455
		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return JsonReaderException.Create(reader, message, null);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0000F25F File Offset: 0x0000D45F
		internal static JsonReaderException Create(JsonReader reader, string message, [Nullable(2)] Exception ex)
		{
			return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00050058 File Offset: 0x0004E258
		internal static JsonReaderException Create([Nullable(2)] IJsonLineInfo lineInfo, string path, string message, [Nullable(2)] Exception ex)
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
			return new JsonReaderException(message, path, lineNumber, linePosition, ex);
		}
	}
}
