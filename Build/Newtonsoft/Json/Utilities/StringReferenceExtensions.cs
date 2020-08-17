using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200023F RID: 575
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringReferenceExtensions
	{
		// Token: 0x06001059 RID: 4185 RVA: 0x0005DE6C File Offset: 0x0005C06C
		public static int IndexOf(this StringReference s, char c, int startIndex, int length)
		{
			int num = Array.IndexOf<char>(s.Chars, c, s.StartIndex + startIndex, length);
			if (num == -1)
			{
				return -1;
			}
			return num - s.StartIndex;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0005DEA0 File Offset: 0x0005C0A0
		public static bool StartsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + s.StartIndex])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0005DEF0 File Offset: 0x0005C0F0
		public static bool EndsWith(this StringReference s, string text)
		{
			if (text.Length > s.Length)
			{
				return false;
			}
			char[] chars = s.Chars;
			int num = s.StartIndex + s.Length - text.Length;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != chars[i + num])
				{
					return false;
				}
			}
			return true;
		}
	}
}
