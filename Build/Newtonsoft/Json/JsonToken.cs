using System;

namespace Newtonsoft.Json
{
	// Token: 0x020001D1 RID: 465
	public enum JsonToken
	{
		// Token: 0x04000878 RID: 2168
		None,
		// Token: 0x04000879 RID: 2169
		StartObject,
		// Token: 0x0400087A RID: 2170
		StartArray,
		// Token: 0x0400087B RID: 2171
		StartConstructor,
		// Token: 0x0400087C RID: 2172
		PropertyName,
		// Token: 0x0400087D RID: 2173
		Comment,
		// Token: 0x0400087E RID: 2174
		Raw,
		// Token: 0x0400087F RID: 2175
		Integer,
		// Token: 0x04000880 RID: 2176
		Float,
		// Token: 0x04000881 RID: 2177
		String,
		// Token: 0x04000882 RID: 2178
		Boolean,
		// Token: 0x04000883 RID: 2179
		Null,
		// Token: 0x04000884 RID: 2180
		Undefined,
		// Token: 0x04000885 RID: 2181
		EndObject,
		// Token: 0x04000886 RID: 2182
		EndArray,
		// Token: 0x04000887 RID: 2183
		EndConstructor,
		// Token: 0x04000888 RID: 2184
		Date,
		// Token: 0x04000889 RID: 2185
		Bytes
	}
}
