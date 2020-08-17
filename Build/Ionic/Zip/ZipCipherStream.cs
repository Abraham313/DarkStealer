using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x020000E5 RID: 229
	internal class ZipCipherStream : Stream
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x0000A93E File Offset: 0x00008B3E
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			this._cipher = cipher;
			this._s = s;
			this._mode = mode;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0002FD0C File Offset: 0x0002DF0C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = this._s.Read(array, 0, count);
			byte[] array2 = this._cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0002FD74 File Offset: 0x0002DF74
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = this._cipher.EncryptMessage(array, count);
			this._s.Write(array2, 0, array2.Length);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000A95B File Offset: 0x00008B5B
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000A966 File Offset: 0x00008B66
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00009B58 File Offset: 0x00007D58
		public override void Flush()
		{
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000983A File Offset: 0x00007A3A
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040002E7 RID: 743
		private ZipCrypto _cipher;

		// Token: 0x040002E8 RID: 744
		private Stream _s;

		// Token: 0x040002E9 RID: 745
		private CryptoMode _mode;
	}
}
