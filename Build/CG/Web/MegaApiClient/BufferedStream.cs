using System;
using System.IO;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000168 RID: 360
	internal class BufferedStream : Stream
	{
		// Token: 0x060009D7 RID: 2519 RVA: 0x0000DBB9 File Offset: 0x0000BDB9
		public BufferedStream(Stream innerStream)
		{
			this.innerStream = innerStream;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		public byte[] Buffer
		{
			get
			{
				return this.streamBuffer;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0000DBE0 File Offset: 0x0000BDE0
		public int BufferOffset
		{
			get
			{
				return this.streamBufferDataStartIndex;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public int AvailableCount
		{
			get
			{
				return this.streamBufferDataCount;
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0000A639 File Offset: 0x00008839
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0004CDA0 File Offset: 0x0004AFA0
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			do
			{
				int num2 = Math.Min(this.streamBufferDataCount, count);
				if (num2 != 0)
				{
					Array.Copy(this.streamBuffer, this.streamBufferDataStartIndex, buffer, offset, num2);
					offset += num2;
					count -= num2;
					this.streamBufferDataStartIndex += num2;
					this.streamBufferDataCount -= num2;
					num += num2;
				}
				if (count == 0)
				{
					break;
				}
				this.streamBufferDataStartIndex = 0;
				this.streamBufferDataCount = 0;
				this.FillBuffer();
			}
			while (this.streamBufferDataCount != 0);
			return num;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0004CE28 File Offset: 0x0004B028
		public void FillBuffer()
		{
			for (;;)
			{
				int num = this.streamBufferDataStartIndex + this.streamBufferDataCount;
				int num2 = this.streamBuffer.Length - num;
				if (num2 == 0)
				{
					break;
				}
				int num3 = this.innerStream.Read(this.streamBuffer, num, num2);
				if (num3 == 0)
				{
					break;
				}
				this.streamBufferDataCount += num3;
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0000A639 File Offset: 0x00008839
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00009F16 File Offset: 0x00008116
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0000A639 File Offset: 0x00008839
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x040006E5 RID: 1765
		private const int BufferSize = 65536;

		// Token: 0x040006E6 RID: 1766
		private Stream innerStream;

		// Token: 0x040006E7 RID: 1767
		private byte[] streamBuffer = new byte[65536];

		// Token: 0x040006E8 RID: 1768
		private int streamBufferDataStartIndex;

		// Token: 0x040006E9 RID: 1769
		private int streamBufferDataCount;
	}
}
