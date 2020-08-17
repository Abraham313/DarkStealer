using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200011F RID: 287
	[ComVisible(true)]
	public enum CompressionLevel
	{
		// Token: 0x040005D2 RID: 1490
		None,
		// Token: 0x040005D3 RID: 1491
		Level0 = 0,
		// Token: 0x040005D4 RID: 1492
		BestSpeed,
		// Token: 0x040005D5 RID: 1493
		Level1 = 1,
		// Token: 0x040005D6 RID: 1494
		Level2,
		// Token: 0x040005D7 RID: 1495
		Level3,
		// Token: 0x040005D8 RID: 1496
		Level4,
		// Token: 0x040005D9 RID: 1497
		Level5,
		// Token: 0x040005DA RID: 1498
		Default,
		// Token: 0x040005DB RID: 1499
		Level6 = 6,
		// Token: 0x040005DC RID: 1500
		Level7,
		// Token: 0x040005DD RID: 1501
		Level8,
		// Token: 0x040005DE RID: 1502
		BestCompression,
		// Token: 0x040005DF RID: 1503
		Level9 = 9
	}
}
