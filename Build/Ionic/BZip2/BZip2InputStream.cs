using System;
using System.IO;
using System.Runtime.InteropServices;
using Ionic.Crc;

namespace Ionic.BZip2
{
	// Token: 0x02000102 RID: 258
	[ComVisible(true)]
	public class BZip2InputStream : Stream
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x0000C211 File Offset: 0x0000A411
		public BZip2InputStream(Stream input) : this(input, false)
		{
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0000C21B File Offset: 0x0000A41B
		public BZip2InputStream(Stream input, bool leaveOpen)
		{
			this.input = input;
			this._leaveOpen = leaveOpen;
			this.init();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0003CC04 File Offset: 0x0003AE04
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (this.input == null)
			{
				throw new IOException("the stream is not open");
			}
			int num = offset + count;
			int num2 = offset;
			int num3;
			while (num2 < num && (num3 = this.ReadByte()) >= 0)
			{
				buffer[num2++] = (byte)num3;
			}
			if (num2 != offset)
			{
				return num2 - offset;
			}
			return -1;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0003CCB0 File Offset: 0x0003AEB0
		private void MakeMaps()
		{
			bool[] inUse = this.data.inUse;
			byte[] seqToUnseq = this.data.seqToUnseq;
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				if (inUse[i])
				{
					seqToUnseq[num++] = (byte)i;
				}
			}
			this.nInUse = num;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0003CCFC File Offset: 0x0003AEFC
		public override int ReadByte()
		{
			int result = this.currentChar;
			this.totalBytesRead += 1L;
			switch (this.currentState)
			{
			case BZip2InputStream.CState.EOF:
				return -1;
			case BZip2InputStream.CState.START_BLOCK:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_B:
				this.SetupRandPartB();
				break;
			case BZip2InputStream.CState.RAND_PART_C:
				this.SetupRandPartC();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.NO_RAND_PART_B:
				this.SetupNoRandPartB();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_C:
				this.SetupNoRandPartC();
				break;
			default:
				throw new IOException("bad state");
			}
			return result;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0000C251 File Offset: 0x0000A451
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanRead;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00009021 File Offset: 0x00007221
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0000C271 File Offset: 0x0000A471
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanWrite;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0000C291 File Offset: 0x0000A491
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("BZip2Stream");
			}
			this.input.Flush();
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0000C2B1 File Offset: 0x0000A4B1
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Position
		{
			get
			{
				return this.totalBytesRead;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000A639 File Offset: 0x00008839
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0000A639 File Offset: 0x00008839
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0000A639 File Offset: 0x00008839
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0003CDA0 File Offset: 0x0003AFA0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this.input != null)
					{
						this.input.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0003CDEC File Offset: 0x0003AFEC
		private void init()
		{
			if (this.input == null)
			{
				throw new IOException("No input Stream");
			}
			if (!this.input.CanRead)
			{
				throw new IOException("Unreadable input Stream");
			}
			this.CheckMagicChar('B', 0);
			this.CheckMagicChar('Z', 1);
			this.CheckMagicChar('h', 2);
			int num = this.input.ReadByte();
			if (num < 49 || num > 57)
			{
				throw new IOException("Stream is not BZip2 formatted: illegal blocksize " + (char)num);
			}
			this.blockSize100k = num - 48;
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0003CE84 File Offset: 0x0003B084
		private void CheckMagicChar(char expected, int position)
		{
			int num = this.input.ReadByte();
			if (num != (int)expected)
			{
				string message = string.Format("Not a valid BZip2 stream. byte {0}, expected '{1}', got '{2}'", position, (int)expected, num);
				throw new IOException(message);
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0003CEC8 File Offset: 0x0003B0C8
		private void InitBlock()
		{
			char c = this.bsGetUByte();
			char c2 = this.bsGetUByte();
			char c3 = this.bsGetUByte();
			char c4 = this.bsGetUByte();
			char c5 = this.bsGetUByte();
			char c6 = this.bsGetUByte();
			if (c == '\u0017' && c2 == 'r' && c3 == 'E' && c4 == '8' && c5 == 'P' && c6 == '\u0090')
			{
				this.complete();
				return;
			}
			if (c == '1' && c2 == 'A' && c3 == 'Y' && c4 == '&' && c5 == 'S')
			{
				if (c6 == 'Y')
				{
					this.storedBlockCRC = this.bsGetInt();
					this.blockRandomised = (this.GetBits(1) == 1);
					if (this.data == null)
					{
						this.data = new BZip2InputStream.DecompressionState(this.blockSize100k);
					}
					this.getAndMoveToFrontDecode();
					this.crc.Reset();
					this.currentState = BZip2InputStream.CState.START_BLOCK;
					return;
				}
			}
			this.currentState = BZip2InputStream.CState.EOF;
			string message = string.Format("bad block header at offset 0x{0:X}", this.input.Position);
			throw new IOException(message);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0003CFC8 File Offset: 0x0003B1C8
		private void EndBlock()
		{
			this.computedBlockCRC = (uint)this.crc.Crc32Result;
			if (this.storedBlockCRC != this.computedBlockCRC)
			{
				string message = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedBlockCRC, this.computedBlockCRC);
				throw new IOException(message);
			}
			this.computedCombinedCRC = (this.computedCombinedCRC << 1 | this.computedCombinedCRC >> 31);
			this.computedCombinedCRC ^= this.computedBlockCRC;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0003D048 File Offset: 0x0003B248
		private void complete()
		{
			this.storedCombinedCRC = this.bsGetInt();
			this.currentState = BZip2InputStream.CState.EOF;
			this.data = null;
			if (this.storedCombinedCRC != this.computedCombinedCRC)
			{
				string message = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedCombinedCRC, this.computedCombinedCRC);
				throw new IOException(message);
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0003D0A8 File Offset: 0x0003B2A8
		public override void Close()
		{
			Stream stream = this.input;
			if (stream != null)
			{
				try
				{
					if (!this._leaveOpen)
					{
						stream.Close();
					}
				}
				finally
				{
					this.data = null;
					this.input = null;
				}
			}
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0003D0F0 File Offset: 0x0003B2F0
		private int GetBits(int n)
		{
			int num = this.bsLive;
			int num2 = this.bsBuff;
			if (num < n)
			{
				for (;;)
				{
					int num3 = this.input.ReadByte();
					if (num3 < 0)
					{
						break;
					}
					num2 = (num2 << 8 | num3);
					num += 8;
					if (num >= n)
					{
						goto IL_3D;
					}
				}
				throw new IOException("unexpected end of stream");
				IL_3D:
				this.bsBuff = num2;
			}
			this.bsLive = num - n;
			return num2 >> num - n & (1 << n) - 1;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0003D15C File Offset: 0x0003B35C
		private bool bsGetBit()
		{
			int bits = this.GetBits(1);
			return bits != 0;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0000C2B9 File Offset: 0x0000A4B9
		private char bsGetUByte()
		{
			return (char)this.GetBits(8);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0000C2C3 File Offset: 0x0000A4C3
		private uint bsGetInt()
		{
			return (uint)(((this.GetBits(8) << 8 | this.GetBits(8)) << 8 | this.GetBits(8)) << 8 | this.GetBits(8));
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0003D178 File Offset: 0x0003B378
		private static void hbCreateDecodeTables(int[] limit, int[] bbase, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int i = minLen;
			int num = 0;
			while (i <= maxLen)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						perm[num++] = j;
					}
				}
				i++;
			}
			int num2 = BZip2.MaxCodeLength;
			while (--num2 > 0)
			{
				bbase[num2] = 0;
				limit[num2] = 0;
			}
			for (int k = 0; k < alphaSize; k++)
			{
				bbase[(int)(length[k] + '\u0001')]++;
			}
			int l = 1;
			int num3 = bbase[0];
			while (l < BZip2.MaxCodeLength)
			{
				num3 += bbase[l];
				bbase[l] = num3;
				l++;
			}
			int m = minLen;
			int num4 = 0;
			int num5 = bbase[m];
			while (m <= maxLen)
			{
				int num6 = bbase[m + 1];
				num4 += num6 - num5;
				num5 = num6;
				limit[m] = num4 - 1;
				num4 <<= 1;
				m++;
			}
			for (int n = minLen + 1; n <= maxLen; n++)
			{
				bbase[n] = (limit[n - 1] + 1 << 1) - bbase[n];
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0003D278 File Offset: 0x0003B478
		private void recvDecodingTables()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			bool[] inUse = decompressionState.inUse;
			byte[] recvDecodingTables_pos = decompressionState.recvDecodingTables_pos;
			int num = 0;
			for (int i = 0; i < 16; i++)
			{
				if (this.bsGetBit())
				{
					num |= 1 << i;
				}
			}
			int num2 = 256;
			while (--num2 >= 0)
			{
				inUse[num2] = false;
			}
			for (int j = 0; j < 16; j++)
			{
				if ((num & 1 << j) != 0)
				{
					int num3 = j << 4;
					for (int k = 0; k < 16; k++)
					{
						if (this.bsGetBit())
						{
							inUse[num3 + k] = true;
						}
					}
				}
			}
			this.MakeMaps();
			int num4 = this.nInUse + 2;
			int bits = this.GetBits(3);
			int bits2 = this.GetBits(15);
			for (int l = 0; l < bits2; l++)
			{
				int num5 = 0;
				while (this.bsGetBit())
				{
					num5++;
				}
				decompressionState.selectorMtf[l] = (byte)num5;
			}
			int num6 = bits;
			while (--num6 >= 0)
			{
				recvDecodingTables_pos[num6] = (byte)num6;
			}
			for (int m = 0; m < bits2; m++)
			{
				int n = (int)decompressionState.selectorMtf[m];
				byte b = recvDecodingTables_pos[n];
				while (n > 0)
				{
					recvDecodingTables_pos[n] = recvDecodingTables_pos[n - 1];
					n--;
				}
				recvDecodingTables_pos[0] = b;
				decompressionState.selector[m] = b;
			}
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int num7 = 0; num7 < bits; num7++)
			{
				int num8 = this.GetBits(5);
				char[] array = temp_charArray2d[num7];
				for (int num9 = 0; num9 < num4; num9++)
				{
					while (this.bsGetBit())
					{
						num8 += (this.bsGetBit() ? -1 : 1);
					}
					array[num9] = (char)num8;
				}
			}
			this.createHuffmanDecodingTables(num4, bits);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0003D438 File Offset: 0x0003B638
		private void createHuffmanDecodingTables(int alphaSize, int nGroups)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int i = 0; i < nGroups; i++)
			{
				int num = 32;
				int num2 = 0;
				char[] array = temp_charArray2d[i];
				int num3 = alphaSize;
				while (--num3 >= 0)
				{
					char c = array[num3];
					if ((int)c > num2)
					{
						num2 = (int)c;
					}
					if ((int)c < num)
					{
						num = (int)c;
					}
				}
				BZip2InputStream.hbCreateDecodeTables(decompressionState.gLimit[i], decompressionState.gBase[i], decompressionState.gPerm[i], temp_charArray2d[i], num, num2, alphaSize);
				decompressionState.gMinlen[i] = num;
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0003D4D4 File Offset: 0x0003B6D4
		private void getAndMoveToFrontDecode()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			this.origPtr = this.GetBits(24);
			if (this.origPtr < 0)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			if (this.origPtr > 10 + BZip2.BlockSizeMultiple * this.blockSize100k)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			this.recvDecodingTables();
			byte[] getAndMoveToFrontDecode_yy = decompressionState.getAndMoveToFrontDecode_yy;
			int num = this.blockSize100k * BZip2.BlockSizeMultiple;
			int num2 = 256;
			while (--num2 >= 0)
			{
				getAndMoveToFrontDecode_yy[num2] = (byte)num2;
				decompressionState.unzftab[num2] = 0;
			}
			int num3 = 0;
			int num4 = BZip2.G_SIZE - 1;
			int num5 = this.nInUse + 1;
			int num6 = this.getAndMoveToFrontDecode0(0);
			int num7 = this.bsBuff;
			int i = this.bsLive;
			int num8 = -1;
			int num9 = (int)(decompressionState.selector[0] & byte.MaxValue);
			int[] array = decompressionState.gBase[num9];
			int[] array2 = decompressionState.gLimit[num9];
			int[] array3 = decompressionState.gPerm[num9];
			int num10 = decompressionState.gMinlen[num9];
			while (num6 != num5)
			{
				if (num6 != (int)BZip2.RUNA)
				{
					if (num6 != (int)BZip2.RUNB)
					{
						if (++num8 < num)
						{
							byte b = getAndMoveToFrontDecode_yy[num6 - 1];
							decompressionState.unzftab[(int)(decompressionState.seqToUnseq[(int)b] & byte.MaxValue)]++;
							decompressionState.ll8[num8] = decompressionState.seqToUnseq[(int)b];
							if (num6 <= 16)
							{
								int j = num6 - 1;
								while (j > 0)
								{
									getAndMoveToFrontDecode_yy[j] = getAndMoveToFrontDecode_yy[--j];
								}
							}
							else
							{
								Buffer.BlockCopy(getAndMoveToFrontDecode_yy, 0, getAndMoveToFrontDecode_yy, 1, num6 - 1);
							}
							getAndMoveToFrontDecode_yy[0] = b;
							if (num4 == 0)
							{
								num4 = BZip2.G_SIZE - 1;
								num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
								array = decompressionState.gBase[num9];
								array2 = decompressionState.gLimit[num9];
								array3 = decompressionState.gPerm[num9];
								num10 = decompressionState.gMinlen[num9];
							}
							else
							{
								num4--;
							}
							int num11 = num10;
							while (i < num11)
							{
								int num12 = this.input.ReadByte();
								if (num12 < 0)
								{
									throw new IOException("unexpected end of stream");
								}
								num7 = (num7 << 8 | num12);
								i += 8;
							}
							int k = num7 >> i - num11 & (1 << num11) - 1;
							i -= num11;
							while (k > array2[num11])
							{
								num11++;
								while (i < 1)
								{
									int num13 = this.input.ReadByte();
									if (num13 < 0)
									{
										throw new IOException("unexpected end of stream");
									}
									num7 = (num7 << 8 | num13);
									i += 8;
								}
								i--;
								k = (k << 1 | (num7 >> i & 1));
							}
							num6 = array3[k - array[num11]];
							continue;
						}
						throw new IOException("block overrun");
					}
				}
				int num14 = -1;
				int num15 = 1;
				for (;;)
				{
					if (num6 == (int)BZip2.RUNA)
					{
						num14 += num15;
					}
					else
					{
						if (num6 != (int)BZip2.RUNB)
						{
							break;
						}
						num14 += num15 << 1;
					}
					if (num4 == 0)
					{
						num4 = BZip2.G_SIZE - 1;
						num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
						array = decompressionState.gBase[num9];
						array2 = decompressionState.gLimit[num9];
						array3 = decompressionState.gPerm[num9];
						num10 = decompressionState.gMinlen[num9];
					}
					else
					{
						num4--;
					}
					int num16 = num10;
					while (i < num16)
					{
						int num17 = this.input.ReadByte();
						if (num17 < 0)
						{
							goto IL_47E;
						}
						num7 = (num7 << 8 | num17);
						i += 8;
					}
					int l = num7 >> i - num16 & (1 << num16) - 1;
					i -= num16;
					while (l > array2[num16])
					{
						num16++;
						while (i < 1)
						{
							int num18 = this.input.ReadByte();
							if (num18 < 0)
							{
								goto IL_473;
							}
							num7 = (num7 << 8 | num18);
							i += 8;
						}
						i--;
						l = (l << 1 | (num7 >> i & 1));
					}
					num6 = array3[l - array[num16]];
					num15 <<= 1;
				}
				byte b2 = decompressionState.seqToUnseq[(int)getAndMoveToFrontDecode_yy[0]];
				decompressionState.unzftab[(int)(b2 & byte.MaxValue)] += num14 + 1;
				while (num14-- >= 0)
				{
					decompressionState.ll8[++num8] = b2;
				}
				if (num8 < num)
				{
					continue;
				}
				throw new IOException("block overrun");
				IL_473:
				throw new IOException("unexpected end of stream");
				IL_47E:
				throw new IOException("unexpected end of stream");
			}
			this.last = num8;
			this.bsLive = i;
			this.bsBuff = num7;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0003D990 File Offset: 0x0003BB90
		private int getAndMoveToFrontDecode0(int groupNo)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int num = (int)(decompressionState.selector[groupNo] & byte.MaxValue);
			int[] array = decompressionState.gLimit[num];
			int num2 = decompressionState.gMinlen[num];
			int i = this.GetBits(num2);
			int j = this.bsLive;
			int num3 = this.bsBuff;
			while (i > array[num2])
			{
				num2++;
				while (j < 1)
				{
					int num4 = this.input.ReadByte();
					if (num4 < 0)
					{
						throw new IOException("unexpected end of stream");
					}
					num3 = (num3 << 8 | num4);
					j += 8;
				}
				j--;
				i = (i << 1 | (num3 >> j & 1));
			}
			this.bsLive = j;
			this.bsBuff = num3;
			return decompressionState.gPerm[num][i - decompressionState.gBase[num][num2]];
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0003DA6C File Offset: 0x0003BC6C
		private void SetupBlock()
		{
			if (this.data == null)
			{
				return;
			}
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int[] array = decompressionState.initTT(this.last + 1);
			int i;
			for (i = 0; i <= 255; i++)
			{
				if (decompressionState.unzftab[i] < 0 || decompressionState.unzftab[i] > this.last)
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			decompressionState.cftab[0] = 0;
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] = decompressionState.unzftab[i - 1];
			}
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] += decompressionState.cftab[i - 1];
			}
			for (i = 0; i <= 256; i++)
			{
				if (decompressionState.cftab[i] < 0 || decompressionState.cftab[i] > this.last + 1)
				{
					string message = string.Format("BZ_DATA_ERROR: cftab[{0}]={1} last={2}", i, decompressionState.cftab[i], this.last);
					throw new Exception(message);
				}
			}
			for (i = 1; i <= 256; i++)
			{
				if (decompressionState.cftab[i - 1] > decompressionState.cftab[i])
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			i = 0;
			int num = this.last;
			while (i <= num)
			{
				array[decompressionState.cftab[(int)(decompressionState.ll8[i] & byte.MaxValue)]++] = i;
				i++;
			}
			if (this.origPtr < 0 || this.origPtr >= array.Length)
			{
				throw new IOException("stream corrupted");
			}
			this.su_tPos = array[this.origPtr];
			this.su_count = 0;
			this.su_i2 = 0;
			this.su_ch2 = 256;
			if (this.blockRandomised)
			{
				this.su_rNToGo = 0;
				this.su_rTPos = 0;
				this.SetupRandPartA();
				return;
			}
			this.SetupNoRandPartA();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0003DC64 File Offset: 0x0003BE64
		private void SetupRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				num = (this.su_ch2 = (num ^ ((this.su_rNToGo == 1) ? 1 : 0)));
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
				return;
			}
			this.EndBlock();
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0003DD5C File Offset: 0x0003BF5C
		private void SetupNoRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_ch2 = num;
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
				return;
			}
			this.currentState = BZip2InputStream.CState.NO_RAND_PART_A;
			this.EndBlock();
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0003DE00 File Offset: 0x0003C000
		private void SetupRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.currentState = BZip2InputStream.CState.RAND_PART_A;
				this.su_count = 1;
				this.SetupRandPartA();
				return;
			}
			if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				this.su_j2 = 0;
				this.currentState = BZip2InputStream.CState.RAND_PART_C;
				if (this.su_rNToGo == 1)
				{
					this.su_z ^= '\u0001';
				}
				this.SetupRandPartC();
				return;
			}
			this.currentState = BZip2InputStream.CState.RAND_PART_A;
			this.SetupRandPartA();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0003DF04 File Offset: 0x0003C104
		private void SetupRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				this.currentChar = this.su_ch2;
				this.crc.UpdateCRC((byte)this.su_ch2);
				this.su_j2++;
				return;
			}
			this.currentState = BZip2InputStream.CState.RAND_PART_A;
			this.su_i2++;
			this.su_count = 0;
			this.SetupRandPartA();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0003DF70 File Offset: 0x0003C170
		private void SetupNoRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.su_count = 1;
				this.SetupNoRandPartA();
				return;
			}
			if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_j2 = 0;
				this.SetupNoRandPartC();
				return;
			}
			this.SetupNoRandPartA();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0003DFF8 File Offset: 0x0003C1F8
		private void SetupNoRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				int num = this.su_ch2;
				this.currentChar = num;
				this.crc.UpdateCRC((byte)num);
				this.su_j2++;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_C;
				return;
			}
			this.su_i2++;
			this.su_count = 0;
			this.SetupNoRandPartA();
		}

		// Token: 0x04000441 RID: 1089
		private bool _disposed;

		// Token: 0x04000442 RID: 1090
		private bool _leaveOpen;

		// Token: 0x04000443 RID: 1091
		private long totalBytesRead;

		// Token: 0x04000444 RID: 1092
		private int last;

		// Token: 0x04000445 RID: 1093
		private int origPtr;

		// Token: 0x04000446 RID: 1094
		private int blockSize100k;

		// Token: 0x04000447 RID: 1095
		private bool blockRandomised;

		// Token: 0x04000448 RID: 1096
		private int bsBuff;

		// Token: 0x04000449 RID: 1097
		private int bsLive;

		// Token: 0x0400044A RID: 1098
		private readonly CRC32 crc = new CRC32(true);

		// Token: 0x0400044B RID: 1099
		private int nInUse;

		// Token: 0x0400044C RID: 1100
		private Stream input;

		// Token: 0x0400044D RID: 1101
		private int currentChar = -1;

		// Token: 0x0400044E RID: 1102
		private BZip2InputStream.CState currentState = BZip2InputStream.CState.START_BLOCK;

		// Token: 0x0400044F RID: 1103
		private uint storedBlockCRC;

		// Token: 0x04000450 RID: 1104
		private uint storedCombinedCRC;

		// Token: 0x04000451 RID: 1105
		private uint computedBlockCRC;

		// Token: 0x04000452 RID: 1106
		private uint computedCombinedCRC;

		// Token: 0x04000453 RID: 1107
		private int su_count;

		// Token: 0x04000454 RID: 1108
		private int su_ch2;

		// Token: 0x04000455 RID: 1109
		private int su_chPrev;

		// Token: 0x04000456 RID: 1110
		private int su_i2;

		// Token: 0x04000457 RID: 1111
		private int su_j2;

		// Token: 0x04000458 RID: 1112
		private int su_rNToGo;

		// Token: 0x04000459 RID: 1113
		private int su_rTPos;

		// Token: 0x0400045A RID: 1114
		private int su_tPos;

		// Token: 0x0400045B RID: 1115
		private char su_z;

		// Token: 0x0400045C RID: 1116
		private BZip2InputStream.DecompressionState data;

		// Token: 0x02000103 RID: 259
		private enum CState
		{
			// Token: 0x0400045E RID: 1118
			EOF,
			// Token: 0x0400045F RID: 1119
			START_BLOCK,
			// Token: 0x04000460 RID: 1120
			RAND_PART_A,
			// Token: 0x04000461 RID: 1121
			RAND_PART_B,
			// Token: 0x04000462 RID: 1122
			RAND_PART_C,
			// Token: 0x04000463 RID: 1123
			NO_RAND_PART_A,
			// Token: 0x04000464 RID: 1124
			NO_RAND_PART_B,
			// Token: 0x04000465 RID: 1125
			NO_RAND_PART_C
		}

		// Token: 0x02000104 RID: 260
		private sealed class DecompressionState
		{
			// Token: 0x060006E9 RID: 1769 RVA: 0x0003E060 File Offset: 0x0003C260
			public DecompressionState(int blockSize100k)
			{
				this.unzftab = new int[256];
				this.gLimit = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gBase = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gPerm = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gMinlen = new int[BZip2.NGroups];
				this.cftab = new int[257];
				this.getAndMoveToFrontDecode_yy = new byte[256];
				this.temp_charArray2d = BZip2.InitRectangularArray<char>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.recvDecodingTables_pos = new byte[BZip2.NGroups];
				this.ll8 = new byte[blockSize100k * BZip2.BlockSizeMultiple];
			}

			// Token: 0x060006EA RID: 1770 RVA: 0x0003E16C File Offset: 0x0003C36C
			public int[] initTT(int length)
			{
				int[] array = this.tt;
				if (array == null || array.Length < length)
				{
					array = (this.tt = new int[length]);
				}
				return array;
			}

			// Token: 0x04000466 RID: 1126
			public readonly bool[] inUse = new bool[256];

			// Token: 0x04000467 RID: 1127
			public readonly byte[] seqToUnseq = new byte[256];

			// Token: 0x04000468 RID: 1128
			public readonly byte[] selector = new byte[BZip2.MaxSelectors];

			// Token: 0x04000469 RID: 1129
			public readonly byte[] selectorMtf = new byte[BZip2.MaxSelectors];

			// Token: 0x0400046A RID: 1130
			public readonly int[] unzftab;

			// Token: 0x0400046B RID: 1131
			public readonly int[][] gLimit;

			// Token: 0x0400046C RID: 1132
			public readonly int[][] gBase;

			// Token: 0x0400046D RID: 1133
			public readonly int[][] gPerm;

			// Token: 0x0400046E RID: 1134
			public readonly int[] gMinlen;

			// Token: 0x0400046F RID: 1135
			public readonly int[] cftab;

			// Token: 0x04000470 RID: 1136
			public readonly byte[] getAndMoveToFrontDecode_yy;

			// Token: 0x04000471 RID: 1137
			public readonly char[][] temp_charArray2d;

			// Token: 0x04000472 RID: 1138
			public readonly byte[] recvDecodingTables_pos;

			// Token: 0x04000473 RID: 1139
			public int[] tt;

			// Token: 0x04000474 RID: 1140
			public byte[] ll8;
		}
	}
}
