using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Ionic.Zip
{
	// Token: 0x020000E1 RID: 225
	internal class WinZipAesCipherStream : Stream
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x0000A8DB File Offset: 0x00008ADB
		internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, long length, CryptoMode mode) : this(s, cryptoParams, mode)
		{
			this._length = length;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0002F348 File Offset: 0x0002D548
		internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, CryptoMode mode)
		{
			this._params = cryptoParams;
			this._s = s;
			this._mode = mode;
			this._nonce = 1;
			if (this._params == null)
			{
				throw new BadPasswordException("Supply a password to use AES encryption.");
			}
			int num = this._params.KeyBytes.Length * 8;
			if (num != 256 && num != 128 && num != 192)
			{
				throw new ArgumentOutOfRangeException("keysize", "size of key must be 128, 192, or 256");
			}
			this._mac = new HMACSHA1(this._params.MacIv);
			this._aesCipher = new RijndaelManaged();
			this._aesCipher.BlockSize = 128;
			this._aesCipher.KeySize = num;
			this._aesCipher.Mode = CipherMode.ECB;
			this._aesCipher.Padding = PaddingMode.None;
			byte[] rgbIV = new byte[16];
			this._xform = this._aesCipher.CreateEncryptor(this._params.KeyBytes, rgbIV);
			if (this._mode == CryptoMode.Encrypt)
			{
				this._iobuf = new byte[2048];
				this._PendingWriteBlock = new byte[16];
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0002F488 File Offset: 0x0002D688
		private void XorInPlace(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				buffer[offset + i] = (this.counterOut[i] ^ buffer[offset + i]);
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0002F4B8 File Offset: 0x0002D6B8
		private void WriteTransformOneBlock(byte[] buffer, int offset)
		{
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			this._xform.TransformBlock(this.counter, 0, 16, this.counterOut, 0);
			this.XorInPlace(buffer, offset, 16);
			this._mac.TransformBlock(buffer, offset, 16, null, 0);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0002F524 File Offset: 0x0002D724
		private void WriteTransformBlocks(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int num2 = count + offset;
			while (num < buffer.Length && num < num2)
			{
				this.WriteTransformOneBlock(buffer, num);
				num += 16;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0002F550 File Offset: 0x0002D750
		private void WriteTransformFinalBlock()
		{
			if (this._pendingCount == 0)
			{
				throw new InvalidOperationException("No bytes available.");
			}
			if (this._finalBlock)
			{
				throw new InvalidOperationException("The final block has already been transformed.");
			}
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16);
			this.XorInPlace(this._PendingWriteBlock, 0, this._pendingCount);
			this._mac.TransformFinalBlock(this._PendingWriteBlock, 0, this._pendingCount);
			this._finalBlock = true;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0002F5F4 File Offset: 0x0002D7F4
		private int ReadTransformOneBlock(byte[] buffer, int offset, int last)
		{
			if (this._finalBlock)
			{
				throw new NotSupportedException();
			}
			int num = last - offset;
			int num2 = (num > 16) ? 16 : num;
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			if (num2 == num && this._length > 0L && this._totalBytesXferred + (long)last == this._length)
			{
				this._mac.TransformFinalBlock(buffer, offset, num2);
				this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16);
				this._finalBlock = true;
			}
			else
			{
				this._mac.TransformBlock(buffer, offset, num2, null, 0);
				this._xform.TransformBlock(this.counter, 0, 16, this.counterOut, 0);
			}
			this.XorInPlace(buffer, offset, num2);
			return num2;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0002F6D4 File Offset: 0x0002D8D4
		private void ReadTransformBlocks(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int num2 = count + offset;
			while (num < buffer.Length && num < num2)
			{
				int num3 = this.ReadTransformOneBlock(buffer, num, num2);
				num += num3;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0002F704 File Offset: 0x0002D904
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must not be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must not be less than zero.");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("The buffer is too small");
			}
			int count2 = count;
			if (this._totalBytesXferred >= this._length)
			{
				return 0;
			}
			long num = this._length - this._totalBytesXferred;
			if (num < (long)count)
			{
				count2 = (int)num;
			}
			int num2 = this._s.Read(buffer, offset, count2);
			this.ReadTransformBlocks(buffer, offset, count2);
			this._totalBytesXferred += (long)num2;
			return num2;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0002F7B8 File Offset: 0x0002D9B8
		public byte[] FinalAuthentication
		{
			get
			{
				if (!this._finalBlock)
				{
					if (this._totalBytesXferred != 0L)
					{
						throw new BadStateException("The final hash has not been computed.");
					}
					byte[] buffer = new byte[0];
					this._mac.ComputeHash(buffer);
				}
				byte[] array = new byte[10];
				Array.Copy(this._mac.Hash, 0, array, 0, 10);
				return array;
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0002F81C File Offset: 0x0002DA1C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._finalBlock)
			{
				throw new InvalidOperationException("The final block has already been transformed.");
			}
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must not be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must not be less than zero.");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("The offset and count are too large");
			}
			if (count == 0)
			{
				return;
			}
			if (count + this._pendingCount <= 16)
			{
				Buffer.BlockCopy(buffer, offset, this._PendingWriteBlock, this._pendingCount, count);
				this._pendingCount += count;
				return;
			}
			int num = count;
			int num2 = offset;
			if (this._pendingCount != 0)
			{
				int num3 = 16 - this._pendingCount;
				if (num3 > 0)
				{
					Buffer.BlockCopy(buffer, offset, this._PendingWriteBlock, this._pendingCount, num3);
					num -= num3;
					num2 += num3;
				}
				this.WriteTransformOneBlock(this._PendingWriteBlock, 0);
				this._s.Write(this._PendingWriteBlock, 0, 16);
				this._totalBytesXferred += 16L;
				this._pendingCount = 0;
			}
			int num4 = (num - 1) / 16;
			this._pendingCount = num - num4 * 16;
			Buffer.BlockCopy(buffer, num2 + num - this._pendingCount, this._PendingWriteBlock, 0, this._pendingCount);
			num -= this._pendingCount;
			this._totalBytesXferred += (long)num;
			if (num4 > 0)
			{
				do
				{
					int num5 = this._iobuf.Length;
					if (num5 > num)
					{
						num5 = num;
					}
					Buffer.BlockCopy(buffer, num2, this._iobuf, 0, num5);
					this.WriteTransformBlocks(this._iobuf, 0, num5);
					this._s.Write(this._iobuf, 0, num5);
					num -= num5;
					num2 += num5;
				}
				while (num > 0);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0002F9E4 File Offset: 0x0002DBE4
		public override void Close()
		{
			if (this._pendingCount > 0)
			{
				this.WriteTransformFinalBlock();
				this._s.Write(this._PendingWriteBlock, 0, this._pendingCount);
				this._totalBytesXferred += (long)this._pendingCount;
				this._pendingCount = 0;
			}
			this._s.Close();
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000A8EE File Offset: 0x00008AEE
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000A8FC File Offset: 0x00008AFC
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000A907 File Offset: 0x00008B07
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000A639 File Offset: 0x00008839
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000A639 File Offset: 0x00008839
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

		// Token: 0x06000444 RID: 1092 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0002FA40 File Offset: 0x0002DC40
		[Conditional("Trace")]
		private void TraceOutput(string format, params object[] varParams)
		{
			lock (this._outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
				Console.Write("{0:000} WZACS ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x040002C4 RID: 708
		private const int BLOCK_SIZE_IN_BYTES = 16;

		// Token: 0x040002C5 RID: 709
		private WinZipAesCrypto _params;

		// Token: 0x040002C6 RID: 710
		private Stream _s;

		// Token: 0x040002C7 RID: 711
		private CryptoMode _mode;

		// Token: 0x040002C8 RID: 712
		private int _nonce;

		// Token: 0x040002C9 RID: 713
		private bool _finalBlock;

		// Token: 0x040002CA RID: 714
		internal HMACSHA1 _mac;

		// Token: 0x040002CB RID: 715
		internal RijndaelManaged _aesCipher;

		// Token: 0x040002CC RID: 716
		internal ICryptoTransform _xform;

		// Token: 0x040002CD RID: 717
		private byte[] counter = new byte[16];

		// Token: 0x040002CE RID: 718
		private byte[] counterOut = new byte[16];

		// Token: 0x040002CF RID: 719
		private long _length;

		// Token: 0x040002D0 RID: 720
		private long _totalBytesXferred;

		// Token: 0x040002D1 RID: 721
		private byte[] _PendingWriteBlock;

		// Token: 0x040002D2 RID: 722
		private int _pendingCount;

		// Token: 0x040002D3 RID: 723
		private byte[] _iobuf;

		// Token: 0x040002D4 RID: 724
		private object _outputLock = new object();
	}
}
