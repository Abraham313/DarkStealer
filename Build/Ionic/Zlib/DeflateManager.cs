using System;

namespace Ionic.Zlib
{
	// Token: 0x0200010E RID: 270
	internal sealed class DeflateManager
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x0003EE40 File Offset: 0x0003D040
		internal DeflateManager()
		{
			this.dyn_ltree = new short[DeflateManager.HEAP_SIZE * 2];
			this.dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
			this.bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0003EEF4 File Offset: 0x0003D0F4
		private void _InitializeLazyMatch()
		{
			this.window_size = 2 * this.w_size;
			Array.Clear(this.head, 0, this.hash_size);
			this.config = DeflateManager.Config.Lookup(this.compressionLevel);
			this.SetDeflater();
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = DeflateManager.MIN_MATCH - 1);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0003EF74 File Offset: 0x0003D174
		private void _InitializeTreeData()
		{
			this.treeLiterals.dyn_tree = this.dyn_ltree;
			this.treeLiterals.staticTree = StaticTree.Literals;
			this.treeDistances.dyn_tree = this.dyn_dtree;
			this.treeDistances.staticTree = StaticTree.Distances;
			this.treeBitLengths.dyn_tree = this.bl_tree;
			this.treeBitLengths.staticTree = StaticTree.BitLengths;
			this.bi_buf = 0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this._InitializeBlocks();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0003F000 File Offset: 0x0003D200
		internal void _InitializeBlocks()
		{
			for (int i = 0; i < InternalConstants.L_CODES; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < InternalConstants.D_CODES; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < InternalConstants.BL_CODES; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[DeflateManager.END_BLOCK * 2] = 1;
			this.static_len = 0;
			this.opt_len = 0;
			this.matches = 0;
			this.last_lit = 0;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0003F08C File Offset: 0x0003D28C
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && DeflateManager._IsSmaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (DeflateManager._IsSmaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0003F118 File Offset: 0x0003D318
		internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0003F148 File Offset: 0x0003D348
		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = short.MaxValue;
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array = this.bl_tree;
							int num7 = num6 * 2;
							array[num7] += 1;
						}
						short[] array2 = this.bl_tree;
						int num8 = InternalConstants.REP_3_6 * 2;
						array2[num8] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array3 = this.bl_tree;
						int num9 = InternalConstants.REPZ_3_10 * 2;
						array3[num9] += 1;
					}
					else
					{
						short[] array4 = this.bl_tree;
						int num10 = InternalConstants.REPZ_11_138 * 2;
						array4[num10] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0003F264 File Offset: 0x0003D464
		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.treeLiterals.max_code);
			this.scan_tree(this.dyn_dtree, this.treeDistances.max_code);
			this.treeBitLengths.build_tree(this);
			int num = InternalConstants.BL_CODES - 1;
			while (num >= 3 && this.bl_tree[(int)(Tree.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(Tree.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0003F360 File Offset: 0x0003D560
		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(InternalConstants.REP_3_6, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000C48C File Offset: 0x0000A68C
		private void put_bytes(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending, this.pendingCount, len);
			this.pendingCount += len;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0003F470 File Offset: 0x0003D670
		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0003F49C File Offset: 0x0003D69C
		internal void send_bits(int value, int length)
		{
			if (this.bi_valid > DeflateManager.Buf_size - length)
			{
				this.bi_buf |= (short)(value << this.bi_valid & 65535);
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (short)((uint)value >> DeflateManager.Buf_size - this.bi_valid);
				this.bi_valid += length - DeflateManager.Buf_size;
				return;
			}
			this.bi_buf |= (short)(value << this.bi_valid & 65535);
			this.bi_valid += length;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0003F578 File Offset: 0x0003D778
		internal void _tr_align()
		{
			this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
			this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(DeflateManager.STATIC_TREES << 1, 3);
				this.send_code(DeflateManager.END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0003F5EC File Offset: 0x0003D7EC
		internal bool _tr_tally(int dist, int lc)
		{
			this.pending[this._distanceOffset + this.last_lit * 2] = (byte)((uint)dist >> 8);
			this.pending[this._distanceOffset + this.last_lit * 2 + 1] = (byte)dist;
			this.pending[this._lengthOffset + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] array = this.dyn_ltree;
				int num = lc * 2;
				array[num] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] array2 = this.dyn_ltree;
				int num2 = ((int)Tree.LengthCode[lc] + InternalConstants.LITERALS + 1) * 2;
				array2[num2] += 1;
				short[] array3 = this.dyn_dtree;
				int num3 = Tree.DistanceCode(dist) * 2;
				array3[num3] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.compressionLevel > CompressionLevel.Level2)
			{
				int num4 = this.last_lit << 3;
				int num5 = this.strstart - this.block_start;
				for (int i = 0; i < InternalConstants.D_CODES; i++)
				{
					num4 = (int)((long)num4 + (long)this.dyn_dtree[i * 2] * (5L + (long)Tree.ExtraDistanceBits[i]));
				}
				num4 >>= 3;
				if (this.matches < this.last_lit / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1 || this.last_lit == this.lit_bufsize;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0003F758 File Offset: 0x0003D958
		internal void send_compressed_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = this._distanceOffset + num * 2;
					int num3 = ((int)this.pending[num2] << 8 & 65280) | (int)(this.pending[num2 + 1] & byte.MaxValue);
					int num4 = (int)(this.pending[this._lengthOffset + num] & byte.MaxValue);
					num++;
					if (num3 == 0)
					{
						this.send_code(num4, ltree);
					}
					else
					{
						int num5 = (int)Tree.LengthCode[num4];
						this.send_code(num5 + InternalConstants.LITERALS + 1, ltree);
						int num6 = Tree.ExtraLengthBits[num5];
						if (num6 != 0)
						{
							num4 -= Tree.LengthBase[num5];
							this.send_bits(num4, num6);
						}
						num3--;
						num5 = Tree.DistanceCode(num3);
						this.send_code(num5, dtree);
						num6 = Tree.ExtraDistanceBits[num5];
						if (num6 != 0)
						{
							num3 -= Tree.DistanceBase[num5];
							this.send_bits(num3, num6);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(DeflateManager.END_BLOCK, ltree);
			this.last_eob_len = (int)ltree[DeflateManager.END_BLOCK * 2 + 1];
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0003F86C File Offset: 0x0003DA6C
		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < InternalConstants.LITERALS)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = (sbyte)((num2 > num >> 2) ? DeflateManager.Z_BINARY : DeflateManager.Z_ASCII);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0003F8E8 File Offset: 0x0003DAE8
		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
				this.bi_buf = 0;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.bi_buf = (short)(this.bi_buf >> 8);
				this.bi_valid -= 8;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0003F994 File Offset: 0x0003DB94
		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
				this.pending[this.pendingCount++] = (byte)(this.bi_buf >> 8);
			}
			else if (this.bi_valid > 0)
			{
				this.pending[this.pendingCount++] = (byte)this.bi_buf;
			}
			this.bi_buf = 0;
			this.bi_valid = 0;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0003FA24 File Offset: 0x0003DC24
		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				this.pending[this.pendingCount++] = (byte)len;
				this.pending[this.pendingCount++] = (byte)(len >> 8);
				this.pending[this.pendingCount++] = (byte)(~(byte)len);
				this.pending[this.pendingCount++] = (byte)(~len >> 8);
			}
			this.put_bytes(this.window, buf, len);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this._codec.flush_pending();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0003FAC0 File Offset: 0x0003DCC0
		internal BlockState DeflateNone(FlushType flush)
		{
			int num = 65535;
			if (65535 > this.pending.Length - 5)
			{
				num = this.pending.Length - 5;
			}
			for (;;)
			{
				if (this.lookahead <= 1)
				{
					this._fillWindow();
					if (this.lookahead == 0 && flush == FlushType.None)
					{
						return BlockState.NeedMore;
					}
					if (this.lookahead == 0)
					{
						goto IL_E9;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						break;
					}
				}
			}
			return BlockState.NeedMore;
			IL_E9:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.NeedMore;
				}
				return BlockState.FinishStarted;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
			return BlockState.NeedMore;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000C4EF File Offset: 0x0000A6EF
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits((DeflateManager.STORED_BLOCK << 1) + (eof ? 1 : 0), 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0003FBE0 File Offset: 0x0003DDE0
		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.compressionLevel > CompressionLevel.None)
			{
				if ((int)this.data_type == DeflateManager.Z_UNKNOWN)
				{
					this.set_data_type();
				}
				this.treeLiterals.build_tree(this);
				this.treeDistances.build_tree(this);
				num = this.build_bl_tree();
				num2 = this.opt_len + 3 + 7 >> 3;
				num3 = this.static_len + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits((DeflateManager.STATIC_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
			}
			else
			{
				this.send_bits((DeflateManager.DYN_TREES << 1) + (eof ? 1 : 0), 3);
				this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num + 1);
				this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
			}
			this._InitializeBlocks();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0003FCF0 File Offset: 0x0003DEF0
		private void _fillWindow()
		{
			do
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 < this.w_size) ? 0 : (num4 - this.w_size));
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num4 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num4 < this.w_size) ? 0 : (num4 - this.w_size));
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this._codec.AvailableBytesIn == 0)
				{
					return;
				}
				num2 = this._codec.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
				}
				if (this.lookahead >= DeflateManager.MIN_LOOKAHEAD)
				{
					break;
				}
			}
			while (this._codec.AvailableBytesIn != 0);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0003FF00 File Offset: 0x0003E100
		internal BlockState DeflateFast(FlushType flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						return BlockState.NeedMore;
					}
					if (this.lookahead == 0)
					{
						goto IL_2F7;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD && this.compressionStrategy != CompressionStrategy.HuffmanOnly)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= DeflateManager.MIN_MATCH)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.config.MaxLazy && this.lookahead >= DeflateManager.MIN_MATCH)
					{
						this.match_length--;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						while (--this.match_length != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & byte.MaxValue));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this._codec.AvailableBytesOut == 0)
					{
						break;
					}
				}
			}
			return BlockState.NeedMore;
			IL_2F7:
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0004022C File Offset: 0x0003E42C
		internal BlockState DeflateSlow(FlushType flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < DeflateManager.MIN_LOOKAHEAD)
				{
					this._fillWindow();
					if (this.lookahead < DeflateManager.MIN_LOOKAHEAD && flush == FlushType.None)
					{
						return BlockState.NeedMore;
					}
					if (this.lookahead == 0)
					{
						goto IL_385;
					}
				}
				if (this.lookahead >= DeflateManager.MIN_MATCH)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = DeflateManager.MIN_MATCH - 1;
				if (num != 0 && this.prev_length < this.config.MaxLazy && (this.strstart - num & 65535) <= this.w_size - DeflateManager.MIN_LOOKAHEAD)
				{
					if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.compressionStrategy == CompressionStrategy.Filtered || (this.match_length == DeflateManager.MIN_MATCH && this.strstart - this.match_start > 4096)))
					{
						this.match_length = DeflateManager.MIN_MATCH - 1;
					}
				}
				if (this.prev_length >= DeflateManager.MIN_MATCH && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - DeflateManager.MIN_MATCH;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - DeflateManager.MIN_MATCH);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					do
					{
						if (++this.strstart <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
					}
					while (--this.prev_length != 0);
					this.match_available = 0;
					this.match_length = DeflateManager.MIN_MATCH - 1;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this._codec.AvailableBytesOut == 0)
						{
							break;
						}
					}
				}
				else if (this.match_available != 0)
				{
					if (this._tr_tally(0, (int)(this.window[this.strstart - 1] & 255)))
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this._codec.AvailableBytesOut == 0)
					{
						return BlockState.NeedMore;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return BlockState.NeedMore;
			IL_385:
			if (this.match_available != 0)
			{
				bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
				this.match_available = 0;
			}
			this.flush_block_only(flush == FlushType.Finish);
			if (this._codec.AvailableBytesOut == 0)
			{
				if (flush == FlushType.Finish)
				{
					return BlockState.FinishStarted;
				}
				return BlockState.NeedMore;
			}
			else
			{
				if (flush != FlushType.Finish)
				{
					return BlockState.BlockDone;
				}
				return BlockState.FinishDone;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00040614 File Offset: 0x0003E814
		internal int longest_match(int cur_match)
		{
			int num = this.config.MaxChainLength;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - DeflateManager.MIN_LOOKAHEAD) ? (this.strstart - (this.w_size - DeflateManager.MIN_LOOKAHEAD)) : 0;
			int niceLength = this.config.NiceLength;
			int num5 = this.w_mask;
			int num6 = this.strstart + DeflateManager.MAX_MATCH;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.config.GoodLength)
			{
				num >>= 2;
			}
			if (niceLength > this.lookahead)
			{
				niceLength = this.lookahead;
			}
			do
			{
				int num7 = cur_match;
				if (this.window[num7 + num3] == b2 && this.window[num7 + num3 - 1] == b && this.window[num7] == this.window[num2] && this.window[++num7] == this.window[num2 + 1])
				{
					num2 += 2;
					num7++;
					while (this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && this.window[++num2] == this.window[++num7] && num2 < num6)
					{
					}
					int num8 = DeflateManager.MAX_MATCH - (num6 - num2);
					num2 = num6 - DeflateManager.MAX_MATCH;
					if (num8 > num3)
					{
						this.match_start = cur_match;
						num3 = num8;
						if (num8 >= niceLength)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
				if ((cur_match = ((int)this.prev[cur_match & num5] & 65535)) <= num4)
				{
					break;
				}
			}
			while (--num != 0);
			if (num3 <= this.lookahead)
			{
				return num3;
			}
			return this.lookahead;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0000C510 File Offset: 0x0000A710
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0000C518 File Offset: 0x0000A718
		internal bool WantRfc1950HeaderBytes
		{
			get
			{
				return this._WantRfc1950HeaderBytes;
			}
			set
			{
				this._WantRfc1950HeaderBytes = value;
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0000C521 File Offset: 0x0000A721
		internal int Initialize(ZlibCodec codec, CompressionLevel level)
		{
			return this.Initialize(codec, level, 15);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0000C52D File Offset: 0x0000A72D
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, CompressionStrategy.Default);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0000C53E File Offset: 0x0000A73E
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy)
		{
			return this.Initialize(codec, level, bits, DeflateManager.MEM_LEVEL_DEFAULT, compressionStrategy);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000408B4 File Offset: 0x0003EAB4
		internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			this._codec = codec;
			this._codec.Message = null;
			if (windowBits < 9 || windowBits > 15)
			{
				throw new ZlibException("windowBits must be in the range 9..15.");
			}
			if (memLevel < 1 || memLevel > DeflateManager.MEM_LEVEL_MAX)
			{
				throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", DeflateManager.MEM_LEVEL_MAX));
			}
			this._codec.dstate = this;
			this.w_bits = windowBits;
			this.w_size = 1 << this.w_bits;
			this.w_mask = this.w_size - 1;
			this.hash_bits = memLevel + 7;
			this.hash_size = 1 << this.hash_bits;
			this.hash_mask = this.hash_size - 1;
			this.hash_shift = (this.hash_bits + DeflateManager.MIN_MATCH - 1) / DeflateManager.MIN_MATCH;
			this.window = new byte[this.w_size * 2];
			this.prev = new short[this.w_size];
			this.head = new short[this.hash_size];
			this.lit_bufsize = 1 << memLevel + 6;
			this.pending = new byte[this.lit_bufsize * 4];
			this._distanceOffset = this.lit_bufsize;
			this._lengthOffset = 3 * this.lit_bufsize;
			this.compressionLevel = level;
			this.compressionStrategy = strategy;
			this.Reset();
			return 0;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00040A10 File Offset: 0x0003EC10
		internal void Reset()
		{
			ZlibCodec codec = this._codec;
			this._codec.TotalBytesOut = 0L;
			codec.TotalBytesIn = 0L;
			this._codec.Message = null;
			this.pendingCount = 0;
			this.nextPending = 0;
			this.Rfc1950BytesEmitted = false;
			this.status = (this.WantRfc1950HeaderBytes ? DeflateManager.INIT_STATE : DeflateManager.BUSY_STATE);
			this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
			this.last_flush = 0;
			this._InitializeTreeData();
			this._InitializeLazyMatch();
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00040AA8 File Offset: 0x0003ECA8
		internal int End()
		{
			if (this.status != DeflateManager.INIT_STATE && this.status != DeflateManager.BUSY_STATE && this.status != DeflateManager.FINISH_STATE)
			{
				return -2;
			}
			this.pending = null;
			this.head = null;
			this.prev = null;
			this.window = null;
			if (this.status != DeflateManager.BUSY_STATE)
			{
				return 0;
			}
			return -3;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00040B0C File Offset: 0x0003ED0C
		private void SetDeflater()
		{
			switch (this.config.Flavor)
			{
			case DeflateFlavor.Store:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateNone);
				return;
			case DeflateFlavor.Fast:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateFast);
				return;
			case DeflateFlavor.Slow:
				this.DeflateFunction = new DeflateManager.CompressFunc(this.DeflateSlow);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00040B70 File Offset: 0x0003ED70
		internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
		{
			int result = 0;
			if (this.compressionLevel != level)
			{
				DeflateManager.Config config = DeflateManager.Config.Lookup(level);
				if (config.Flavor != this.config.Flavor && this._codec.TotalBytesIn != 0L)
				{
					result = this._codec.Deflate(FlushType.Partial);
				}
				this.compressionLevel = level;
				this.config = config;
				this.SetDeflater();
			}
			this.compressionStrategy = strategy;
			return result;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00040BE4 File Offset: 0x0003EDE4
		internal int SetDictionary(byte[] dictionary)
		{
			int num = dictionary.Length;
			int sourceIndex = 0;
			if (dictionary != null)
			{
				if (this.status == DeflateManager.INIT_STATE)
				{
					this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
					if (num < DeflateManager.MIN_MATCH)
					{
						return 0;
					}
					if (num > this.w_size - DeflateManager.MIN_LOOKAHEAD)
					{
						num = this.w_size - DeflateManager.MIN_LOOKAHEAD;
						sourceIndex = dictionary.Length - num;
					}
					Array.Copy(dictionary, sourceIndex, this.window, 0, num);
					this.strstart = num;
					this.block_start = num;
					this.ins_h = (int)(this.window[0] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & byte.MaxValue)) & this.hash_mask);
					for (int i = 0; i <= num - DeflateManager.MIN_MATCH; i++)
					{
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + (DeflateManager.MIN_MATCH - 1)] & byte.MaxValue)) & this.hash_mask);
						this.prev[i & this.w_mask] = this.head[this.ins_h];
						this.head[this.ins_h] = (short)i;
					}
					return 0;
				}
			}
			throw new ZlibException("Stream error.");
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00040D38 File Offset: 0x0003EF38
		internal int Deflate(FlushType flush)
		{
			if (this._codec.OutputBuffer != null && (this._codec.InputBuffer != null || this._codec.AvailableBytesIn == 0))
			{
				if (this.status != DeflateManager.FINISH_STATE || flush == FlushType.Finish)
				{
					if (this._codec.AvailableBytesOut == 0)
					{
						this._codec.Message = DeflateManager._ErrorMessage[7];
						throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
					}
					int num = this.last_flush;
					this.last_flush = (int)flush;
					if (this.status == DeflateManager.INIT_STATE)
					{
						int num2 = DeflateManager.Z_DEFLATED + (this.w_bits - 8 << 4) << 8;
						int num3 = (this.compressionLevel - CompressionLevel.BestSpeed & 255) >> 1;
						if (num3 > 3)
						{
							num3 = 3;
						}
						num2 |= num3 << 6;
						if (this.strstart != 0)
						{
							num2 |= DeflateManager.PRESET_DICT;
						}
						num2 += 31 - num2 % 31;
						this.status = DeflateManager.BUSY_STATE;
						this.pending[this.pendingCount++] = (byte)(num2 >> 8);
						this.pending[this.pendingCount++] = (byte)num2;
						if (this.strstart != 0)
						{
							this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 4278190080U) >> 24);
							this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 16711680U) >> 16);
							this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 65280U) >> 8);
							this.pending[this.pendingCount++] = (byte)(this._codec._Adler32 & 255U);
						}
						this._codec._Adler32 = Adler.Adler32(0U, null, 0, 0);
					}
					if (this.pendingCount != 0)
					{
						this._codec.flush_pending();
						if (this._codec.AvailableBytesOut == 0)
						{
							this.last_flush = -1;
							return 0;
						}
					}
					else if (this._codec.AvailableBytesIn == 0 && flush <= (FlushType)num && flush != FlushType.Finish)
					{
						return 0;
					}
					if (this.status == DeflateManager.FINISH_STATE && this._codec.AvailableBytesIn != 0)
					{
						this._codec.Message = DeflateManager._ErrorMessage[7];
						throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
					}
					if (this._codec.AvailableBytesIn != 0 || this.lookahead != 0 || (flush != FlushType.None && this.status != DeflateManager.FINISH_STATE))
					{
						BlockState blockState = this.DeflateFunction(flush);
						if (blockState == BlockState.FinishStarted || blockState == BlockState.FinishDone)
						{
							this.status = DeflateManager.FINISH_STATE;
						}
						if (blockState != BlockState.NeedMore)
						{
							if (blockState != BlockState.FinishStarted)
							{
								if (blockState != BlockState.BlockDone)
								{
									goto IL_32B;
								}
								if (flush == FlushType.Partial)
								{
									this._tr_align();
								}
								else
								{
									this._tr_stored_block(0, 0, false);
									if (flush == FlushType.Full)
									{
										for (int i = 0; i < this.hash_size; i++)
										{
											this.head[i] = 0;
										}
									}
								}
								this._codec.flush_pending();
								if (this._codec.AvailableBytesOut == 0)
								{
									this.last_flush = -1;
									return 0;
								}
								goto IL_32B;
							}
						}
						if (this._codec.AvailableBytesOut == 0)
						{
							this.last_flush = -1;
						}
						return 0;
					}
					IL_32B:
					if (flush != FlushType.Finish)
					{
						return 0;
					}
					if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
					{
						return 1;
					}
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 4278190080U) >> 24);
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 16711680U) >> 16);
					this.pending[this.pendingCount++] = (byte)((this._codec._Adler32 & 65280U) >> 8);
					this.pending[this.pendingCount++] = (byte)(this._codec._Adler32 & 255U);
					this._codec.flush_pending();
					this.Rfc1950BytesEmitted = true;
					if (this.pendingCount == 0)
					{
						return 1;
					}
					return 0;
				}
			}
			this._codec.Message = DeflateManager._ErrorMessage[4];
			throw new ZlibException(string.Format("Something is fishy. [{0}]", this._codec.Message));
		}

		// Token: 0x040004BC RID: 1212
		private static readonly int MEM_LEVEL_MAX = 9;

		// Token: 0x040004BD RID: 1213
		private static readonly int MEM_LEVEL_DEFAULT = 8;

		// Token: 0x040004BE RID: 1214
		private DeflateManager.CompressFunc DeflateFunction;

		// Token: 0x040004BF RID: 1215
		private static readonly string[] _ErrorMessage = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x040004C0 RID: 1216
		private static readonly int PRESET_DICT = 32;

		// Token: 0x040004C1 RID: 1217
		private static readonly int INIT_STATE = 42;

		// Token: 0x040004C2 RID: 1218
		private static readonly int BUSY_STATE = 113;

		// Token: 0x040004C3 RID: 1219
		private static readonly int FINISH_STATE = 666;

		// Token: 0x040004C4 RID: 1220
		private static readonly int Z_DEFLATED = 8;

		// Token: 0x040004C5 RID: 1221
		private static readonly int STORED_BLOCK = 0;

		// Token: 0x040004C6 RID: 1222
		private static readonly int STATIC_TREES = 1;

		// Token: 0x040004C7 RID: 1223
		private static readonly int DYN_TREES = 2;

		// Token: 0x040004C8 RID: 1224
		private static readonly int Z_BINARY = 0;

		// Token: 0x040004C9 RID: 1225
		private static readonly int Z_ASCII = 1;

		// Token: 0x040004CA RID: 1226
		private static readonly int Z_UNKNOWN = 2;

		// Token: 0x040004CB RID: 1227
		private static readonly int Buf_size = 16;

		// Token: 0x040004CC RID: 1228
		private static readonly int MIN_MATCH = 3;

		// Token: 0x040004CD RID: 1229
		private static readonly int MAX_MATCH = 258;

		// Token: 0x040004CE RID: 1230
		private static readonly int MIN_LOOKAHEAD = DeflateManager.MAX_MATCH + DeflateManager.MIN_MATCH + 1;

		// Token: 0x040004CF RID: 1231
		private static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

		// Token: 0x040004D0 RID: 1232
		private static readonly int END_BLOCK = 256;

		// Token: 0x040004D1 RID: 1233
		internal ZlibCodec _codec;

		// Token: 0x040004D2 RID: 1234
		internal int status;

		// Token: 0x040004D3 RID: 1235
		internal byte[] pending;

		// Token: 0x040004D4 RID: 1236
		internal int nextPending;

		// Token: 0x040004D5 RID: 1237
		internal int pendingCount;

		// Token: 0x040004D6 RID: 1238
		internal sbyte data_type;

		// Token: 0x040004D7 RID: 1239
		internal int last_flush;

		// Token: 0x040004D8 RID: 1240
		internal int w_size;

		// Token: 0x040004D9 RID: 1241
		internal int w_bits;

		// Token: 0x040004DA RID: 1242
		internal int w_mask;

		// Token: 0x040004DB RID: 1243
		internal byte[] window;

		// Token: 0x040004DC RID: 1244
		internal int window_size;

		// Token: 0x040004DD RID: 1245
		internal short[] prev;

		// Token: 0x040004DE RID: 1246
		internal short[] head;

		// Token: 0x040004DF RID: 1247
		internal int ins_h;

		// Token: 0x040004E0 RID: 1248
		internal int hash_size;

		// Token: 0x040004E1 RID: 1249
		internal int hash_bits;

		// Token: 0x040004E2 RID: 1250
		internal int hash_mask;

		// Token: 0x040004E3 RID: 1251
		internal int hash_shift;

		// Token: 0x040004E4 RID: 1252
		internal int block_start;

		// Token: 0x040004E5 RID: 1253
		private DeflateManager.Config config;

		// Token: 0x040004E6 RID: 1254
		internal int match_length;

		// Token: 0x040004E7 RID: 1255
		internal int prev_match;

		// Token: 0x040004E8 RID: 1256
		internal int match_available;

		// Token: 0x040004E9 RID: 1257
		internal int strstart;

		// Token: 0x040004EA RID: 1258
		internal int match_start;

		// Token: 0x040004EB RID: 1259
		internal int lookahead;

		// Token: 0x040004EC RID: 1260
		internal int prev_length;

		// Token: 0x040004ED RID: 1261
		internal CompressionLevel compressionLevel;

		// Token: 0x040004EE RID: 1262
		internal CompressionStrategy compressionStrategy;

		// Token: 0x040004EF RID: 1263
		internal short[] dyn_ltree;

		// Token: 0x040004F0 RID: 1264
		internal short[] dyn_dtree;

		// Token: 0x040004F1 RID: 1265
		internal short[] bl_tree;

		// Token: 0x040004F2 RID: 1266
		internal Tree treeLiterals = new Tree();

		// Token: 0x040004F3 RID: 1267
		internal Tree treeDistances = new Tree();

		// Token: 0x040004F4 RID: 1268
		internal Tree treeBitLengths = new Tree();

		// Token: 0x040004F5 RID: 1269
		internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

		// Token: 0x040004F6 RID: 1270
		internal int[] heap = new int[2 * InternalConstants.L_CODES + 1];

		// Token: 0x040004F7 RID: 1271
		internal int heap_len;

		// Token: 0x040004F8 RID: 1272
		internal int heap_max;

		// Token: 0x040004F9 RID: 1273
		internal sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

		// Token: 0x040004FA RID: 1274
		internal int _lengthOffset;

		// Token: 0x040004FB RID: 1275
		internal int lit_bufsize;

		// Token: 0x040004FC RID: 1276
		internal int last_lit;

		// Token: 0x040004FD RID: 1277
		internal int _distanceOffset;

		// Token: 0x040004FE RID: 1278
		internal int opt_len;

		// Token: 0x040004FF RID: 1279
		internal int static_len;

		// Token: 0x04000500 RID: 1280
		internal int matches;

		// Token: 0x04000501 RID: 1281
		internal int last_eob_len;

		// Token: 0x04000502 RID: 1282
		internal short bi_buf;

		// Token: 0x04000503 RID: 1283
		internal int bi_valid;

		// Token: 0x04000504 RID: 1284
		private bool Rfc1950BytesEmitted;

		// Token: 0x04000505 RID: 1285
		private bool _WantRfc1950HeaderBytes = true;

		// Token: 0x0200010F RID: 271
		// (Invoke) Token: 0x0600074D RID: 1869
		internal delegate BlockState CompressFunc(FlushType flush);

		// Token: 0x02000110 RID: 272
		internal class Config
		{
			// Token: 0x06000750 RID: 1872 RVA: 0x0000C550 File Offset: 0x0000A750
			private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
			{
				this.GoodLength = goodLength;
				this.MaxLazy = maxLazy;
				this.NiceLength = niceLength;
				this.MaxChainLength = maxChainLength;
				this.Flavor = flavor;
			}

			// Token: 0x06000751 RID: 1873 RVA: 0x0000C57D File Offset: 0x0000A77D
			public static DeflateManager.Config Lookup(CompressionLevel level)
			{
				return DeflateManager.Config.Table[(int)level];
			}

			// Token: 0x04000506 RID: 1286
			internal int GoodLength;

			// Token: 0x04000507 RID: 1287
			internal int MaxLazy;

			// Token: 0x04000508 RID: 1288
			internal int NiceLength;

			// Token: 0x04000509 RID: 1289
			internal int MaxChainLength;

			// Token: 0x0400050A RID: 1290
			internal DeflateFlavor Flavor;

			// Token: 0x0400050B RID: 1291
			private static readonly DeflateManager.Config[] Table = new DeflateManager.Config[]
			{
				new DeflateManager.Config(0, 0, 0, 0, DeflateFlavor.Store),
				new DeflateManager.Config(4, 4, 8, 4, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 5, 16, 8, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 6, 32, 32, DeflateFlavor.Fast),
				new DeflateManager.Config(4, 4, 16, 16, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 32, 32, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 16, 128, 128, DeflateFlavor.Slow),
				new DeflateManager.Config(8, 32, 128, 256, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 128, 258, 1024, DeflateFlavor.Slow),
				new DeflateManager.Config(32, 258, 258, 4096, DeflateFlavor.Slow)
			};
		}
	}
}
