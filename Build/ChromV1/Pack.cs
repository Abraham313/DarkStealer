using System;

namespace ChromV1
{
	// Token: 0x0200007C RID: 124
	internal sealed class Pack
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x00008754 File Offset: 0x00006954
		private Pack()
		{
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00009DF5 File Offset: 0x00007FF5
		internal static void UInt32_To_BE(uint uint_0, byte[] byte_0, int int_0)
		{
			byte_0[int_0] = (byte)(uint_0 >> 24);
			byte_0[++int_0] = (byte)(uint_0 >> 16);
			byte_0[++int_0] = (byte)(uint_0 >> 8);
			byte_0[++int_0] = (byte)uint_0;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00009E22 File Offset: 0x00008022
		internal static uint BE_To_UInt32(byte[] byte_0, int int_0)
		{
			return (uint)((int)byte_0[int_0] << 24 | (int)byte_0[++int_0] << 16 | (int)byte_0[++int_0] << 8 | (int)byte_0[++int_0]);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00009E4A File Offset: 0x0000804A
		internal static void UInt32_To_LE(uint uint_0, byte[] byte_0, int int_0)
		{
			byte_0[int_0] = (byte)uint_0;
			byte_0[++int_0] = (byte)(uint_0 >> 8);
			byte_0[++int_0] = (byte)(uint_0 >> 16);
			byte_0[++int_0] = (byte)(uint_0 >> 24);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00009E77 File Offset: 0x00008077
		internal static uint LE_To_UInt32(byte[] byte_0, int int_0)
		{
			return (uint)((int)byte_0[int_0] | (int)byte_0[++int_0] << 8 | (int)byte_0[++int_0] << 16 | (int)byte_0[++int_0] << 24);
		}
	}
}
