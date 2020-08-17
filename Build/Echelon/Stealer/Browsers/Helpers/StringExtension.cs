using System;
using System.Globalization;
using System.Text;

namespace Echelon.Stealer.Browsers.Helpers
{
	// Token: 0x02000037 RID: 55
	public static class StringExtension
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00008E4D File Offset: 0x0000704D
		public static T ForceTo<T>(this object @this)
		{
			return (T)((object)Convert.ChangeType(@this, typeof(T)));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00008E64 File Offset: 0x00007064
		public static string Remove(this string input, string strToRemove)
		{
			if (input.IsNullOrEmpty())
			{
				return null;
			}
			return input.Replace(strToRemove, "");
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008E7C File Offset: 0x0000707C
		public static string Left(this string input, int minusRight = 1)
		{
			if (!input.IsNullOrEmpty() && input.Length > minusRight)
			{
				return input.Substring(0, input.Length - minusRight);
			}
			return null;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008EA0 File Offset: 0x000070A0
		public static CultureInfo ToCultureInfo(this string culture, CultureInfo defaultCulture)
		{
			if (!culture.IsNullOrEmpty())
			{
				return defaultCulture;
			}
			return new CultureInfo(culture);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008EB2 File Offset: 0x000070B2
		public static string ToCamelCasing(this string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);
			}
			return value;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		public static double? ToDouble(this string value, string culture = "en-US")
		{
			double? result;
			try
			{
				result = new double?(double.Parse(value, new CultureInfo(culture)));
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0001EDBC File Offset: 0x0001CFBC
		public static bool? ToBoolean(this string value)
		{
			bool value2 = false;
			if (bool.TryParse(value, out value2))
			{
				return new bool?(value2);
			}
			return null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		public static int? ToInt32(this string value)
		{
			int value2 = 0;
			if (int.TryParse(value, out value2))
			{
				return new int?(value2);
			}
			return null;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0001EE14 File Offset: 0x0001D014
		public static long? ToInt64(this string value)
		{
			long value2 = 0L;
			if (long.TryParse(value, out value2))
			{
				return new long?(value2);
			}
			return null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0001EE48 File Offset: 0x0001D048
		public static string AddQueyString(this string url, string queryStringKey, string queryStringValue)
		{
			string text = (url.Split(new char[]
			{
				'?'
			}).Length <= 1) ? "?" : "&";
			return string.Concat(new string[]
			{
				url,
				text,
				queryStringKey,
				"=",
				queryStringValue
			});
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00008EDF File Offset: 0x000070DF
		public static string FormatFirstLetterUpperCase(this string value, string culture = "en-US")
		{
			return CultureInfo.GetCultureInfo(culture).TextInfo.ToTitleCase(value);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
		public static string FillLeftWithZeros(this string value, int decimalDigits)
		{
			if (!string.IsNullOrEmpty(value))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(value);
				string[] array = value.Split(new char[]
				{
					','
				});
				for (int i = array[array.Length - 1].Length; i < decimalDigits; i++)
				{
					stringBuilder.Append("0");
				}
				value = stringBuilder.ToString();
			}
			return value;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0001EF04 File Offset: 0x0001D104
		public static string FormatWithDecimalDigits(this string value, bool removeCurrencySymbol, bool returnZero, int? decimalDigits)
		{
			if (value.IsNullOrEmpty())
			{
				return value;
			}
			if (!value.IndexOf(",").Equals(-1))
			{
				string[] array = value.Split(new char[]
				{
					','
				});
				if (array.Length.Equals(2) && array[1].Length > 0)
				{
					value = array[0] + "," + array[1].Substring(0, (array[1].Length >= decimalDigits.Value) ? decimalDigits.Value : array[1].Length);
				}
			}
			if (decimalDigits == null)
			{
				return value;
			}
			return value.FillLeftWithZeros(decimalDigits.Value);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00008EF2 File Offset: 0x000070F2
		public static string FormatWithoutDecimalDigits(this string value, bool removeCurrencySymbol, bool returnZero, int? decimalDigits, CultureInfo culture)
		{
			if (removeCurrencySymbol)
			{
				value = value.Remove(culture.NumberFormat.CurrencySymbol).Trim();
			}
			return value;
		}
	}
}
