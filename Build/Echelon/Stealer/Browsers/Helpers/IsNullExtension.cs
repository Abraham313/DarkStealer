using System;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000033 RID: 51
	public static class IsNullExtension
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00008D15 File Offset: 0x00006F15
		public static bool IsNotNull<T>(this T data)
		{
			return data != null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00008D20 File Offset: 0x00006F20
		public static string IsNull(this string value, string defaultValue)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}
			return defaultValue;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00008D2D File Offset: 0x00006F2D
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008D35 File Offset: 0x00006F35
		public static bool IsNull(this bool? value, bool def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008D49 File Offset: 0x00006F49
		public static T IsNull<T>(this T value) where T : class
		{
			if (value == null)
			{
				return Activator.CreateInstance<T>();
			}
			return value;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008D5A File Offset: 0x00006F5A
		public static T IsNull<T>(this T value, T def) where T : class
		{
			if (value != null)
			{
				return value;
			}
			if (def == null)
			{
				return Activator.CreateInstance<T>();
			}
			return def;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008D75 File Offset: 0x00006F75
		public static int IsNull(this int? value, int def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008D89 File Offset: 0x00006F89
		public static long IsNull(this long? value, long def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008D9D File Offset: 0x00006F9D
		public static double IsNull(this double? value, double def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00008DB1 File Offset: 0x00006FB1
		public static DateTime IsNull(this DateTime? value, DateTime def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008DC5 File Offset: 0x00006FC5
		public static Guid IsNull(this Guid? value, Guid def)
		{
			if (value != null)
			{
				return value.Value;
			}
			return def;
		}
	}
}
