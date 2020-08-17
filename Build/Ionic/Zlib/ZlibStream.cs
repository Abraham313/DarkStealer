using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200012C RID: 300
	[ComVisible(true)]
	public class ZlibStream : Stream
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x0000CD97 File Offset: 0x0000AF97
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0000CDA3 File Offset: 0x0000AFA3
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0000CDAF File Offset: 0x0000AFAF
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0000CDBB File Offset: 0x0000AFBB
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0000CDE5 File Offset: 0x0000AFE5
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
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0000CE06 File Offset: 0x0000B006
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x00046FB0 File Offset: 0x000451B0
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
					throw new ObjectDisposedException("ZlibStream");
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0000CE13 File Offset: 0x0000B013
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0000CE25 File Offset: 0x0000B025
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0004701C File Offset: 0x0004521C
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0000CE37 File Offset: 0x0000B037
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0000CE81 File Offset: 0x0000B081
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00047068 File Offset: 0x00045268
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0000983A File Offset: 0x00007A3A
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
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000CEA1 File Offset: 0x0000B0A1
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0000CEC4 File Offset: 0x0000B0C4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000470BC File Offset: 0x000452BC
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00047104 File Offset: 0x00045304
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0004714C File Offset: 0x0004534C
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00047190 File Offset: 0x00045390
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000630 RID: 1584
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000631 RID: 1585
		private bool _disposed;
	}
}
