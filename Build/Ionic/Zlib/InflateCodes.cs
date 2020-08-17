using System;

namespace Ionic.Zlib
{
	// Token: 0x02000116 RID: 278
	internal sealed class InflateCodes
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x00008754 File Offset: 0x00006954
		internal InflateCodes()
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000C8CA File Offset: 0x0000AACA
		internal void Init(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index)
		{
			this.mode = 0;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = tl_index;
			this.dtree = td;
			this.dtree_index = td_index;
			this.tree = null;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00042D40 File Offset: 0x00040F40
		internal int Process(InflateBlocks blocks, int r)
		{
			ZlibCodec codec = blocks._codec;
			int num = codec.NextIn;
			int num2 = codec.AvailableBytesIn;
			int num3 = blocks.bitb;
			int i = blocks.bitk;
			int num4 = blocks.writeAt;
			int num5 = (num4 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4);
			for (;;)
			{
				int num6;
				switch (this.mode)
				{
				case 0:
					if (num5 >= 258 && num2 >= 10)
					{
						blocks.bitb = num3;
						blocks.bitk = i;
						codec.AvailableBytesIn = num2;
						codec.TotalBytesIn += (long)(num - codec.NextIn);
						codec.NextIn = num;
						blocks.writeAt = num4;
						r = this.InflateFast((int)this.lbits, (int)this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, blocks, codec);
						num = codec.NextIn;
						num2 = codec.AvailableBytesIn;
						num3 = blocks.bitb;
						i = blocks.bitk;
						num4 = blocks.writeAt;
						num5 = ((num4 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
						if (r != 0)
						{
							this.mode = ((r == 1) ? 7 : 9);
							continue;
						}
					}
					this.need = (int)this.lbits;
					this.tree = this.ltree;
					this.tree_index = this.ltree_index;
					this.mode = 1;
					goto IL_441;
				case 1:
					goto IL_441;
				case 2:
					num6 = this.bitsToGet;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_773;
						}
						r = 0;
						num2--;
						num3 |= (int)(codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.len += (num3 & InternalInflateConstants.InflateMask[num6]);
					num3 >>= num6;
					i -= num6;
					this.need = (int)this.dbits;
					this.tree = this.dtree;
					this.tree_index = this.dtree_index;
					this.mode = 3;
					goto IL_2D8;
				case 3:
					goto IL_2D8;
				case 4:
					num6 = this.bitsToGet;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_854;
						}
						r = 0;
						num2--;
						num3 |= (int)(codec.InputBuffer[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.dist += (num3 & InternalInflateConstants.InflateMask[num6]);
					num3 >>= num6;
					i -= num6;
					this.mode = 5;
					goto IL_13C;
				case 5:
					goto IL_13C;
				case 6:
					if (num5 == 0)
					{
						if (num4 == blocks.end && blocks.readAt != 0)
						{
							num4 = 0;
							num5 = ((0 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
						}
						if (num5 == 0)
						{
							blocks.writeAt = num4;
							r = blocks.Flush(r);
							num4 = blocks.writeAt;
							num5 = ((num4 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
							if (num4 == blocks.end && blocks.readAt != 0)
							{
								num4 = 0;
								num5 = ((0 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
							}
							if (num5 == 0)
							{
								goto IL_8DA;
							}
						}
					}
					r = 0;
					blocks.window[num4++] = (byte)this.lit;
					num5--;
					this.mode = 0;
					continue;
				case 7:
					goto IL_91D;
				case 8:
					goto IL_9C2;
				case 9:
					goto IL_A08;
				}
				break;
				IL_13C:
				int j;
				for (j = num4 - this.dist; j < 0; j += blocks.end)
				{
				}
				while (this.len != 0)
				{
					if (num5 == 0)
					{
						if (num4 == blocks.end && blocks.readAt != 0)
						{
							num4 = 0;
							num5 = ((0 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
						}
						if (num5 == 0)
						{
							blocks.writeAt = num4;
							r = blocks.Flush(r);
							num4 = blocks.writeAt;
							num5 = ((num4 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
							if (num4 == blocks.end && blocks.readAt != 0)
							{
								num4 = 0;
								num5 = ((0 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4));
							}
							if (num5 == 0)
							{
								goto IL_897;
							}
						}
					}
					blocks.window[num4++] = blocks.window[j++];
					num5--;
					if (j == blocks.end)
					{
						j = 0;
					}
					this.len--;
				}
				this.mode = 0;
				continue;
				IL_2D8:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_811;
					}
					r = 0;
					num2--;
					num3 |= (int)(codec.InputBuffer[num++] & byte.MaxValue) << i;
					i += 8;
				}
				int num7 = (this.tree_index + (num3 & InternalInflateConstants.InflateMask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				int num8 = this.tree[num7];
				if ((num8 & 16) != 0)
				{
					this.bitsToGet = (num8 & 15);
					this.dist = this.tree[num7 + 2];
					this.mode = 4;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				goto IL_7B6;
				IL_441:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_730;
					}
					r = 0;
					num2--;
					num3 |= (int)(codec.InputBuffer[num++] & byte.MaxValue) << i;
					i += 8;
				}
				num7 = (this.tree_index + (num3 & InternalInflateConstants.InflateMask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				num8 = this.tree[num7];
				if (num8 == 0)
				{
					this.lit = this.tree[num7 + 2];
					this.mode = 6;
				}
				else if ((num8 & 16) != 0)
				{
					this.bitsToGet = (num8 & 15);
					this.len = this.tree[num7 + 2];
					this.mode = 2;
				}
				else if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
				}
				else
				{
					if ((num8 & 32) == 0)
					{
						goto IL_6D5;
					}
					this.mode = 7;
				}
			}
			r = -2;
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(-2);
			IL_6D5:
			this.mode = 9;
			codec.Message = "invalid literal/length code";
			r = -3;
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(-3);
			IL_730:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_773:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_7B6:
			this.mode = 9;
			codec.Message = "invalid distance code";
			r = -3;
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(-3);
			IL_811:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_854:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_897:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_8DA:
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(r);
			IL_91D:
			if (i > 7)
			{
				i -= 8;
				num2++;
				num--;
			}
			blocks.writeAt = num4;
			r = blocks.Flush(r);
			num4 = blocks.writeAt;
			int num9 = (num4 < blocks.readAt) ? (blocks.readAt - num4 - 1) : (blocks.end - num4);
			if (blocks.readAt != blocks.writeAt)
			{
				blocks.bitb = num3;
				blocks.bitk = i;
				codec.AvailableBytesIn = num2;
				codec.TotalBytesIn += (long)(num - codec.NextIn);
				codec.NextIn = num;
				blocks.writeAt = num4;
				return blocks.Flush(r);
			}
			this.mode = 8;
			IL_9C2:
			r = 1;
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(1);
			IL_A08:
			r = -3;
			blocks.bitb = num3;
			blocks.bitk = i;
			codec.AvailableBytesIn = num2;
			codec.TotalBytesIn += (long)(num - codec.NextIn);
			codec.NextIn = num;
			blocks.writeAt = num4;
			return blocks.Flush(-3);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0004379C File Offset: 0x0004199C
		internal int InflateFast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InflateBlocks s, ZlibCodec z)
		{
			int num = z.NextIn;
			int num2 = z.AvailableBytesIn;
			int num3 = s.bitb;
			int i = s.bitk;
			int num4 = s.writeAt;
			int num5 = (num4 < s.readAt) ? (s.readAt - num4 - 1) : (s.end - num4);
			int num6 = InternalInflateConstants.InflateMask[bl];
			int num7 = InternalInflateConstants.InflateMask[bd];
			int num10;
			int num11;
			for (;;)
			{
				if (i >= 20)
				{
					int num8 = num3 & num6;
					int num9 = (tl_index + num8) * 3;
					if ((num10 = tl[num9]) == 0)
					{
						num3 >>= tl[num9 + 1];
						i -= tl[num9 + 1];
						s.window[num4++] = (byte)tl[num9 + 2];
						num5--;
					}
					else
					{
						for (;;)
						{
							num3 >>= tl[num9 + 1];
							i -= tl[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_531;
							}
							num8 += tl[num9 + 2];
							num8 += (num3 & InternalInflateConstants.InflateMask[num10]);
							num9 = (tl_index + num8) * 3;
							if ((num10 = tl[num9]) == 0)
							{
								goto IL_3F1;
							}
						}
						num10 &= 15;
						num11 = tl[num9 + 2] + (num3 & InternalInflateConstants.InflateMask[num10]);
						num3 >>= num10;
						for (i -= num10; i < 15; i += 8)
						{
							num2--;
							num3 |= (int)(z.InputBuffer[num++] & byte.MaxValue) << i;
						}
						num8 = (num3 & num7);
						num9 = (td_index + num8) * 3;
						num10 = td[num9];
						for (;;)
						{
							num3 >>= td[num9 + 1];
							i -= td[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_446;
							}
							num8 += td[num9 + 2];
							num8 += (num3 & InternalInflateConstants.InflateMask[num10]);
							num9 = (td_index + num8) * 3;
							num10 = td[num9];
						}
						num10 &= 15;
						while (i < num10)
						{
							num2--;
							num3 |= (int)(z.InputBuffer[num++] & byte.MaxValue) << i;
							i += 8;
						}
						int num12 = td[num9 + 2] + (num3 & InternalInflateConstants.InflateMask[num10]);
						num3 >>= num10;
						i -= num10;
						num5 -= num11;
						int num13;
						if (num4 >= num12)
						{
							num13 = num4 - num12;
							if (num4 - num13 > 0 && 2 > num4 - num13)
							{
								s.window[num4++] = s.window[num13++];
								s.window[num4++] = s.window[num13++];
								num11 -= 2;
							}
							else
							{
								Array.Copy(s.window, num13, s.window, num4, 2);
								num4 += 2;
								num13 += 2;
								num11 -= 2;
							}
						}
						else
						{
							num13 = num4 - num12;
							do
							{
								num13 += s.end;
							}
							while (num13 < 0);
							num10 = s.end - num13;
							if (num11 > num10)
							{
								num11 -= num10;
								if (num4 - num13 > 0 && num10 > num4 - num13)
								{
									do
									{
										s.window[num4++] = s.window[num13++];
									}
									while (--num10 != 0);
								}
								else
								{
									Array.Copy(s.window, num13, s.window, num4, num10);
									num4 += num10;
									num13 += num10;
								}
								num13 = 0;
							}
						}
						if (num4 - num13 > 0 && num11 > num4 - num13)
						{
							do
							{
								s.window[num4++] = s.window[num13++];
							}
							while (--num11 != 0);
							goto IL_425;
						}
						Array.Copy(s.window, num13, s.window, num4, num11);
						num4 += num11;
						num13 += num11;
						goto IL_425;
						IL_3F1:
						num3 >>= tl[num9 + 1];
						i -= tl[num9 + 1];
						s.window[num4++] = (byte)tl[num9 + 2];
						num5--;
					}
					IL_425:
					if (num5 < 258 || num2 < 10)
					{
						goto IL_4C2;
					}
				}
				else
				{
					num2--;
					num3 |= (int)(z.InputBuffer[num++] & byte.MaxValue) << i;
					i += 8;
				}
			}
			IL_446:
			z.Message = "invalid distance code";
			num11 = z.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.AvailableBytesIn = num2;
			z.TotalBytesIn += (long)(num - z.NextIn);
			z.NextIn = num;
			s.writeAt = num4;
			return -3;
			IL_4C2:
			num11 = z.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.AvailableBytesIn = num2;
			z.TotalBytesIn += (long)(num - z.NextIn);
			z.NextIn = num;
			s.writeAt = num4;
			return 0;
			IL_531:
			if ((num10 & 32) != 0)
			{
				num11 = z.AvailableBytesIn - num2;
				num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
				num2 += num11;
				num -= num11;
				i -= num11 << 3;
				s.bitb = num3;
				s.bitk = i;
				z.AvailableBytesIn = num2;
				z.TotalBytesIn += (long)(num - z.NextIn);
				z.NextIn = num;
				s.writeAt = num4;
				return 1;
			}
			z.Message = "invalid literal/length code";
			num11 = z.AvailableBytesIn - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.AvailableBytesIn = num2;
			z.TotalBytesIn += (long)(num - z.NextIn);
			z.NextIn = num;
			s.writeAt = num4;
			return -3;
		}

		// Token: 0x0400053B RID: 1339
		private const int START = 0;

		// Token: 0x0400053C RID: 1340
		private const int LEN = 1;

		// Token: 0x0400053D RID: 1341
		private const int LENEXT = 2;

		// Token: 0x0400053E RID: 1342
		private const int DIST = 3;

		// Token: 0x0400053F RID: 1343
		private const int DISTEXT = 4;

		// Token: 0x04000540 RID: 1344
		private const int COPY = 5;

		// Token: 0x04000541 RID: 1345
		private const int LIT = 6;

		// Token: 0x04000542 RID: 1346
		private const int WASH = 7;

		// Token: 0x04000543 RID: 1347
		private const int END = 8;

		// Token: 0x04000544 RID: 1348
		private const int BADCODE = 9;

		// Token: 0x04000545 RID: 1349
		internal int mode;

		// Token: 0x04000546 RID: 1350
		internal int len;

		// Token: 0x04000547 RID: 1351
		internal int[] tree;

		// Token: 0x04000548 RID: 1352
		internal int tree_index;

		// Token: 0x04000549 RID: 1353
		internal int need;

		// Token: 0x0400054A RID: 1354
		internal int lit;

		// Token: 0x0400054B RID: 1355
		internal int bitsToGet;

		// Token: 0x0400054C RID: 1356
		internal int dist;

		// Token: 0x0400054D RID: 1357
		internal byte lbits;

		// Token: 0x0400054E RID: 1358
		internal byte dbits;

		// Token: 0x0400054F RID: 1359
		internal int[] ltree;

		// Token: 0x04000550 RID: 1360
		internal int ltree_index;

		// Token: 0x04000551 RID: 1361
		internal int[] dtree;

		// Token: 0x04000552 RID: 1362
		internal int dtree_index;
	}
}
