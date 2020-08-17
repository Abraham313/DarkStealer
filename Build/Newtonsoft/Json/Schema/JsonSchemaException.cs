using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000296 RID: 662
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00014B94 File Offset: 0x00012D94
		public int LineNumber { get; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00014B9C File Offset: 0x00012D9C
		public int LinePosition { get; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00014BA4 File Offset: 0x00012DA4
		public string Path { get; }

		// Token: 0x060013BA RID: 5050 RVA: 0x0000F20F File Offset: 0x0000D40F
		public JsonSchemaException()
		{
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0000F217 File Offset: 0x0000D417
		public JsonSchemaException(string message) : base(message)
		{
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0000F220 File Offset: 0x0000D420
		public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0000F22A File Offset: 0x0000D42A
		public JsonSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00014BAC File Offset: 0x00012DAC
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
