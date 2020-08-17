using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x0200012D RID: 301
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class CRC32
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0000CEE7 File Offset: 0x0000B0E7
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0000CEEF File Offset: 0x0000B0EF
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0000CEF8 File Offset: 0x0000B0F8
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000471D4 File Offset: 0x000453D4
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, 8192);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000CF02 File Offset: 0x0000B102
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000CF0C File Offset: 0x0000B10C
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((UIntPtr)((W ^ (uint)B) & 255U))] ^ W >> 8);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00047274 File Offset: 0x00045474
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
				}
				else
				{
					uint num3 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num3)]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0004730C File Offset: 0x0004550C
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num)]);
				return;
			}
			uint num2 = (this._register & 255U) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00047370 File Offset: 0x00045570
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num)]);
				}
				else
				{
					uint num2 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
				}
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000473E0 File Offset: 0x000455E0
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765U) << 1 | (data >> 1 & 1431655765U);
			num = ((num & 858993459U) << 2 | (num >> 2 & 858993459U));
			num = ((num & 252645135U) << 4 | (num >> 4 & 252645135U));
			return num << 24 | (num & 65280U) << 8 | (num >> 8 & 65280U) | num >> 24;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0004744C File Offset: 0x0004564C
		private static byte ReverseBits(byte data)
		{
			uint num = (uint)data * 131586U;
			uint num2 = num & 17055760U;
			uint num3 = num << 2 & 34111520U;
			return (byte)(16781313U * (num2 + num3) >> 24);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00047480 File Offset: 0x00045680
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1U) == 1U)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000474F8 File Offset: 0x000456F8
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0U;
			int num2 = 0;
			while (vec != 0U)
			{
				if ((vec & 1U) == 1U)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00047524 File Offset: 0x00045724
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0004754C File Offset: 0x0004574C
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1U;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0U)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0U);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000CF23 File Offset: 0x0000B123
		public CRC32() : this(false)
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000CF2C File Offset: 0x0000B12C
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0000CF3A File Offset: 0x0000B13A
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0000CF5D File Offset: 0x0000B15D
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x04000632 RID: 1586
		private const int BUFFER_SIZE = 8192;

		// Token: 0x04000633 RID: 1587
		private uint dwPolynomial;

		// Token: 0x04000634 RID: 1588
		private long _TotalBytesRead;

		// Token: 0x04000635 RID: 1589
		private bool reverseBits;

		// Token: 0x04000636 RID: 1590
		private uint[] crc32Table;

		// Token: 0x04000637 RID: 1591
		private uint _register = uint.MaxValue;
	}
}
