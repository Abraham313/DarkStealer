using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000EB RID: 235
	[Flags]
	[ComVisible(true)]
	public enum ZipEntryTimestamp
	{
		// Token: 0x04000348 RID: 840
		None = 0,
		// Token: 0x04000349 RID: 841
		DOS = 1,
		// Token: 0x0400034A RID: 842
		Windows = 2,
		// Token: 0x0400034B RID: 843
		Unix = 4,
		// Token: 0x0400034C RID: 844
		InfoZip1 = 8
	}
}
