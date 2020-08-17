using System;

namespace Ionic.BZip2
{
	// Token: 0x02000105 RID: 261
	internal static class BZip2
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x0003E198 File Offset: 0x0003C398
		internal static T[][] InitRectangularArray<T>(int d1, int d2)
		{
			T[][] array = new T[d1][];
			for (int i = 0; i < d1; i++)
			{
				array[i] = new T[d2];
			}
			return array;
		}

		// Token: 0x04000475 RID: 1141
		public static readonly int BlockSizeMultiple = 100000;

		// Token: 0x04000476 RID: 1142
		public static readonly int MinBlockSize = 1;

		// Token: 0x04000477 RID: 1143
		public static readonly int MaxBlockSize = 9;

		// Token: 0x04000478 RID: 1144
		public static readonly int MaxAlphaSize = 258;

		// Token: 0x04000479 RID: 1145
		public static readonly int MaxCodeLength = 23;

		// Token: 0x0400047A RID: 1146
		public static readonly char RUNA = '\0';

		// Token: 0x0400047B RID: 1147
		public static readonly char RUNB = '\u0001';

		// Token: 0x0400047C RID: 1148
		public static readonly int NGroups = 6;

		// Token: 0x0400047D RID: 1149
		public static readonly int G_SIZE = 50;

		// Token: 0x0400047E RID: 1150
		public static readonly int N_ITERS = 4;

		// Token: 0x0400047F RID: 1151
		public static readonly int MaxSelectors = 2 + 900000 / BZip2.G_SIZE;

		// Token: 0x04000480 RID: 1152
		public static readonly int NUM_OVERSHOOT_BYTES = 20;

		// Token: 0x04000481 RID: 1153
		internal static readonly int QSORT_STACK_SIZE = 1000;
	}
}
