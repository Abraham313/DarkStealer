using System;
using System.IO;

namespace Ionic.BZip2
{
	// Token: 0x020000FF RID: 255
	internal class BitWriter
	{
		// Token: 0x06000696 RID: 1686 RVA: 0x0000C13E File Offset: 0x0000A33E
		public BitWriter(Stream s)
		{
			this.output = s;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0000C14D File Offset: 0x0000A34D
		public byte RemainingBits
		{
			get
			{
				return (byte)(this.accumulator >> 32 - this.nAccumulatedBits & 255U);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0000C169 File Offset: 0x0000A369
		public int NumRemainingBits
		{
			get
			{
				return this.nAccumulatedBits;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0000C171 File Offset: 0x0000A371
		public int TotalBytesWrittenOut
		{
			get
			{
				return this.totalBytesWrittenOut;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0003AAD0 File Offset: 0x00038CD0
		public void Reset()
		{
			this.accumulator = 0U;
			this.nAccumulatedBits = 0;
			this.totalBytesWrittenOut = 0;
			this.output.Seek(0L, SeekOrigin.Begin);
			this.output.SetLength(0L);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0003AB1C File Offset: 0x00038D1C
		public void WriteBits(int nbits, uint value)
		{
			int i = this.nAccumulatedBits;
			uint num = this.accumulator;
			while (i >= 8)
			{
				this.output.WriteByte((byte)(num >> 24 & 255U));
				this.totalBytesWrittenOut++;
				num <<= 8;
				i -= 8;
			}
			this.accumulator = (num | value << 32 - i - nbits);
			this.nAccumulatedBits = i + nbits;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0000C179 File Offset: 0x0000A379
		public void WriteByte(byte b)
		{
			this.WriteBits(8, (uint)b);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0003AB88 File Offset: 0x00038D88
		public void WriteInt(uint u)
		{
			this.WriteBits(8, u >> 24 & 255U);
			this.WriteBits(8, u >> 16 & 255U);
			this.WriteBits(8, u >> 8 & 255U);
			this.WriteBits(8, u & 255U);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000C183 File Offset: 0x0000A383
		public void Flush()
		{
			this.WriteBits(0, 0U);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0003ABD8 File Offset: 0x00038DD8
		public void FinishAndPad()
		{
			this.Flush();
			if (this.NumRemainingBits > 0)
			{
				byte value = (byte)(this.accumulator >> 24 & 255U);
				this.output.WriteByte(value);
				this.totalBytesWrittenOut++;
			}
		}

		// Token: 0x04000408 RID: 1032
		private uint accumulator;

		// Token: 0x04000409 RID: 1033
		private int nAccumulatedBits;

		// Token: 0x0400040A RID: 1034
		private Stream output;

		// Token: 0x0400040B RID: 1035
		private int totalBytesWrittenOut;
	}
}
