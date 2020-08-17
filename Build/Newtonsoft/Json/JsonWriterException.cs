using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001D8 RID: 472
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class JsonWriterException : JsonException
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00010C12 File Offset: 0x0000EE12
		[Nullable(2)]
		public string Path { [NullableContext(2)] get; }

		// Token: 0x06000E30 RID: 3632 RVA: 0x0000F20F File Offset: 0x0000D40F
		public JsonWriterException()
		{
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0000F217 File Offset: 0x0000D417
		public JsonWriterException(string message) : base(message)
		{
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0000F220 File Offset: 0x0000D420
		public JsonWriterException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0000F22A File Offset: 0x0000D42A
		public JsonWriterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00010C1A File Offset: 0x0000EE1A
		public JsonWriterException(string message, string path, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Path = path;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00010C2B File Offset: 0x0000EE2B
		internal static JsonWriterException Create(JsonWriter writer, string message, [Nullable(2)] Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00010C3A File Offset: 0x0000EE3A
		internal static JsonWriterException Create(string path, string message, [Nullable(2)] Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, path, ex);
		}
	}
}
