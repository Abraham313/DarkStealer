using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SmartAssembly.Zip;

namespace SmartAssembly.StringsEncoding
{
	// Token: 0x02000089 RID: 137
	public sealed class Strings
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x00009EE6 File Offset: 0x000080E6
		public static string Get(int int_0)
		{
			int_0 ^= 107396847;
			int_0 -= Strings.offset;
			if (!Strings.cacheStrings)
			{
				return Strings.GetFromResource(int_0);
			}
			return Strings.GetCachedOrResource(int_0);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00027DB0 File Offset: 0x00025FB0
		public static string GetCachedOrResource(int int_0)
		{
			object obj = Strings.hashtableLock;
			lock (obj)
			{
				string text;
				Strings.hashtable.TryGetValue(int_0, out text);
				if (text != null)
				{
					return text;
				}
			}
			return Strings.GetFromResource(int_0);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00027E00 File Offset: 0x00026000
		public static string GetFromResource(int int_0)
		{
			byte[] array = Strings.bytes;
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
				num2 = ((num & 63) << 8) + (int)Strings.bytes[index++];
			}
			else
			{
				num2 = ((num & 31) << 24) + ((int)Strings.bytes[index++] << 16) + ((int)Strings.bytes[index++] << 8) + (int)Strings.bytes[index++];
			}
			string result;
			try
			{
				byte[] array2 = Convert.FromBase64String(Encoding.UTF8.GetString(Strings.bytes, index, num2));
				string text = string.Intern(Encoding.UTF8.GetString(array2, 0, array2.Length));
				if (Strings.cacheStrings)
				{
					Strings.CacheString(int_0, text);
				}
				result = text;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00027ED8 File Offset: 0x000260D8
		public static void CacheString(int int_0, string string_0)
		{
			try
			{
				object obj = Strings.hashtableLock;
				lock (obj)
				{
					Strings.hashtable.Add(int_0, string_0);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00027F28 File Offset: 0x00026128
		static Strings()
		{
			if (Strings.MustUseCache == "1")
			{
				Strings.cacheStrings = true;
				Strings.hashtable = new Dictionary<int, string>();
			}
			Strings.offset = Convert.ToInt32(Strings.OffsetValue);
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("{e4775d96-dfa1-43fa-97d8-8f29405b74d4}"))
			{
				int num = Convert.ToInt32(manifestResourceStream.Length);
				byte[] array = new byte[num];
				manifestResourceStream.Read(array, 0, num);
				Strings.bytes = SimpleZip.Unzip(array);
			}
		}

		// Token: 0x04000194 RID: 404
		private static readonly string MustUseCache = "0";

		// Token: 0x04000195 RID: 405
		private static readonly string OffsetValue = "240";

		// Token: 0x04000196 RID: 406
		private static readonly byte[] bytes = null;

		// Token: 0x04000197 RID: 407
		private static readonly Dictionary<int, string> hashtable;

		// Token: 0x04000198 RID: 408
		private static readonly object hashtableLock = new object();

		// Token: 0x04000199 RID: 409
		private static readonly bool cacheStrings = false;

		// Token: 0x0400019A RID: 410
		private static readonly int offset = 0;
	}
}
