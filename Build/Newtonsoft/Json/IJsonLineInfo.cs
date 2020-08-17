using System;

namespace Newtonsoft.Json
{
	// Token: 0x020001B4 RID: 436
	public interface IJsonLineInfo
	{
		// Token: 0x06000B22 RID: 2850
		bool HasLineInfo();

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000B23 RID: 2851
		int LineNumber { get; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000B24 RID: 2852
		int LinePosition { get; }
	}
}
