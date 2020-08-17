using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02000128 RID: 296
	internal class ZlibBaseStream : Stream
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0000CB01 File Offset: 0x0000AD01
		internal int Crc32
		{
			get
			{
				if (this.crc == null)
				{
					return 0;
				}
				return this.crc.Crc32Result;
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000460A0 File Offset: 0x000442A0
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen)
		{
			this._flushMode = FlushType.None;
			this._stream = stream;
			this._leaveOpen = leaveOpen;
			this._compressionMode = compressionMode;
			this._flavor = flavor;
			this._level = level;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				this.crc = new CRC32();
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0000CB18 File Offset: 0x0000AD18
		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00046114 File Offset: 0x00044314
		private ZlibCodec z
		{
			get
			{
				if (this._z == null)
				{
					bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
					this._z = new ZlibCodec();
					if (this._compressionMode == CompressionMode.Decompress)
					{
						this._z.InitializeInflate(flag);
					}
					else
					{
						this._z.Strategy = this.Strategy;
						this._z.InitializeDeflate(this._level, flag);
					}
				}
				return this._z;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0000CB23 File Offset: 0x0000AD23
		private byte[] workingBuffer
		{
			get
			{
				if (this._workingBuffer == null)
				{
					this._workingBuffer = new byte[this._bufferSize];
				}
				return this._workingBuffer;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00046184 File Offset: 0x00044384
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				this._streamMode = ZlibBaseStream.StreamMode.Writer;
			}
			else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			this.z.InputBuffer = buffer;
			this._z.NextIn = offset;
			this._z.AvailableBytesIn = count;
			for (;;)
			{
				this._z.OutputBuffer = this.workingBuffer;
				this._z.NextOut = 0;
				this._z.AvailableBytesOut = this._workingBuffer.Length;
				int num = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
				if (num != 0 && num != 1)
				{
					break;
				}
				this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
				bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
				if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
				{
					flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
				}
				if (flag)
				{
					return;
				}
			}
			throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00046324 File Offset: 0x00044524
		private void finish()
		{
			if (this._z == null)
			{
				return;
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				bool flag;
				do
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					int num = this._wantCompress ? this._z.Deflate(FlushType.Finish) : this._z.Inflate(FlushType.Finish);
					if (num != 1 && num != 0)
					{
						goto IL_11F;
					}
					if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
					{
						this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					}
					flag = (this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0);
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
				}
				while (!flag);
				this.Flush();
				if (this._flavor != ZlibStreamFlavor.GZIP)
				{
					return;
				}
				if (this._wantCompress)
				{
					int crc32Result = this.crc.Crc32Result;
					this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
					int value = (int)(this.crc.TotalBytesRead & 4294967295L);
					this._stream.Write(BitConverter.GetBytes(value), 0, 4);
					return;
				}
				throw new ZlibException("Writing with decompression is not supported.");
				IL_11F:
				string text = (this._wantCompress ? "de" : "in") + "flating";
				if (this._z.Message == null)
				{
					int num;
					throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
				}
				throw new ZlibException(text + ": " + this._z.Message);
			}
			else if (this._streamMode == ZlibBaseStream.StreamMode.Reader && this._flavor == ZlibStreamFlavor.GZIP)
			{
				if (this._wantCompress)
				{
					throw new ZlibException("Reading with compression is not supported.");
				}
				if (this._z.TotalBytesOut == 0L)
				{
					return;
				}
				byte[] array = new byte[8];
				if (this._z.AvailableBytesIn < 8)
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, this._z.AvailableBytesIn);
					int num2 = 8 - this._z.AvailableBytesIn;
					int num3 = this._stream.Read(array, this._z.AvailableBytesIn, num2);
					if (num2 != num3)
					{
						throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + num3));
					}
				}
				else
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, array.Length);
				}
				int num4 = BitConverter.ToInt32(array, 0);
				int crc32Result2 = this.crc.Crc32Result;
				int num5 = BitConverter.ToInt32(array, 4);
				int num6 = (int)(this._z.TotalBytesOut & 4294967295L);
				if (crc32Result2 != num4)
				{
					throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
				}
				if (num6 != num5)
				{
					throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
				}
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0000CB44 File Offset: 0x0000AD44
		private void end()
		{
			if (this.z == null)
			{
				return;
			}
			if (this._wantCompress)
			{
				this._z.EndDeflate();
			}
			else
			{
				this._z.EndInflate();
			}
			this._z = null;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000466A4 File Offset: 0x000448A4
		public override void Close()
		{
			if (this._stream == null)
			{
				return;
			}
			try
			{
				this.finish();
			}
			finally
			{
				this.end();
				if (!this._leaveOpen)
				{
					this._stream.Close();
				}
				this._stream = null;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0000CB78 File Offset: 0x0000AD78
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0000CB85 File Offset: 0x0000AD85
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000466F4 File Offset: 0x000448F4
		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			for (;;)
			{
				int num = this._stream.Read(this._buf1, 0, 1);
				if (num != 1)
				{
					break;
				}
				if (this._buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this._buf1[0]);
				}
				if (flag)
				{
					goto IL_4C;
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
			IL_4C:
			byte[] array = list.ToArray();
			return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00046764 File Offset: 0x00044964
		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this._stream.Read(array, 0, array.Length);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 != 10)
			{
				throw new ZlibException("Not a valid GZIP stream.");
			}
			if (array[0] == 31 && array[1] == 139)
			{
				if (array[2] == 8)
				{
					int num3 = BitConverter.ToInt32(array, 4);
					this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
					num += num2;
					if ((array[3] & 4) == 4)
					{
						num2 = this._stream.Read(array, 0, 2);
						num += num2;
						short num4 = (short)((int)array[0] + (int)array[1] * 256);
						byte[] array2 = new byte[(int)num4];
						num2 = this._stream.Read(array2, 0, array2.Length);
						if (num2 != (int)num4)
						{
							throw new ZlibException("Unexpected end-of-file reading GZIP header.");
						}
						num += num2;
					}
					if ((array[3] & 8) == 8)
					{
						this._GzipFileName = this.ReadZeroTerminatedString();
					}
					if ((array[3] & 16) == 16)
					{
						this._GzipComment = this.ReadZeroTerminatedString();
					}
					if ((array[3] & 2) == 2)
					{
						this.Read(this._buf1, 0, 1);
					}
					return num;
				}
			}
			throw new ZlibException("Bad GZIP header.");
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00046890 File Offset: 0x00044A90
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = ZlibBaseStream.StreamMode.Reader;
				this.z.AvailableBytesIn = 0;
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
					if (this._gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.nomoreinput && this._wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < buffer.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._z.OutputBuffer = buffer;
			this._z.NextOut = offset;
			this._z.AvailableBytesOut = count;
			this._z.InputBuffer = this.workingBuffer;
			int num;
			for (;;)
			{
				if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
				{
					this._z.NextIn = 0;
					this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
					if (this._z.AvailableBytesIn == 0)
					{
						this.nomoreinput = true;
					}
				}
				num = (this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode));
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_17;
				}
				if (((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count) || this._z.AvailableBytesOut <= 0 || this.nomoreinput)
				{
					goto IL_234;
				}
				if (num != 0)
				{
					goto Block_21;
				}
			}
			return 0;
			Block_17:
			throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", num, this._z.Message));
			Block_21:
			IL_234:
			if (this._z.AvailableBytesOut > 0)
			{
				if (num != 0)
				{
				}
				if (this.nomoreinput && this._wantCompress)
				{
					num = this._z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
					}
				}
			}
			num = count - this._z.AvailableBytesOut;
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0000CB93 File Offset: 0x0000AD93
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0000CBAD File Offset: 0x0000ADAD
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0000CBBA File Offset: 0x0000ADBA
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0000A639 File Offset: 0x00008839
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0000A639 File Offset: 0x00008839
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

		// Token: 0x060007FE RID: 2046 RVA: 0x00046B4C File Offset: 0x00044D4C
		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00046B90 File Offset: 0x00044D90
		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			try
			{
				compressor.Write(b, 0, b.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00046BC8 File Offset: 0x00044DC8
		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			Encoding utf = Encoding.UTF8;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(memoryStream, utf);
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00046C64 File Offset: 0x00044E64
		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x04000601 RID: 1537
		protected internal ZlibCodec _z;

		// Token: 0x04000602 RID: 1538
		protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;

		// Token: 0x04000603 RID: 1539
		protected internal FlushType _flushMode;

		// Token: 0x04000604 RID: 1540
		protected internal ZlibStreamFlavor _flavor;

		// Token: 0x04000605 RID: 1541
		protected internal CompressionMode _compressionMode;

		// Token: 0x04000606 RID: 1542
		protected internal CompressionLevel _level;

		// Token: 0x04000607 RID: 1543
		protected internal bool _leaveOpen;

		// Token: 0x04000608 RID: 1544
		protected internal byte[] _workingBuffer;

		// Token: 0x04000609 RID: 1545
		protected internal int _bufferSize = 16384;

		// Token: 0x0400060A RID: 1546
		protected internal byte[] _buf1 = new byte[1];

		// Token: 0x0400060B RID: 1547
		protected internal Stream _stream;

		// Token: 0x0400060C RID: 1548
		protected internal CompressionStrategy Strategy;

		// Token: 0x0400060D RID: 1549
		private CRC32 crc;

		// Token: 0x0400060E RID: 1550
		protected internal string _GzipFileName;

		// Token: 0x0400060F RID: 1551
		protected internal string _GzipComment;

		// Token: 0x04000610 RID: 1552
		protected internal DateTime _GzipMtime;

		// Token: 0x04000611 RID: 1553
		protected internal int _gzipHeaderByteCount;

		// Token: 0x04000612 RID: 1554
		private bool nomoreinput;

		// Token: 0x02000129 RID: 297
		internal enum StreamMode
		{
			// Token: 0x04000614 RID: 1556
			Writer,
			// Token: 0x04000615 RID: 1557
			Reader,
			// Token: 0x04000616 RID: 1558
			Undefined
		}
	}
}
