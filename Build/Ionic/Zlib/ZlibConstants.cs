using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200012B RID: 299
	[ComVisible(true)]
	public static class ZlibConstants
	{
		// Token: 0x04000626 RID: 1574
		public const int WindowBitsMax = 15;

		// Token: 0x04000627 RID: 1575
		public const int WindowBitsDefault = 15;

		// Token: 0x04000628 RID: 1576
		public const int Z_OK = 0;

		// Token: 0x04000629 RID: 1577
		public const int Z_STREAM_END = 1;

		// Token: 0x0400062A RID: 1578
		public const int Z_NEED_DICT = 2;

		// Token: 0x0400062B RID: 1579
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x0400062C RID: 1580
		public const int Z_DATA_ERROR = -3;

		// Token: 0x0400062D RID: 1581
		public const int Z_BUF_ERROR = -5;

		// Token: 0x0400062E RID: 1582
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x0400062F RID: 1583
		public const int WorkingBufferSizeMin = 1024;
	}
}
