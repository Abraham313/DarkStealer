using System;

namespace ChromV1
{
	// Token: 0x02000078 RID: 120
	internal abstract class GcmUtilities
	{
		// Token: 0x06000299 RID: 665 RVA: 0x00009D64 File Offset: 0x00007F64
		internal static uint[] AsUints(byte[] byte_0)
		{
			return new uint[]
			{
				Pack.BE_To_UInt32(byte_0, 0),
				Pack.BE_To_UInt32(byte_0, 4),
				Pack.BE_To_UInt32(byte_0, 8),
				Pack.BE_To_UInt32(byte_0, 12)
			};
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00009D95 File Offset: 0x00007F95
		internal static void MultiplyP(uint[] uint_0)
		{
			bool flag = (uint_0[3] & 1U) > 0U;
			GcmUtilities.ShiftRight(uint_0);
			if (flag)
			{
				uint_0[0] ^= 3774873600U;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000269F4 File Offset: 0x00024BF4
		internal static void MultiplyP8(uint[] uint_0)
		{
			uint num = uint_0[3];
			GcmUtilities.ShiftRightN(uint_0, 8);
			for (int i = 7; i >= 0; i--)
			{
				if (((ulong)num & (ulong)(1L << (i & 31))) != 0UL)
				{
					uint_0[0] ^= 3774873600U >> 7 - i;
				}
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00026A3C File Offset: 0x00024C3C
		internal static void ShiftRight(uint[] uint_0)
		{
			int num = 0;
			uint num2 = 0U;
			for (;;)
			{
				uint num3 = uint_0[num];
				uint_0[num] = (num3 >> 1 | num2);
				if (++num == 4)
				{
					break;
				}
				num2 = num3 << 31;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00026A6C File Offset: 0x00024C6C
		internal static void ShiftRightN(uint[] uint_0, int int_0)
		{
			int num = 0;
			uint num2 = 0U;
			for (;;)
			{
				uint num3 = uint_0[num];
				uint_0[num] = (num3 >> int_0 | num2);
				if (++num == 4)
				{
					break;
				}
				num2 = num3 << 32 - int_0;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00026AA4 File Offset: 0x00024CA4
		internal static void Xor(byte[] byte_0, byte[] byte_1)
		{
			for (int i = 15; i >= 0; i--)
			{
				int num = i;
				byte_0[num] ^= byte_1[i];
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00026AD0 File Offset: 0x00024CD0
		internal static void Xor(uint[] uint_0, uint[] uint_1)
		{
			for (int i = 3; i >= 0; i--)
			{
				uint_0[i] ^= uint_1[i];
			}
		}
	}
}
