using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000112 RID: 274
	[ComVisible(true)]
	public class GZipStream : Stream
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0000C70F File Offset: 0x0000A90F
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x0000C717 File Offset: 0x0000A917
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0000C733 File Offset: 0x0000A933
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x000415F8 File Offset: 0x0003F7F8
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0000C73B File Offset: 0x0000A93B
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000C743 File Offset: 0x0000A943
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000C74F File Offset: 0x0000A94F
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000C75B File Offset: 0x0000A95B
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000C767 File Offset: 0x0000A967
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0000C784 File Offset: 0x0000A984
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x0000C791 File Offset: 0x0000A991
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0000C7B2 File Offset: 0x0000A9B2
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00041698 File Offset: 0x0003F898
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0000C7BF File Offset: 0x0000A9BF
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0000C7D1 File Offset: 0x0000A9D1
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00041704 File Offset: 0x0003F904
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0000C7E3 File Offset: 0x0000A9E3
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0000C808 File Offset: 0x0000AA08
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000C82D File Offset: 0x0000AA2D
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00041764 File Offset: 0x0003F964
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000417CC File Offset: 0x0003F9CC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00041830 File Offset: 0x0003FA30
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00041890 File Offset: 0x0003FA90
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			array3[0] = 31;
			array3[1] = 139;
			byte[] array4 = array3;
			int num4 = 2;
			int num5 = 3;
			array4[num4] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num5++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			int value = (int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(value), 0, array3, num5, 4);
			num5 += 4;
			array3[num5++] = 0;
			array3[num5++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num5, num2 - 1);
				num5 += num2 - 1;
				array3[num5++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num5, num - 1);
				num5 += num - 1;
				array3[num5++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00041A2C File Offset: 0x0003FC2C
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00041A74 File Offset: 0x0003FC74
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00041ABC File Offset: 0x0003FCBC
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00041B00 File Offset: 0x0003FD00
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400050F RID: 1295
		public DateTime? LastModified;

		// Token: 0x04000510 RID: 1296
		private int _headerByteCount;

		// Token: 0x04000511 RID: 1297
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000512 RID: 1298
		private bool _disposed;

		// Token: 0x04000513 RID: 1299
		private bool _firstReadDone;

		// Token: 0x04000514 RID: 1300
		private string _FileName;

		// Token: 0x04000515 RID: 1301
		private string _Comment;

		// Token: 0x04000516 RID: 1302
		private int _Crc32;

		// Token: 0x04000517 RID: 1303
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000518 RID: 1304
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
