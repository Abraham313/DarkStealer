using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x0200012E RID: 302
	[ComVisible(true)]
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x0000CF66 File Offset: 0x0000B166
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0000CF76 File Offset: 0x0000B176
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0000CF86 File Offset: 0x0000B186
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0000CFF0 File Offset: 0x0000B1F0
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this._innerStream = stream;
			this._Crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0000D02D File Offset: 0x0000B22D
		public long TotalBytesSlurped
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0000D03A File Offset: 0x0000B23A
		public int Crc
		{
			get
			{
				return this._Crc32.Crc32Result;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0000D047 File Offset: 0x0000B247
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0000D04F File Offset: 0x0000B24F
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0004760C File Offset: 0x0004580C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._Crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._Crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0000D058 File Offset: 0x0000B258
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0000D07A File Offset: 0x0000B27A
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0000D087 File Offset: 0x0000B287
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0000D094 File Offset: 0x0000B294
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0000D0A1 File Offset: 0x0000B2A1
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0000D02D File Offset: 0x0000B22D
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Position
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0000A6BA File Offset: 0x000088BA
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000D0C2 File Offset: 0x0000B2C2
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04000638 RID: 1592
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04000639 RID: 1593
		internal Stream _innerStream;

		// Token: 0x0400063A RID: 1594
		private CRC32 _Crc32;

		// Token: 0x0400063B RID: 1595
		private long _lengthLimit = -99L;

		// Token: 0x0400063C RID: 1596
		private bool _leaveOpen;
	}
}
