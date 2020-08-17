using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000230 RID: 560
	[NullableContext(1)]
	[Nullable(0)]
	internal static class MiscellaneousUtils
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x00009B58 File Offset: 0x00007D58
		[NullableContext(2)]
		[Conditional("DEBUG")]
		public static void Assert([DoesNotReturnIf(false)] bool condition, string message = null)
		{
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005C8D8 File Offset: 0x0005AAD8
		[NullableContext(2)]
		public static bool ValueEquals(object objA, object objB)
		{
			if (objA == objB)
			{
				return true;
			}
			if (objA == null || objB == null)
			{
				return false;
			}
			if (!(objA.GetType() != objB.GetType()))
			{
				return objA.Equals(objB);
			}
			if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
			{
				return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
			}
			return (objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal) && MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture));
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0005C990 File Offset: 0x0005AB90
		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string message2 = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, actualValue);
			return new ArgumentOutOfRangeException(paramName, message2);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005C9C0 File Offset: 0x0005ABC0
		public static string ToString([Nullable(2)] object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			string text = value as string;
			if (text == null)
			{
				return value.ToString();
			}
			return "\"" + text + "\"";
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0005C9F8 File Offset: 0x0005ABF8
		public static int ByteArrayCompare(byte[] a1, byte[] a2)
		{
			int num = a1.Length.CompareTo(a2.Length);
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				int num2 = a1[i].CompareTo(a2[i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return 0;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005CA40 File Offset: 0x0005AC40
		[return: Nullable(2)]
		public static string GetPrefix(string qualifiedName)
		{
			string result;
			string text;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out result, out text);
			return result;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0005CA58 File Offset: 0x0005AC58
		public static string GetLocalName(string qualifiedName)
		{
			string text;
			string result;
			MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out text, out result);
			return result;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0005CA70 File Offset: 0x0005AC70
		public static void GetQualifiedNameParts(string qualifiedName, [Nullable(2)] out string prefix, out string localName)
		{
			int num = qualifiedName.IndexOf(':');
			if (num != -1 && num != 0)
			{
				if (qualifiedName.Length - 1 != num)
				{
					prefix = qualifiedName.Substring(0, num);
					localName = qualifiedName.Substring(num + 1);
					return;
				}
			}
			prefix = null;
			localName = qualifiedName;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0005CAB8 File Offset: 0x0005ACB8
		internal static RegexOptions GetRegexOptions(string optionsText)
		{
			RegexOptions regexOptions = RegexOptions.None;
			foreach (char c in optionsText)
			{
				if (c <= 'm')
				{
					if (c != 'i')
					{
						if (c == 'm')
						{
							regexOptions |= RegexOptions.Multiline;
						}
					}
					else
					{
						regexOptions |= RegexOptions.IgnoreCase;
					}
				}
				else if (c != 's')
				{
					if (c == 'x')
					{
						regexOptions |= RegexOptions.ExplicitCapture;
					}
				}
				else
				{
					regexOptions |= RegexOptions.Singleline;
				}
			}
			return regexOptions;
		}
	}
}
