using System;

namespace Ionic.Zlib
{
	// Token: 0x02000124 RID: 292
	internal static class InternalConstants
	{
		// Token: 0x040005E7 RID: 1511
		internal static readonly int MAX_BITS = 15;

		// Token: 0x040005E8 RID: 1512
		internal static readonly int BL_CODES = 19;

		// Token: 0x040005E9 RID: 1513
		internal static readonly int D_CODES = 30;

		// Token: 0x040005EA RID: 1514
		internal static readonly int LITERALS = 256;

		// Token: 0x040005EB RID: 1515
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x040005EC RID: 1516
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x040005ED RID: 1517
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x040005EE RID: 1518
		internal static readonly int REP_3_6 = 16;

		// Token: 0x040005EF RID: 1519
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x040005F0 RID: 1520
		internal static readonly int REPZ_11_138 = 18;
	}
}
