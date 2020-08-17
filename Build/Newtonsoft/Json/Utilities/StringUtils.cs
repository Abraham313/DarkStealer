using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000240 RID: 576
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringUtils
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x00008D2D File Offset: 0x00006F2D
		[NullableContext(2)]
		public static bool IsNullOrEmpty([NotNullWhen(false)] string value)
		{
			return string.IsNullOrEmpty(value);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000124C7 File Offset: 0x000106C7
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0
			});
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000124DA File Offset: 0x000106DA
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0, [Nullable(2)] object arg1)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1
			});
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x000124F1 File Offset: 0x000106F1
		public static string FormatWith(this string format, IFormatProvider provider, [Nullable(2)] object arg0, [Nullable(2)] object arg1, [Nullable(2)] object arg2)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2
			});
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0001250D File Offset: 0x0001070D
		[NullableContext(2)]
		[return: Nullable(1)]
		public static string FormatWith([Nullable(1)] this string format, [Nullable(1)] IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0001252E File Offset: 0x0001072E
		private static string FormatWith(this string format, IFormatProvider provider, [Nullable(new byte[]
		{
			1,
			2
		})] params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0005DF54 File Offset: 0x0005C154
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00012543 File Offset: 0x00010743
		public static StringWriter CreateStringWriter(int capacity)
		{
			return new StringWriter(new StringBuilder(capacity), CultureInfo.InvariantCulture);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0005DF9C File Offset: 0x0005C19C
		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			buffer[3] = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			buffer[4] = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			buffer[5] = MathUtils.IntToHex((int)(c & '\u000f'));
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0005DFEC File Offset: 0x0005C1EC
		public static TSource ForgivingCaseSensitiveFind<[Nullable(2)] TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> source2 = from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)
			select s;
			if (source2.Count<TSource>() <= 1)
			{
				return source2.SingleOrDefault<TSource>();
			}
			return (from s in source
			where string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)
			select s).SingleOrDefault<TSource>();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0005E068 File Offset: 0x0005C268
		public static string ToCamelCase(string s)
		{
			if (!StringUtils.IsNullOrEmpty(s) && char.IsUpper(s[0]))
			{
				char[] array = s.ToCharArray();
				int i = 0;
				while (i < array.Length)
				{
					if (i != 1 || char.IsUpper(array[i]))
					{
						bool flag = i + 1 < array.Length;
						if (i <= 0 || !flag || char.IsUpper(array[i + 1]))
						{
							array[i] = StringUtils.ToLower(array[i]);
							i++;
							continue;
						}
						if (char.IsSeparator(array[i + 1]))
						{
							array[i] = StringUtils.ToLower(array[i]);
						}
					}
					IL_7A:
					return new string(array);
				}
				goto IL_7A;
			}
			return s;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00012555 File Offset: 0x00010755
		private static char ToLower(char c)
		{
			c = char.ToLower(c, CultureInfo.InvariantCulture);
			return c;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00012565 File Offset: 0x00010765
		public static string ToSnakeCase(string s)
		{
			return StringUtils.ToSeparatedCase(s, '_');
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0001256F File Offset: 0x0001076F
		public static string ToKebabCase(string s)
		{
			return StringUtils.ToSeparatedCase(s, '-');
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0005E0F8 File Offset: 0x0005C2F8
		private static string ToSeparatedCase(string s, char separator)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringUtils.SeparatedCaseState separatedCaseState = StringUtils.SeparatedCaseState.Start;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ')
				{
					if (separatedCaseState != StringUtils.SeparatedCaseState.Start)
					{
						separatedCaseState = StringUtils.SeparatedCaseState.NewWord;
					}
				}
				else if (char.IsUpper(s[i]))
				{
					switch (separatedCaseState)
					{
					case StringUtils.SeparatedCaseState.Lower:
					case StringUtils.SeparatedCaseState.NewWord:
						stringBuilder.Append(separator);
						break;
					case StringUtils.SeparatedCaseState.Upper:
					{
						bool flag = i + 1 < s.Length;
						if (i > 0 && flag)
						{
							char c = s[i + 1];
							if (!char.IsUpper(c) && c != separator)
							{
								stringBuilder.Append(separator);
							}
						}
						break;
					}
					}
					char value = char.ToLower(s[i], CultureInfo.InvariantCulture);
					stringBuilder.Append(value);
					separatedCaseState = StringUtils.SeparatedCaseState.Upper;
				}
				else if (s[i] == separator)
				{
					stringBuilder.Append(separator);
					separatedCaseState = StringUtils.SeparatedCaseState.Start;
				}
				else
				{
					if (separatedCaseState == StringUtils.SeparatedCaseState.NewWord)
					{
						stringBuilder.Append(separator);
					}
					stringBuilder.Append(s[i]);
					separatedCaseState = StringUtils.SeparatedCaseState.Lower;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00012579 File Offset: 0x00010779
		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00012581 File Offset: 0x00010781
		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00012589 File Offset: 0x00010789
		public static bool StartsWith(this string source, char value)
		{
			return source.Length > 0 && source[0] == value;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x000125A0 File Offset: 0x000107A0
		public static bool EndsWith(this string source, char value)
		{
			return source.Length > 0 && source[source.Length - 1] == value;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0005E204 File Offset: 0x0005C404
		public static string Trim(this string s, int start, int length)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = start + length - 1;
			if (num >= s.Length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			while (start < num)
			{
				if (!char.IsWhiteSpace(s[start]))
				{
					IL_6C:
					while (num >= start && char.IsWhiteSpace(s[num]))
					{
						num--;
					}
					return s.Substring(start, num - start + 1);
				}
				start++;
			}
			goto IL_6C;
		}

		// Token: 0x04000A11 RID: 2577
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x04000A12 RID: 2578
		public const string Empty = "";

		// Token: 0x04000A13 RID: 2579
		public const char CarriageReturn = '\r';

		// Token: 0x04000A14 RID: 2580
		public const char LineFeed = '\n';

		// Token: 0x04000A15 RID: 2581
		public const char Tab = '\t';

		// Token: 0x02000241 RID: 577
		[NullableContext(0)]
		private enum SeparatedCaseState
		{
			// Token: 0x04000A17 RID: 2583
			Start,
			// Token: 0x04000A18 RID: 2584
			Lower,
			// Token: 0x04000A19 RID: 2585
			Upper,
			// Token: 0x04000A1A RID: 2586
			NewWord
		}
	}
}
