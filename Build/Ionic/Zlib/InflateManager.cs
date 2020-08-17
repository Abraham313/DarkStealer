using System;

namespace Ionic.Zlib
{
	// Token: 0x02000117 RID: 279
	internal sealed class InflateManager
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0000C909 File Offset: 0x0000AB09
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x0000C911 File Offset: 0x0000AB11
		internal bool HandleRfc1950HeaderBytes
		{
			get
			{
				return this._handleRfc1950HeaderBytes;
			}
			set
			{
				this._handleRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0000C91A File Offset: 0x0000AB1A
		public InflateManager()
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000C929 File Offset: 0x0000AB29
		public InflateManager(bool expectRfc1950HeaderBytes)
		{
			this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00043DCC File Offset: 0x00041FCC
		internal int Reset()
		{
			ZlibCodec codec = this._codec;
			this._codec.TotalBytesOut = 0L;
			codec.TotalBytesIn = 0L;
			this._codec.Message = null;
			this.mode = (this.HandleRfc1950HeaderBytes ? InflateManager.InflateManagerMode.METHOD : InflateManager.InflateManagerMode.BLOCKS);
			this.blocks.Reset();
			return 0;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0000C93F File Offset: 0x0000AB3F
		internal int End()
		{
			if (this.blocks != null)
			{
				this.blocks.Free();
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00043E2C File Offset: 0x0004202C
		internal int Initialize(ZlibCodec codec, int w)
		{
			this._codec = codec;
			this._codec.Message = null;
			this.blocks = null;
			if (w >= 8 && w <= 15)
			{
				this.wbits = w;
				this.blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? this : null, 1 << w);
				this.Reset();
				return 0;
			}
			this.End();
			throw new ZlibException("Bad window size.");
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00043EA0 File Offset: 0x000420A0
		internal int Inflate(FlushType flush)
		{
			if (this._codec.InputBuffer == null)
			{
				throw new ZlibException("InputBuffer is null. ");
			}
			int num = 0;
			int num2 = -5;
			for (;;)
			{
				switch (this.mode)
				{
				case InflateManager.InflateManagerMode.METHOD:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					if (((this.method = (int)this._codec.InputBuffer[this._codec.NextIn++]) & 15) != 8)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("unknown compression method (0x{0:X2})", this.method);
						this.marker = 5;
						continue;
					}
					if ((this.method >> 4) + 8 > this.wbits)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = string.Format("invalid window size ({0})", (this.method >> 4) + 8);
						this.marker = 5;
						continue;
					}
					this.mode = InflateManager.InflateManagerMode.FLAG;
					continue;
				case InflateManager.InflateManagerMode.FLAG:
				{
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					int num3 = (int)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
					if (((this.method << 8) + num3) % 31 != 0)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect header check";
						this.marker = 5;
						continue;
					}
					this.mode = (((num3 & 32) == 0) ? InflateManager.InflateManagerMode.BLOCKS : InflateManager.InflateManagerMode.DICT4);
					continue;
				}
				case InflateManager.InflateManagerMode.DICT4:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & 4278190080L);
						this.mode = InflateManager.InflateManagerMode.DICT3;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.DICT3:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
						this.mode = InflateManager.InflateManagerMode.DICT2;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.DICT2:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
						this.mode = InflateManager.InflateManagerMode.DICT1;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.DICT1:
					goto IL_651;
				case InflateManager.InflateManagerMode.DICT0:
					goto IL_6E1;
				case InflateManager.InflateManagerMode.BLOCKS:
					num2 = this.blocks.Process(num2);
					if (num2 == -3)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this.marker = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						return num2;
					}
					num2 = num;
					this.computedCheck = this.blocks.Reset();
					if (this.HandleRfc1950HeaderBytes)
					{
						this.mode = InflateManager.InflateManagerMode.CHECK4;
						continue;
					}
					goto IL_705;
				case InflateManager.InflateManagerMode.CHECK4:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck = (uint)((long)((long)this._codec.InputBuffer[this._codec.NextIn++] << 24) & 4278190080L);
						this.mode = InflateManager.InflateManagerMode.CHECK3;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.CHECK3:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
						this.mode = InflateManager.InflateManagerMode.CHECK2;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.CHECK2:
					if (this._codec.AvailableBytesIn != 0)
					{
						num2 = num;
						this._codec.AvailableBytesIn--;
						this._codec.TotalBytesIn += 1L;
						this.expectedCheck += (uint)((int)this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
						this.mode = InflateManager.InflateManagerMode.CHECK1;
						continue;
					}
					return num2;
				case InflateManager.InflateManagerMode.CHECK1:
					if (this._codec.AvailableBytesIn == 0)
					{
						return num2;
					}
					num2 = num;
					this._codec.AvailableBytesIn--;
					this._codec.TotalBytesIn += 1L;
					this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
					if (this.computedCheck != this.expectedCheck)
					{
						this.mode = InflateManager.InflateManagerMode.BAD;
						this._codec.Message = "incorrect data check";
						this.marker = 5;
						continue;
					}
					goto IL_717;
				case InflateManager.InflateManagerMode.DONE:
					return 1;
				case InflateManager.InflateManagerMode.BAD:
					goto IL_721;
				}
				goto Block_20;
			}
			return num2;
			Block_20:
			throw new ZlibException("Stream error.");
			IL_651:
			if (this._codec.AvailableBytesIn == 0)
			{
				return num2;
			}
			this._codec.AvailableBytesIn--;
			this._codec.TotalBytesIn += 1L;
			this.expectedCheck += (uint)(this._codec.InputBuffer[this._codec.NextIn++] & byte.MaxValue);
			this._codec._Adler32 = this.expectedCheck;
			this.mode = InflateManager.InflateManagerMode.DICT0;
			return 2;
			IL_6E1:
			this.mode = InflateManager.InflateManagerMode.BAD;
			this._codec.Message = "need dictionary";
			this.marker = 0;
			return -2;
			IL_705:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_717:
			this.mode = InflateManager.InflateManagerMode.DONE;
			return 1;
			IL_721:
			throw new ZlibException(string.Format("Bad state ({0})", this._codec.Message));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000445EC File Offset: 0x000427EC
		internal int SetDictionary(byte[] dictionary)
		{
			int start = 0;
			int num = dictionary.Length;
			if (this.mode != InflateManager.InflateManagerMode.DICT0)
			{
				throw new ZlibException("Stream error.");
			}
			if (Adler.Adler32(1U, dictionary, 0, dictionary.Length) != this._codec._Adler32)
			{
				return -3;
			}
			this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
			if (num >= 1 << this.wbits)
			{
				num = (1 << this.wbits) - 1;
				start = dictionary.Length - num;
			}
			this.blocks.SetDictionary(dictionary, start, num);
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0004467C File Offset: 0x0004287C
		internal int Sync()
		{
			if (this.mode != InflateManager.InflateManagerMode.BAD)
			{
				this.mode = InflateManager.InflateManagerMode.BAD;
				this.marker = 0;
			}
			int num;
			if ((num = this._codec.AvailableBytesIn) == 0)
			{
				return -5;
			}
			int num2 = this._codec.NextIn;
			int num3 = this.marker;
			while (num != 0 && num3 < 4)
			{
				if (this._codec.InputBuffer[num2] == InflateManager.mark[num3])
				{
					num3++;
				}
				else if (this._codec.InputBuffer[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			this._codec.TotalBytesIn += (long)(num2 - this._codec.NextIn);
			this._codec.NextIn = num2;
			this._codec.AvailableBytesIn = num;
			this.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long totalBytesIn = this._codec.TotalBytesIn;
			long totalBytesOut = this._codec.TotalBytesOut;
			this.Reset();
			this._codec.TotalBytesIn = totalBytesIn;
			this._codec.TotalBytesOut = totalBytesOut;
			this.mode = InflateManager.InflateManagerMode.BLOCKS;
			return 0;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000C95C File Offset: 0x0000AB5C
		internal int SyncPoint(ZlibCodec z)
		{
			return this.blocks.SyncPoint();
		}

		// Token: 0x04000553 RID: 1363
		private const int PRESET_DICT = 32;

		// Token: 0x04000554 RID: 1364
		private const int Z_DEFLATED = 8;

		// Token: 0x04000555 RID: 1365
		private InflateManager.InflateManagerMode mode;

		// Token: 0x04000556 RID: 1366
		internal ZlibCodec _codec;

		// Token: 0x04000557 RID: 1367
		internal int method;

		// Token: 0x04000558 RID: 1368
		internal uint computedCheck;

		// Token: 0x04000559 RID: 1369
		internal uint expectedCheck;

		// Token: 0x0400055A RID: 1370
		internal int marker;

		// Token: 0x0400055B RID: 1371
		private bool _handleRfc1950HeaderBytes = true;

		// Token: 0x0400055C RID: 1372
		internal int wbits;

		// Token: 0x0400055D RID: 1373
		internal InflateBlocks blocks;

		// Token: 0x0400055E RID: 1374
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x02000118 RID: 280
		private enum InflateManagerMode
		{
			// Token: 0x04000560 RID: 1376
			METHOD,
			// Token: 0x04000561 RID: 1377
			FLAG,
			// Token: 0x04000562 RID: 1378
			DICT4,
			// Token: 0x04000563 RID: 1379
			DICT3,
			// Token: 0x04000564 RID: 1380
			DICT2,
			// Token: 0x04000565 RID: 1381
			DICT1,
			// Token: 0x04000566 RID: 1382
			DICT0,
			// Token: 0x04000567 RID: 1383
			BLOCKS,
			// Token: 0x04000568 RID: 1384
			CHECK4,
			// Token: 0x04000569 RID: 1385
			CHECK3,
			// Token: 0x0400056A RID: 1386
			CHECK2,
			// Token: 0x0400056B RID: 1387
			CHECK1,
			// Token: 0x0400056C RID: 1388
			DONE,
			// Token: 0x0400056D RID: 1389
			BAD
		}
	}
}
