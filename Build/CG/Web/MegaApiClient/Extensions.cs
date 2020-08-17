using System;
using System.Linq;
using System.Text;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000149 RID: 329
	internal static class Extensions
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x0000D4BF File Offset: 0x0000B6BF
		public static string ToBase64(this byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Convert.ToBase64String(data));
			stringBuilder.Replace('+', '-');
			stringBuilder.Replace('/', '_');
			stringBuilder.Replace("=", string.Empty);
			return stringBuilder.ToString();
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0004B168 File Offset: 0x00049368
		public static byte[] FromBase64(this string data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(data);
			stringBuilder.Append(string.Empty.PadRight((4 - data.Length % 4) % 4, '='));
			stringBuilder.Replace('-', '+');
			stringBuilder.Replace('_', '/');
			stringBuilder.Replace(",", string.Empty);
			return Convert.FromBase64String(stringBuilder.ToString());
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0000D4FF File Offset: 0x0000B6FF
		public static string ToUTF8String(this byte[] data)
		{
			return Encoding.UTF8.GetString(data);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000CAA4 File Offset: 0x0000ACA4
		public static byte[] ToBytes(this string data)
		{
			return Encoding.UTF8.GetBytes(data);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0004B1D4 File Offset: 0x000493D4
		public static byte[] ToBytesPassword(this string data)
		{
			uint[] array = new uint[data.Length + 3 >> 2];
			for (int i = 0; i < data.Length; i++)
			{
				array[i >> 2] |= (uint)((uint)data[i] << (24 - (i & 3) * 8 & 31));
			}
			return array.SelectMany(delegate(uint x)
			{
				byte[] bytes = BitConverter.GetBytes(x);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes);
				}
				return bytes;
			}).ToArray<byte>();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0004B24C File Offset: 0x0004944C
		public static T[] CopySubArray<T>(this T[] source, int length, int offset = 0)
		{
			T[] array = new T[length];
			while (--length >= 0)
			{
				if (source.Length > offset + length)
				{
					array[length] = source[offset + length];
				}
			}
			return array;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0004B284 File Offset: 0x00049484
		public static BigInteger FromMPINumber(this byte[] data)
		{
			byte[] array = new byte[((int)data[0] * 256 + (int)data[1] + 7) / 8];
			Array.Copy(data, 2, array, 0, array.Length);
			return new BigInteger(array, -1, 0);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0004B2BC File Offset: 0x000494BC
		public static DateTime ToDateTime(this long seconds)
		{
			return Extensions.EpochStart.AddSeconds((double)seconds).ToLocalTime();
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0004B2E0 File Offset: 0x000494E0
		public static long ToEpoch(this DateTime datetime)
		{
			return (long)datetime.ToUniversalTime().Subtract(Extensions.EpochStart).TotalSeconds;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0004B30C File Offset: 0x0004950C
		public static long DeserializeToLong(this byte[] data, int index, int length)
		{
			byte b = data[index];
			long num = 0L;
			if (b <= 8)
			{
				if ((int)b < length)
				{
					while (b > 0)
					{
						long num2 = num << 8;
						byte b2 = b;
						b = b2 - 1;
						num = num2 + (long)((ulong)data[index + (int)b2]);
					}
					return num;
				}
			}
			throw new ArgumentException("Invalid value");
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0004B354 File Offset: 0x00049554
		public static byte[] SerializeToBytes(this long data)
		{
			byte[] array = new byte[9];
			byte b = 0;
			while (data != 0L)
			{
				array[(int)(b += 1)] = (byte)data;
				data >>= 8;
			}
			array[0] = b;
			Array.Resize<byte>(ref array, (int)(array[0] + 1));
			return array;
		}

		// Token: 0x04000690 RID: 1680
		private static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);
	}
}
