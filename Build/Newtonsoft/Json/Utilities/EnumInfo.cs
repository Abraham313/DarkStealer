using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000210 RID: 528
	[NullableContext(1)]
	[Nullable(0)]
	internal class EnumInfo
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x00011A9E File Offset: 0x0000FC9E
		public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
		{
			this.IsFlags = isFlags;
			this.Values = values;
			this.Names = names;
			this.ResolvedNames = resolvedNames;
		}

		// Token: 0x040009A0 RID: 2464
		public readonly bool IsFlags;

		// Token: 0x040009A1 RID: 2465
		public readonly ulong[] Values;

		// Token: 0x040009A2 RID: 2466
		public readonly string[] Names;

		// Token: 0x040009A3 RID: 2467
		public readonly string[] ResolvedNames;
	}
}
