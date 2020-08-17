using System;

namespace Newtonsoft.Json
{
	// Token: 0x020001E4 RID: 484
	public enum WriteState
	{
		// Token: 0x040008EE RID: 2286
		Error,
		// Token: 0x040008EF RID: 2287
		Closed,
		// Token: 0x040008F0 RID: 2288
		Object,
		// Token: 0x040008F1 RID: 2289
		Array,
		// Token: 0x040008F2 RID: 2290
		Constructor,
		// Token: 0x040008F3 RID: 2291
		Property,
		// Token: 0x040008F4 RID: 2292
		Start
	}
}
