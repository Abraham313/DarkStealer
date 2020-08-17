using System;
using System.IO;

namespace Ionic.BZip2
{
	// Token: 0x02000108 RID: 264
	internal class WorkItem
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0000C364 File Offset: 0x0000A564
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0000C36C File Offset: 0x0000A56C
		public BZip2Compressor Compressor { get; private set; }

		// Token: 0x06000704 RID: 1796 RVA: 0x0000C375 File Offset: 0x0000A575
		public WorkItem(int ix, int blockSize)
		{
			this.ms = new MemoryStream();
			this.bw = new BitWriter(this.ms);
			this.Compressor = new BZip2Compressor(this.bw, blockSize);
			this.index = ix;
		}

		// Token: 0x0400048F RID: 1167
		public int index;

		// Token: 0x04000490 RID: 1168
		public MemoryStream ms;

		// Token: 0x04000491 RID: 1169
		public int ordinal;

		// Token: 0x04000492 RID: 1170
		public BitWriter bw;
	}
}
