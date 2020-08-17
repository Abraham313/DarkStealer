using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000111 RID: 273
	[ComVisible(true)]
	public class DeflateStream : Stream
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x0000C58A File Offset: 0x0000A78A
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000C596 File Offset: 0x0000A796
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000C5A2 File Offset: 0x0000A7A2
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000C5AE File Offset: 0x0000A7AE
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000C5D2 File Offset: 0x0000A7D2
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0000C5DF File Offset: 0x0000A7DF
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0000C600 File Offset: 0x0000A800
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x000413D4 File Offset: 0x0003F5D4
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0000C60D File Offset: 0x0000A80D
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0000C61A File Offset: 0x0000A81A
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000C63B File Offset: 0x0000A83B
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0000C64D File Offset: 0x0000A84D
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00041440 File Offset: 0x0003F640
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0000C65F File Offset: 0x0000A85F
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0000C684 File Offset: 0x0000A884
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000C6A9 File Offset: 0x0000A8A9
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0004148C File Offset: 0x0003F68C
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000C6C9 File Offset: 0x0000A8C9
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000414E0 File Offset: 0x0003F6E0
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00041528 File Offset: 0x0003F728
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00041570 File Offset: 0x0003F770
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000415B4 File Offset: 0x0003F7B4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400050C RID: 1292
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400050D RID: 1293
		internal Stream _innerStream;

		// Token: 0x0400050E RID: 1294
		private bool _disposed;
	}
}
