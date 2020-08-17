using System;
using System.Text;
using SmartAssembly.StringsEncoding;

namespace ChromV1
{
	// Token: 0x0200006F RID: 111
	public sealed class Arrays
	{
		// Token: 0x0600025A RID: 602 RVA: 0x00008754 File Offset: 0x00006954
		private Arrays()
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009BFE File Offset: 0x00007DFE
		public static bool AreEqual(bool[] a, bool[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009C15 File Offset: 0x00007E15
		public static bool AreEqual(char[] a, char[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009C2C File Offset: 0x00007E2C
		public static bool AreEqual(byte[] a, byte[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009C43 File Offset: 0x00007E43
		[Obsolete("Use 'AreEqual' method instead")]
		public static bool AreSame(byte[] a, byte[] b)
		{
			return Arrays.AreEqual(a, b);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00023AD0 File Offset: 0x00021CD0
		public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			int num2 = 0;
			while (num != 0)
			{
				num--;
				num2 |= (int)(a[num] ^ b[num]);
			}
			return num2 == 0;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009C4C File Offset: 0x00007E4C
		public static bool AreEqual(int[] a, int[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00023B04 File Offset: 0x00021D04
		private static bool HaveSameContents(bool[] bool_0, bool[] bool_1)
		{
			int num = bool_0.Length;
			if (num != bool_1.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (bool_0[num] != bool_1[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00023B34 File Offset: 0x00021D34
		private static bool HaveSameContents(char[] char_0, char[] char_1)
		{
			int num = char_0.Length;
			if (num != char_1.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (char_0[num] != char_1[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00023B04 File Offset: 0x00021D04
		private static bool HaveSameContents(byte[] byte_0, byte[] byte_1)
		{
			int num = byte_0.Length;
			if (num != byte_1.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (byte_0[num] != byte_1[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00023B64 File Offset: 0x00021D64
		private static bool HaveSameContents(int[] int_0, int[] int_1)
		{
			int num = int_0.Length;
			if (num != int_1.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (int_0[num] != int_1[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00023B94 File Offset: 0x00021D94
		public static string ToString(object[] a)
		{
			StringBuilder stringBuilder = new StringBuilder(91);
			if (a.Length != 0)
			{
				stringBuilder.Append(a[0]);
				for (int i = 1; i < a.Length; i++)
				{
					stringBuilder.Append(Strings.Get(107396908)).Append(a[i]);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00023BF0 File Offset: 0x00021DF0
		public static int GetHashCode(byte[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009C63 File Offset: 0x00007E63
		public static byte[] Clone(byte[] data)
		{
			if (data != null)
			{
				return (byte[])data.Clone();
			}
			return null;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009C75 File Offset: 0x00007E75
		public static int[] Clone(int[] data)
		{
			if (data != null)
			{
				return (int[])data.Clone();
			}
			return null;
		}
	}
}
