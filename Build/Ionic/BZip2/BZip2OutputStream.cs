using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ionic.BZip2
{
	// Token: 0x02000106 RID: 262
	[ComVisible(true)]
	public class BZip2OutputStream : Stream
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0000C2EA File Offset: 0x0000A4EA
		public BZip2OutputStream(Stream output) : this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0000C2F9 File Offset: 0x0000A4F9
		public BZip2OutputStream(Stream output, int blockSize) : this(output, blockSize, false)
		{
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0000C304 File Offset: 0x0000A504
		public BZip2OutputStream(Stream output, bool leaveOpen) : this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0003E240 File Offset: 0x0003C440
		public BZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
		{
			if (blockSize < BZip2.MinBlockSize || blockSize > BZip2.MaxBlockSize)
			{
				string message = string.Format("blockSize={0} is out of range; must be between {1} and {2}", blockSize, BZip2.MinBlockSize, BZip2.MaxBlockSize);
				throw new ArgumentException(message, "blockSize");
			}
			this.output = output;
			if (!this.output.CanWrite)
			{
				throw new ArgumentException("The stream is not writable.", "output");
			}
			this.bw = new BitWriter(this.output);
			this.blockSize100k = blockSize;
			this.compressor = new BZip2Compressor(this.bw, blockSize);
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.EmitHeader();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0003E300 File Offset: 0x0003C500
		public override void Close()
		{
			if (this.output != null)
			{
				Stream stream = this.output;
				this.Finish();
				if (!this.leaveOpen)
				{
					stream.Close();
				}
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0000C313 File Offset: 0x0000A513
		public override void Flush()
		{
			if (this.output != null)
			{
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0003E330 File Offset: 0x0003C530
		private void EmitHeader()
		{
			byte[] array = new byte[]
			{
				66,
				90,
				104,
				0
			};
			array[3] = (byte)(48 + this.blockSize100k);
			byte[] array2 = array;
			this.output.Write(array2, 0, array2.Length);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0003E370 File Offset: 0x0003C570
		private void EmitTrailer()
		{
			this.bw.WriteByte(23);
			this.bw.WriteByte(114);
			this.bw.WriteByte(69);
			this.bw.WriteByte(56);
			this.bw.WriteByte(80);
			this.bw.WriteByte(144);
			this.bw.WriteInt(this.combinedCRC);
			this.bw.FinishAndPad();
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0003E3EC File Offset: 0x0003C5EC
		private void Finish()
		{
			try
			{
				int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
				this.compressor.CompressAndWrite();
				this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
				this.combinedCRC ^= this.compressor.Crc32;
				this.EmitTrailer();
			}
			finally
			{
				this.output = null;
				this.compressor = null;
				this.bw = null;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0000C333 File Offset: 0x0000A533
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0003E470 File Offset: 0x0003C670
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = count;
			do
			{
				int num3 = this.compressor.Fill(buffer, offset, num2);
				if (num3 != num2)
				{
					int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
					this.compressor.CompressAndWrite();
					this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
					this.combinedCRC ^= this.compressor.Crc32;
					offset += num3;
				}
				num2 -= num3;
				num += num3;
			}
			while (num2 > 0);
			this.totalBytesWrittenIn += num;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0000C33B File Offset: 0x0000A53B
		public override bool CanWrite
		{
			get
			{
				if (this.output == null)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.output.CanWrite;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0000C35B File Offset: 0x0000A55B
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				return (long)this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0000A639 File Offset: 0x00008839
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0003E57C File Offset: 0x0003C77C
		[Conditional("Trace")]
		private void TraceOutput(BZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != BZip2OutputStream.TraceBits.None)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.Green;
				Console.Write("{0:000} PBOS ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x04000482 RID: 1154
		private int totalBytesWrittenIn;

		// Token: 0x04000483 RID: 1155
		private bool leaveOpen;

		// Token: 0x04000484 RID: 1156
		private BZip2Compressor compressor;

		// Token: 0x04000485 RID: 1157
		private uint combinedCRC;

		// Token: 0x04000486 RID: 1158
		private Stream output;

		// Token: 0x04000487 RID: 1159
		private BitWriter bw;

		// Token: 0x04000488 RID: 1160
		private int blockSize100k;

		// Token: 0x04000489 RID: 1161
		private BZip2OutputStream.TraceBits desiredTrace = BZip2OutputStream.TraceBits.Crc | BZip2OutputStream.TraceBits.Write;

		// Token: 0x02000107 RID: 263
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400048B RID: 1163
			None = 0U,
			// Token: 0x0400048C RID: 1164
			Crc = 1U,
			// Token: 0x0400048D RID: 1165
			Write = 2U,
			// Token: 0x0400048E RID: 1166
			All = 4294967295U
		}
	}
}
