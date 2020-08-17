using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x020000DD RID: 221
	internal class OffsetStream : Stream, IDisposable
	{
		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A60E File Offset: 0x0000880E
		public OffsetStream(Stream s)
		{
			this._originalPosition = s.Position;
			this._innerStream = s;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000A629 File Offset: 0x00008829
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000A639 File Offset: 0x00008839
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000A640 File Offset: 0x00008840
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000A64D File Offset: 0x0000884D
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000A65A File Offset: 0x0000885A
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000A667 File Offset: 0x00008867
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000A674 File Offset: 0x00008874
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0000A688 File Offset: 0x00008888
		public override long Position
		{
			get
			{
				return this._innerStream.Position - this._originalPosition;
			}
			set
			{
				this._innerStream.Position = this._originalPosition + value;
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000A69D File Offset: 0x0000889D
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000A6BA File Offset: 0x000088BA
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000A6C2 File Offset: 0x000088C2
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x040002AE RID: 686
		private long _originalPosition;

		// Token: 0x040002AF RID: 687
		private Stream _innerStream;
	}
}
