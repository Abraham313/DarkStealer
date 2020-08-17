using System;

namespace Ionic.Zlib
{
	// Token: 0x0200011A RID: 282
	internal class WorkItem
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x00044E98 File Offset: 0x00043098
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x04000587 RID: 1415
		public byte[] buffer;

		// Token: 0x04000588 RID: 1416
		public byte[] compressed;

		// Token: 0x04000589 RID: 1417
		public int crc;

		// Token: 0x0400058A RID: 1418
		public int index;

		// Token: 0x0400058B RID: 1419
		public int ordinal;

		// Token: 0x0400058C RID: 1420
		public int inputBytesAvailable;

		// Token: 0x0400058D RID: 1421
		public int compressedBytesAvailable;

		// Token: 0x0400058E RID: 1422
		public ZlibCodec compressor;
	}
}
