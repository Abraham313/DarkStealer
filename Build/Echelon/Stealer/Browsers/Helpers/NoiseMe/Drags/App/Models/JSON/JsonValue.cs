using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON
{
	// Token: 0x0200003F RID: 63
	public abstract class JsonValue : IEnumerable
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009411 File Offset: 0x00007611
		public virtual int Count
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600016C RID: 364
		public abstract JsonType JsonType { get; }

		// Token: 0x17000023 RID: 35
		public virtual JsonValue this[int index]
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000024 RID: 36
		public virtual JsonValue this[string key]
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00009411 File Offset: 0x00007611
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009418 File Offset: 0x00007618
		public static JsonValue Load(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			return JsonValue.Load(new StreamReader(stream, true));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00009434 File Offset: 0x00007634
		public static JsonValue Load(TextReader textReader)
		{
			if (textReader == null)
			{
				throw new ArgumentNullException("textReader");
			}
			return JsonValue.ToJsonValue<object>(new JavaScriptReader(textReader).Read());
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00009454 File Offset: 0x00007654
		private static IEnumerable<KeyValuePair<string, JsonValue>> ToJsonPairEnumerable(IEnumerable<KeyValuePair<string, object>> kvpc)
		{
			JsonValue.<ToJsonPairEnumerable>d__14 <ToJsonPairEnumerable>d__ = new JsonValue.<ToJsonPairEnumerable>d__14(-2);
			<ToJsonPairEnumerable>d__.<>3__kvpc = kvpc;
			return <ToJsonPairEnumerable>d__;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00009464 File Offset: 0x00007664
		private static IEnumerable<JsonValue> ToJsonValueEnumerable(IEnumerable arr)
		{
			JsonValue.<ToJsonValueEnumerable>d__15 <ToJsonValueEnumerable>d__ = new JsonValue.<ToJsonValueEnumerable>d__15(-2);
			<ToJsonValueEnumerable>d__.<>3__arr = arr;
			return <ToJsonValueEnumerable>d__;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0001FB24 File Offset: 0x0001DD24
		public static JsonValue ToJsonValue<T>(T ret)
		{
			if (ret == null)
			{
				return null;
			}
			T t;
			if ((t = ret) is bool)
			{
				return new JsonPrimitive((bool)((object)t));
			}
			if ((t = ret) is byte)
			{
				return new JsonPrimitive((byte)((object)t));
			}
			if ((t = ret) is char)
			{
				return new JsonPrimitive((char)((object)t));
			}
			if ((t = ret) is decimal)
			{
				return new JsonPrimitive((decimal)((object)t));
			}
			if ((t = ret) is double)
			{
				return new JsonPrimitive((double)((object)t));
			}
			if ((t = ret) is float)
			{
				return new JsonPrimitive((float)((object)t));
			}
			if ((t = ret) is int)
			{
				return new JsonPrimitive((int)((object)t));
			}
			if ((t = ret) is long)
			{
				return new JsonPrimitive((long)((object)t));
			}
			if ((t = ret) is sbyte)
			{
				return new JsonPrimitive((sbyte)((object)t));
			}
			if ((t = ret) is short)
			{
				return new JsonPrimitive((short)((object)t));
			}
			string value;
			if ((value = (ret as string)) != null)
			{
				return new JsonPrimitive(value);
			}
			if ((t = ret) is uint)
			{
				return new JsonPrimitive((uint)((object)t));
			}
			if ((t = ret) is ulong)
			{
				return new JsonPrimitive((ulong)((object)t));
			}
			if ((t = ret) is ushort)
			{
				return new JsonPrimitive((ushort)((object)t));
			}
			if ((t = ret) is DateTime)
			{
				return new JsonPrimitive((DateTime)((object)t));
			}
			if ((t = ret) is DateTimeOffset)
			{
				return new JsonPrimitive((DateTimeOffset)((object)t));
			}
			if ((t = ret) is Guid)
			{
				return new JsonPrimitive((Guid)((object)t));
			}
			if ((t = ret) is TimeSpan)
			{
				return new JsonPrimitive((TimeSpan)((object)t));
			}
			Uri value2;
			if ((value2 = (ret as Uri)) != null)
			{
				return new JsonPrimitive(value2);
			}
			IEnumerable<KeyValuePair<string, object>> enumerable = ret as IEnumerable<KeyValuePair<string, object>>;
			if (enumerable != null)
			{
				return new JsonObject(JsonValue.ToJsonPairEnumerable(enumerable));
			}
			IEnumerable enumerable2 = ret as IEnumerable;
			if (enumerable2 != null)
			{
				return new JsonArray(JsonValue.ToJsonValueEnumerable(enumerable2));
			}
			if (!(ret is IEnumerable))
			{
				PropertyInfo[] properties = ret.GetType().GetProperties();
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				foreach (PropertyInfo propertyInfo in properties)
				{
					dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(ret, null).IsNull("null"));
				}
				if (dictionary.Count > 0)
				{
					return new JsonObject(JsonValue.ToJsonPairEnumerable(dictionary));
				}
			}
			throw new NotSupportedException(string.Format("Unexpected parser return type: {0}", ret.GetType()));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00009474 File Offset: 0x00007674
		public static JsonValue Parse(string jsonString)
		{
			if (jsonString == null)
			{
				throw new ArgumentNullException("jsonString");
			}
			return JsonValue.Load(new StringReader(jsonString));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00009411 File Offset: 0x00007611
		public virtual bool ContainsKey(string key)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000948F File Offset: 0x0000768F
		public virtual void Save(Stream stream, bool parsing)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.Save(new StreamWriter(stream), parsing);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000094AC File Offset: 0x000076AC
		public virtual void Save(TextWriter textWriter, bool parsing)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this.Savepublic(textWriter, parsing);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001FE60 File Offset: 0x0001E060
		private void Savepublic(TextWriter w, bool saving)
		{
			switch (this.JsonType)
			{
			case JsonType.String:
				if (saving)
				{
					w.Write('"');
				}
				w.Write(this.EscapeString(((JsonPrimitive)this).GetFormattedString()));
				if (saving)
				{
					w.Write('"');
					return;
				}
				return;
			case JsonType.Object:
			{
				w.Write('{');
				bool flag = false;
				foreach (KeyValuePair<string, JsonValue> keyValuePair in ((JsonObject)this))
				{
					if (flag)
					{
						w.Write(", ");
					}
					w.Write('"');
					w.Write(this.EscapeString(keyValuePair.Key));
					w.Write("\": ");
					if (keyValuePair.Value == null)
					{
						w.Write("null");
					}
					else
					{
						keyValuePair.Value.Savepublic(w, saving);
					}
					flag = true;
				}
				w.Write('}');
				return;
			}
			case JsonType.Array:
			{
				w.Write('[');
				bool flag2 = false;
				foreach (JsonValue jsonValue in ((IEnumerable<JsonValue>)((JsonArray)this)))
				{
					if (flag2)
					{
						w.Write(", ");
					}
					if (jsonValue != null)
					{
						jsonValue.Savepublic(w, saving);
					}
					else
					{
						w.Write("null");
					}
					flag2 = true;
				}
				w.Write(']');
				return;
			}
			case JsonType.Boolean:
				w.Write(this ? "true" : "false");
				return;
			}
			w.Write(((JsonPrimitive)this).GetFormattedString());
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00020010 File Offset: 0x0001E210
		public string ToString(bool saving = true)
		{
			StringWriter stringWriter = new StringWriter();
			this.Save(stringWriter, saving);
			return stringWriter.ToString();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00020034 File Offset: 0x0001E234
		private bool NeedEscape(string src, int i)
		{
			char c = src[i];
			return c < ' ' || c == '"' || c == '\\' || (c >= '\ud800' && c <= '\udbff' && (i == src.Length - 1 || src[i + 1] < '\udc00' || src[i + 1] > '\udfff')) || (c >= '\udc00' && c <= '\udfff' && (i == 0 || src[i - 1] < '\ud800' || src[i - 1] > '\udbff')) || c == '\u2028' || c == '\u2029' || (c == '/' && i > 0 && src[i - 1] == '<');
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000200FC File Offset: 0x0001E2FC
		public string EscapeString(string src)
		{
			if (src == null)
			{
				return null;
			}
			for (int i = 0; i < src.Length; i++)
			{
				if (this.NeedEscape(src, i))
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (i > 0)
					{
						stringBuilder.Append(src, 0, i);
					}
					return this.DoEscapeString(stringBuilder, src, i);
				}
			}
			return src;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00020148 File Offset: 0x0001E348
		private string DoEscapeString(StringBuilder sb, string src, int cur)
		{
			int num = cur;
			for (int i = cur; i < src.Length; i++)
			{
				if (this.NeedEscape(src, i))
				{
					sb.Append(src, num, i - num);
					char c = src[i];
					if (c <= '"')
					{
						switch (c)
						{
						case '\b':
							sb.Append("\\b");
							break;
						case '\t':
							sb.Append("\\t");
							break;
						case '\n':
							sb.Append("\\n");
							break;
						case '\v':
							goto IL_BD;
						case '\f':
							sb.Append("\\f");
							break;
						case '\r':
							sb.Append("\\r");
							break;
						default:
							if (c != '"')
							{
								goto IL_BD;
							}
							sb.Append("\\\"");
							break;
						}
					}
					else if (c != '/')
					{
						if (c != '\\')
						{
							goto IL_BD;
						}
						sb.Append("\\\\");
					}
					else
					{
						sb.Append("\\/");
					}
					IL_100:
					num = i + 1;
					goto IL_104;
					IL_BD:
					sb.Append("\\u");
					sb.Append(((int)src[i]).ToString("x04"));
					goto IL_100;
				}
				IL_104:;
			}
			sb.Append(src, num, src.Length - num);
			return sb.ToString();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000094C4 File Offset: 0x000076C4
		public static implicit operator JsonValue(bool value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000094CC File Offset: 0x000076CC
		public static implicit operator JsonValue(byte value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000094D4 File Offset: 0x000076D4
		public static implicit operator JsonValue(char value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000094DC File Offset: 0x000076DC
		public static implicit operator JsonValue(decimal value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000094E4 File Offset: 0x000076E4
		public static implicit operator JsonValue(double value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000094EC File Offset: 0x000076EC
		public static implicit operator JsonValue(float value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000094F4 File Offset: 0x000076F4
		public static implicit operator JsonValue(int value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000094FC File Offset: 0x000076FC
		public static implicit operator JsonValue(long value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00009504 File Offset: 0x00007704
		public static implicit operator JsonValue(sbyte value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000950C File Offset: 0x0000770C
		public static implicit operator JsonValue(short value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00009514 File Offset: 0x00007714
		public static implicit operator JsonValue(string value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000951C File Offset: 0x0000771C
		public static implicit operator JsonValue(uint value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009524 File Offset: 0x00007724
		public static implicit operator JsonValue(ulong value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000952C File Offset: 0x0000772C
		public static implicit operator JsonValue(ushort value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009534 File Offset: 0x00007734
		public static implicit operator JsonValue(DateTime value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000953C File Offset: 0x0000773C
		public static implicit operator JsonValue(DateTimeOffset value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009544 File Offset: 0x00007744
		public static implicit operator JsonValue(Guid value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000954C File Offset: 0x0000774C
		public static implicit operator JsonValue(TimeSpan value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009554 File Offset: 0x00007754
		public static implicit operator JsonValue(Uri value)
		{
			return new JsonPrimitive(value);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000955C File Offset: 0x0000775C
		public static implicit operator bool(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToBoolean(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009581 File Offset: 0x00007781
		public static implicit operator byte(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToByte(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000095A6 File Offset: 0x000077A6
		public static implicit operator char(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToChar(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000095CB File Offset: 0x000077CB
		public static implicit operator decimal(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToDecimal(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000095F0 File Offset: 0x000077F0
		public static implicit operator double(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToDouble(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009615 File Offset: 0x00007815
		public static implicit operator float(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToSingle(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000963A File Offset: 0x0000783A
		public static implicit operator int(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToInt32(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000965F File Offset: 0x0000785F
		public static implicit operator long(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToInt64(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009684 File Offset: 0x00007884
		public static implicit operator sbyte(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToSByte(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000096A9 File Offset: 0x000078A9
		public static implicit operator short(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToInt16(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000096CE File Offset: 0x000078CE
		public static implicit operator string(JsonValue value)
		{
			if (value == null)
			{
				return null;
			}
			return value.ToString(true);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000096DC File Offset: 0x000078DC
		public static implicit operator uint(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToUInt32(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009701 File Offset: 0x00007901
		public static implicit operator ulong(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToUInt64(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00009726 File Offset: 0x00007926
		public static implicit operator ushort(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Convert.ToUInt16(((JsonPrimitive)value).Value, NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000974B File Offset: 0x0000794B
		public static implicit operator DateTime(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return (DateTime)((JsonPrimitive)value).Value;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000976B File Offset: 0x0000796B
		public static implicit operator DateTimeOffset(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return (DateTimeOffset)((JsonPrimitive)value).Value;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000978B File Offset: 0x0000798B
		public static implicit operator TimeSpan(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return (TimeSpan)((JsonPrimitive)value).Value;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000097AB File Offset: 0x000079AB
		public static implicit operator Guid(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return (Guid)((JsonPrimitive)value).Value;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000097CB File Offset: 0x000079CB
		public static implicit operator Uri(JsonValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return (Uri)((JsonPrimitive)value).Value;
		}

		// Token: 0x040000B8 RID: 184
		public static string buildversion = "DarkStealer";
	}
}
