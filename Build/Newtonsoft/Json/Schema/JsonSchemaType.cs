using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020002A1 RID: 673
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x04000B92 RID: 2962
		None = 0,
		// Token: 0x04000B93 RID: 2963
		String = 1,
		// Token: 0x04000B94 RID: 2964
		Float = 2,
		// Token: 0x04000B95 RID: 2965
		Integer = 4,
		// Token: 0x04000B96 RID: 2966
		Boolean = 8,
		// Token: 0x04000B97 RID: 2967
		Object = 16,
		// Token: 0x04000B98 RID: 2968
		Array = 32,
		// Token: 0x04000B99 RID: 2969
		Null = 64,
		// Token: 0x04000B9A RID: 2970
		Any = 127
	}
}
