using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001C4 RID: 452
	[NullableContext(1)]
	[Nullable(0)]
	internal struct JsonPosition
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x0000ED7A File Offset: 0x0000CF7A
		public JsonPosition(JsonContainerType type)
		{
			this.Type = type;
			this.HasIndex = JsonPosition.TypeHasIndex(type);
			this.Position = -1;
			this.PropertyName = null;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0004ECCC File Offset: 0x0004CECC
		internal int CalculateLength()
		{
			JsonContainerType type = this.Type;
			if (type == JsonContainerType.Object)
			{
				return this.PropertyName.Length + 5;
			}
			if (type - JsonContainerType.Array > 1)
			{
				throw new ArgumentOutOfRangeException("Type");
			}
			return MathUtils.IntLength((ulong)((long)this.Position)) + 2;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004ED14 File Offset: 0x0004CF14
		[NullableContext(2)]
		internal void WriteTo([Nullable(1)] StringBuilder sb, ref StringWriter writer, ref char[] buffer)
		{
			JsonContainerType type = this.Type;
			if (type != JsonContainerType.Object)
			{
				if (type - JsonContainerType.Array > 1)
				{
					return;
				}
				sb.Append('[');
				sb.Append(this.Position);
				sb.Append(']');
				return;
			}
			else
			{
				string propertyName = this.PropertyName;
				if (propertyName.IndexOfAny(JsonPosition.SpecialCharacters) != -1)
				{
					sb.Append("['");
					if (writer == null)
					{
						writer = new StringWriter(sb);
					}
					JavaScriptUtils.WriteEscapedJavaScriptString(writer, propertyName, '\'', false, JavaScriptUtils.SingleQuoteCharEscapeFlags, StringEscapeHandling.Default, null, ref buffer);
					sb.Append("']");
					return;
				}
				if (sb.Length > 0)
				{
					sb.Append('.');
				}
				sb.Append(propertyName);
				return;
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0000ED9D File Offset: 0x0000CF9D
		internal static bool TypeHasIndex(JsonContainerType type)
		{
			return type == JsonContainerType.Array || type == JsonContainerType.Constructor;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0004EDBC File Offset: 0x0004CFBC
		internal static string BuildPath(List<JsonPosition> positions, JsonPosition? currentPosition)
		{
			int num = 0;
			if (positions != null)
			{
				for (int i = 0; i < positions.Count; i++)
				{
					num += positions[i].CalculateLength();
				}
			}
			if (currentPosition != null)
			{
				num += currentPosition.GetValueOrDefault().CalculateLength();
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			StringWriter stringWriter = null;
			char[] array = null;
			if (positions != null)
			{
				foreach (JsonPosition jsonPosition in positions)
				{
					jsonPosition.WriteTo(stringBuilder, ref stringWriter, ref array);
				}
			}
			if (currentPosition != null)
			{
				currentPosition.GetValueOrDefault().WriteTo(stringBuilder, ref stringWriter, ref array);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0004EE88 File Offset: 0x0004D088
		internal static string FormatMessage([Nullable(2)] IJsonLineInfo lineInfo, string path, string message)
		{
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
			{
				message = message.Trim();
				if (!message.EndsWith('.'))
				{
					message += ".";
				}
				message += " ";
			}
			message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
			}
			message += ".";
			return message;
		}

		// Token: 0x040007C7 RID: 1991
		private static readonly char[] SpecialCharacters = new char[]
		{
			'.',
			' ',
			'\'',
			'/',
			'"',
			'[',
			']',
			'(',
			')',
			'\t',
			'\n',
			'\r',
			'\f',
			'\b',
			'\\',
			'\u0085',
			'\u2028',
			'\u2029'
		};

		// Token: 0x040007C8 RID: 1992
		internal JsonContainerType Type;

		// Token: 0x040007C9 RID: 1993
		internal int Position;

		// Token: 0x040007CA RID: 1994
		[Nullable(2)]
		internal string PropertyName;

		// Token: 0x040007CB RID: 1995
		internal bool HasIndex;
	}
}
