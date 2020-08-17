using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200016C RID: 364
	internal abstract class MegaAesCtrStream : Stream
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x0004D038 File Offset: 0x0004B238
		protected MegaAesCtrStream(Stream stream, long streamLength, MegaAesCtrStream.Mode mode, byte[] fileKey, byte[] iv)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (fileKey != null)
			{
				if (fileKey.Length == 16)
				{
					if (iv == null || iv.Length != 8)
					{
						throw new ArgumentException("Invalid Iv");
					}
					this.stream = stream;
					this.streamLength = streamLength;
					this.mode = mode;
					this.fileKey = fileKey;
					this.iv = iv;
					this.ChunksPositions = this.GetChunksPositions(this.streamLength).ToArray<long>();
					this.chunksPositionsCache = new HashSet<long>(this.ChunksPositions);
					this.encryptor = Crypto.CreateAesEncryptor(this.fileKey);
					return;
				}
			}
			throw new ArgumentException("Invalid fileKey");
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0000DCCA File Offset: 0x0000BECA
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.encryptor.Dispose();
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0000DCDE File Offset: 0x0000BEDE
		public long[] ChunksPositions { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00009F16 File Offset: 0x00008116
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0000DCE6 File Offset: 0x0000BEE6
		public override long Length
		{
			get
			{
				return this.streamLength;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0000DCEE File Offset: 0x0000BEEE
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0000DCF6 File Offset: 0x0000BEF6
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (this.position != value)
				{
					throw new NotSupportedException("Seek is not supported");
				}
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0004D118 File Offset: 0x0004B318
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.position == this.streamLength)
			{
				return 0;
			}
			for (long num = this.position; num < Math.Min(this.position + (long)count, this.streamLength); num += 16L)
			{
				if (this.chunksPositionsCache.Contains(num))
				{
					if (num != 0L)
					{
						this.ComputeChunk(this.encryptor);
					}
					for (int i = 0; i < 8; i++)
					{
						this.currentChunkMac[i] = this.iv[i];
						this.currentChunkMac[i + 8] = this.iv[i];
					}
				}
				this.IncrementCounter();
				byte[] array = new byte[16];
				byte[] array2 = new byte[array.Length];
				int num2 = this.stream.Read(array, 0, array.Length);
				if (num2 != array.Length)
				{
					num2 += this.stream.Read(array, num2, array.Length - num2);
				}
				byte[] array3 = new byte[16];
				Array.Copy(this.iv, array3, 8);
				Array.Copy(this.counter, 0, array3, 8, 8);
				byte[] array4 = Crypto.EncryptAes(array3, this.encryptor);
				for (int j = 0; j < num2; j++)
				{
					array2[j] = (array4[j] ^ array[j]);
					byte[] array5 = this.currentChunkMac;
					int num3 = j;
					array5[num3] ^= ((this.mode == MegaAesCtrStream.Mode.Crypt) ? array[j] : array2[j]);
				}
				Array.Copy(array2, 0, buffer, (int)((long)offset + num - this.position), (int)Math.Min((long)array2.Length, this.streamLength - num));
				this.currentChunkMac = Crypto.EncryptAes(this.currentChunkMac, this.encryptor);
			}
			long num4 = Math.Min((long)count, this.streamLength - this.position);
			this.position += num4;
			if (this.position == this.streamLength)
			{
				this.ComputeChunk(this.encryptor);
				for (int k = 0; k < 4; k++)
				{
					this.metaMac[k] = (this.fileMac[k] ^ this.fileMac[k + 4]);
					this.metaMac[k + 4] = (this.fileMac[k + 8] ^ this.fileMac[k + 12]);
				}
				this.OnStreamRead();
			}
			return (int)num4;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0000983A File Offset: 0x00007A3A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000983A File Offset: 0x00007A3A
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00009B58 File Offset: 0x00007D58
		protected virtual void OnStreamRead()
		{
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0004D350 File Offset: 0x0004B550
		private void IncrementCounter()
		{
			if ((this.currentCounter & 255L) != 255L && (this.currentCounter & 255L) != 0L)
			{
				byte[] array = this.counter;
				int num = 7;
				array[num] += 1;
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(this.currentCounter);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes);
				}
				Array.Copy(bytes, this.counter, 8);
			}
			this.currentCounter += 1L;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0004D3DC File Offset: 0x0004B5DC
		private void ComputeChunk(ICryptoTransform encryptor)
		{
			for (int i = 0; i < 16; i++)
			{
				byte[] array = this.fileMac;
				int num = i;
				array[num] ^= this.currentChunkMac[i];
			}
			this.fileMac = Crypto.EncryptAes(this.fileMac, encryptor);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0000DD0C File Offset: 0x0000BF0C
		private IEnumerable<long> GetChunksPositions(long size)
		{
			MegaAesCtrStream.<GetChunksPositions>d__38 <GetChunksPositions>d__ = new MegaAesCtrStream.<GetChunksPositions>d__38(-2);
			<GetChunksPositions>d__.<>3__size = size;
			return <GetChunksPositions>d__;
		}

		// Token: 0x040006ED RID: 1773
		protected readonly byte[] fileKey;

		// Token: 0x040006EE RID: 1774
		protected readonly byte[] iv;

		// Token: 0x040006EF RID: 1775
		protected readonly long streamLength;

		// Token: 0x040006F0 RID: 1776
		protected long position;

		// Token: 0x040006F1 RID: 1777
		protected byte[] metaMac = new byte[8];

		// Token: 0x040006F2 RID: 1778
		private readonly Stream stream;

		// Token: 0x040006F3 RID: 1779
		private readonly MegaAesCtrStream.Mode mode;

		// Token: 0x040006F4 RID: 1780
		private readonly HashSet<long> chunksPositionsCache;

		// Token: 0x040006F5 RID: 1781
		private readonly byte[] counter = new byte[8];

		// Token: 0x040006F6 RID: 1782
		private readonly ICryptoTransform encryptor;

		// Token: 0x040006F7 RID: 1783
		private long currentCounter;

		// Token: 0x040006F8 RID: 1784
		private byte[] currentChunkMac = new byte[16];

		// Token: 0x040006F9 RID: 1785
		private byte[] fileMac = new byte[16];

		// Token: 0x0200016D RID: 365
		protected enum Mode
		{
			// Token: 0x040006FC RID: 1788
			Crypt,
			// Token: 0x040006FD RID: 1789
			Decrypt
		}
	}
}
