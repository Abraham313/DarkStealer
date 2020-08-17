using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000DF RID: 223
	[ComVisible(true)]
	public class CountingStream : Stream
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x0002F050 File Offset: 0x0002D250
		public CountingStream(Stream stream)
		{
			this._s = stream;
			try
			{
				this._initialOffset = this._s.Position;
			}
			catch
			{
				this._initialOffset = 0L;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000A78A File Offset: 0x0000898A
		public Stream WrappedStream
		{
			get
			{
				return this._s;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000A792 File Offset: 0x00008992
		public long BytesWritten
		{
			get
			{
				return this._bytesWritten;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000A79A File Offset: 0x0000899A
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0002F0A0 File Offset: 0x0002D2A0
		public void Adjust(long delta)
		{
			this._bytesWritten -= delta;
			if (this._bytesWritten < 0L)
			{
				throw new InvalidOperationException();
			}
			if (this._s is CountingStream)
			{
				((CountingStream)this._s).Adjust(delta);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0002F0F0 File Offset: 0x0002D2F0
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._s.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			return num;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000A7A2 File Offset: 0x000089A2
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			this._s.Write(buffer, offset, count);
			this._bytesWritten += (long)count;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000A7C5 File Offset: 0x000089C5
		public override bool CanRead
		{
			get
			{
				return this._s.CanRead;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000A7D2 File Offset: 0x000089D2
		public override bool CanSeek
		{
			get
			{
				return this._s.CanSeek;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000A7DF File Offset: 0x000089DF
		public override bool CanWrite
		{
			get
			{
				return this._s.CanWrite;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000A7EC File Offset: 0x000089EC
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000A7F9 File Offset: 0x000089F9
		public override long Length
		{
			get
			{
				return this._s.Length;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000A806 File Offset: 0x00008A06
		public long ComputedPosition
		{
			get
			{
				return this._initialOffset + this._bytesWritten;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000A815 File Offset: 0x00008A15
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000A822 File Offset: 0x00008A22
		public override long Position
		{
			get
			{
				return this._s.Position;
			}
			set
			{
				this._s.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000A832 File Offset: 0x00008A32
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._s.Seek(offset, origin);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000A841 File Offset: 0x00008A41
		public override void SetLength(long value)
		{
			this._s.SetLength(value);
		}

		// Token: 0x040002B3 RID: 691
		private Stream _s;

		// Token: 0x040002B4 RID: 692
		private long _bytesWritten;

		// Token: 0x040002B5 RID: 693
		private long _bytesRead;

		// Token: 0x040002B6 RID: 694
		private long _initialOffset;
	}
}
