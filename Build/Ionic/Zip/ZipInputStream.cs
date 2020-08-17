using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x020000FA RID: 250
	[ComVisible(true)]
	public class ZipInputStream : Stream
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x0000B947 File Offset: 0x00009B47
		public ZipInputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00039E44 File Offset: 0x00038044
		public ZipInputStream(string fileName)
		{
			Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0000B951 File Offset: 0x00009B51
		public ZipInputStream(Stream stream, bool leaveOpen)
		{
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00039E6C File Offset: 0x0003806C
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._inputStream = stream;
			if (!this._inputStream.CanRead)
			{
				throw new ZipException("The stream must be readable.");
			}
			this._container = new ZipContainer(this);
			this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this._findRequired = true;
			this._name = (name ?? "(stream)");
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0000B962 File Offset: 0x00009B62
		public override string ToString()
		{
			return string.Format("ZipInputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0000B97F File Offset: 0x00009B7F
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x0000B987 File Offset: 0x00009B87
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				return this._provisionalAlternateEncoding;
			}
			set
			{
				this._provisionalAlternateEncoding = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0000B990 File Offset: 0x00009B90
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x0000B998 File Offset: 0x00009B98
		public int CodecBufferSize { get; set; }

		// Token: 0x170000F5 RID: 245
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0000B9A1 File Offset: 0x00009BA1
		public string Password
		{
			set
			{
				if (this._closed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._Password = value;
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		private void SetupStream()
		{
			this._crcStream = this._currentEntry.InternalOpenReader(this._Password);
			this._LeftToRead = this._crcStream.Length;
			this._needSetup = false;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0000B9F5 File Offset: 0x00009BF5
		internal Stream ReadStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00039ED4 File Offset: 0x000380D4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._closed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (this._needSetup)
			{
				this.SetupStream();
			}
			if (this._LeftToRead == 0L)
			{
				return 0;
			}
			int count2 = (this._LeftToRead > (long)count) ? count : ((int)this._LeftToRead);
			int num = this._crcStream.Read(buffer, offset, count2);
			this._LeftToRead -= (long)num;
			if (this._LeftToRead == 0L)
			{
				int crc = this._crcStream.Crc;
				this._currentEntry.VerifyCrcAfterExtract(crc);
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			return num;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00039F8C File Offset: 0x0003818C
		public ZipEntry GetNextEntry()
		{
			if (this._findRequired)
			{
				long num = SharedUtilities.FindSignature(this._inputStream, 67324752);
				if (num == -1L)
				{
					return null;
				}
				this._inputStream.Seek(-4L, SeekOrigin.Current);
			}
			else if (this._firstEntry)
			{
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			this._currentEntry = ZipEntry.ReadEntry(this._container, !this._firstEntry);
			this._endOfEntry = this._inputStream.Position;
			this._firstEntry = true;
			this._needSetup = true;
			this._findRequired = false;
			return this._currentEntry;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0000B9FD File Offset: 0x00009BFD
		protected override void Dispose(bool disposing)
		{
			if (this._closed)
			{
				return;
			}
			if (disposing)
			{
				if (this._exceptionPending)
				{
					return;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					this._inputStream.Dispose();
				}
			}
			this._closed = true;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00009F16 File Offset: 0x00008116
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0000BA2E File Offset: 0x00009C2E
		public override bool CanSeek
		{
			get
			{
				return this._inputStream.CanSeek;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0000BA3B File Offset: 0x00009C3B
		public override long Length
		{
			get
			{
				return this._inputStream.Length;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x0000BA48 File Offset: 0x00009C48
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x0000BA55 File Offset: 0x00009C55
		public override long Position
		{
			get
			{
				return this._inputStream.Position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0000BA60 File Offset: 0x00009C60
		public override void Flush()
		{
			throw new NotSupportedException("Flush");
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Write");
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0003A03C File Offset: 0x0003823C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this._findRequired = true;
			return this._inputStream.Seek(offset, origin);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040003C8 RID: 968
		private Stream _inputStream;

		// Token: 0x040003C9 RID: 969
		private Encoding _provisionalAlternateEncoding;

		// Token: 0x040003CA RID: 970
		private ZipEntry _currentEntry;

		// Token: 0x040003CB RID: 971
		private bool _firstEntry;

		// Token: 0x040003CC RID: 972
		private bool _needSetup;

		// Token: 0x040003CD RID: 973
		private ZipContainer _container;

		// Token: 0x040003CE RID: 974
		private CrcCalculatorStream _crcStream;

		// Token: 0x040003CF RID: 975
		private long _LeftToRead;

		// Token: 0x040003D0 RID: 976
		internal string _Password;

		// Token: 0x040003D1 RID: 977
		private long _endOfEntry;

		// Token: 0x040003D2 RID: 978
		private string _name;

		// Token: 0x040003D3 RID: 979
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x040003D4 RID: 980
		private bool _closed;

		// Token: 0x040003D5 RID: 981
		private bool _findRequired;

		// Token: 0x040003D6 RID: 982
		private bool _exceptionPending;
	}
}
