using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SmartAssembly.Zip;

namespace SmartAssembly.StringsEncoding
{
	// Token: 0x020000A9 RID: 169
	public sealed class Strings
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000A154 File Offset: 0x00008354
		public static string Get(int int_0)
		{
			int_0 ^= 107396847;
			int_0 -= ChromV266351.Strings.offset;
			if (!ChromV266351.Strings.cacheStrings)
			{
				return ChromV266351.Strings.GetFromResource(int_0);
			}
			return ChromV266351.Strings.GetCachedOrResource(int_0);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0002B7B8 File Offset: 0x000299B8
		public static string GetCachedOrResource(int int_0)
		{
			object obj = ChromV266351.Strings.hashtableLock;
			lock (obj)
			{
				string text;
				ChromV266351.Strings.hashtable.TryGetValue(int_0, out text);
				if (text != null)
				{
					return text;
				}
			}
			return ChromV266351.Strings.GetFromResource(int_0);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0002B808 File Offset: 0x00029A08
		public static string GetFromResource(int int_0)
		{
			byte[] array = ChromV266351.Strings.bytes;
			int index = int_0 + 1;
			int num = array[int_0];
			int num2;
			if ((num & 128) == 0)
			{
				num2 = num;
				if (num2 == 0)
				{
					return string.Empty;
				}
			}
			else if ((num & 64) == 0)
			{
				num2 = ((num & 63) << 8) + (int)ChromV266351.Strings.bytes[index++];
			}
			else
			{
				num2 = ((num & 31) << 24) + ((int)ChromV266351.Strings.bytes[index++] << 16) + ((int)ChromV266351.Strings.bytes[index++] << 8) + (int)ChromV266351.Strings.bytes[index++];
			}
			string result;
			try
			{
				byte[] array2 = Convert.FromBase64String(Encoding.UTF8.GetString(ChromV266351.Strings.bytes, index, num2));
				string text = string.Intern(Encoding.UTF8.GetString(array2, 0, array2.Length));
				if (ChromV266351.Strings.cacheStrings)
				{
					ChromV266351.Strings.CacheString(int_0, text);
				}
				result = text;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0002B8E0 File Offset: 0x00029AE0
		public static void CacheString(int int_0, string string_0)
		{
			try
			{
				object obj = ChromV266351.Strings.hashtableLock;
				lock (obj)
				{
					ChromV266351.Strings.hashtable.Add(int_0, string_0);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0002B930 File Offset: 0x00029B30
		static Strings()
		{
			if (ChromV266351.Strings.MustUseCache == "1")
			{
				ChromV266351.Strings.cacheStrings = true;
				ChromV266351.Strings.hashtable = new Dictionary<int, string>();
			}
			ChromV266351.Strings.offset = Convert.ToInt32(ChromV266351.Strings.OffsetValue);
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("{1084b41b-54c8-4d89-99d4-af1358dc4ba0}"))
			{
				int num = Convert.ToInt32(manifestResourceStream.Length);
				byte[] array = new byte[num];
				manifestResourceStream.Read(array, 0, num);
				ChromV266351.Strings.bytes = ChromV266351.SimpleZip.Unzip(array);
			}
		}

		// Token: 0x04000211 RID: 529
		private static readonly string MustUseCache = "0";

		// Token: 0x04000212 RID: 530
		private static readonly string OffsetValue = "242";

		// Token: 0x04000213 RID: 531
		private static readonly byte[] bytes = null;

		// Token: 0x04000214 RID: 532
		private static readonly Dictionary<int, string> hashtable;

		// Token: 0x04000215 RID: 533
		private static readonly object hashtableLock = new object();

		// Token: 0x04000216 RID: 534
		private static readonly bool cacheStrings = false;

		// Token: 0x04000217 RID: 535
		private static readonly int offset = 0;
	}
}
