using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Echelon.Stealer.Browsers.Helpers.NoiseMe.Drags.App.Models.JSON
{
	// Token: 0x0200003D RID: 61
	public class JsonPrimitive : JsonValue
	{
		// Token: 0x06000152 RID: 338 RVA: 0x0000927C File Offset: 0x0000747C
		public JsonPrimitive(bool value)
		{
			this.Value = value;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009290 File Offset: 0x00007490
		public JsonPrimitive(byte value)
		{
			this.Value = value;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000092A4 File Offset: 0x000074A4
		public JsonPrimitive(char value)
		{
			this.Value = value;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000092B8 File Offset: 0x000074B8
		public JsonPrimitive(decimal value)
		{
			this.Value = value;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000092CC File Offset: 0x000074CC
		public JsonPrimitive(double value)
		{
			this.Value = value;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000092E0 File Offset: 0x000074E0
		public JsonPrimitive(float value)
		{
			this.Value = value;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000092F4 File Offset: 0x000074F4
		public JsonPrimitive(int value)
		{
			this.Value = value;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009308 File Offset: 0x00007508
		public JsonPrimitive(long value)
		{
			this.Value = value;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000931C File Offset: 0x0000751C
		public JsonPrimitive(sbyte value)
		{
			this.Value = value;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009330 File Offset: 0x00007530
		public JsonPrimitive(short value)
		{
			this.Value = value;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009344 File Offset: 0x00007544
		public JsonPrimitive(string value)
		{
			this.Value = value;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009353 File Offset: 0x00007553
		public JsonPrimitive(DateTime value)
		{
			this.Value = value;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009367 File Offset: 0x00007567
		public JsonPrimitive(uint value)
		{
			this.Value = value;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000937B File Offset: 0x0000757B
		public JsonPrimitive(ulong value)
		{
			this.Value = value;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000938F File Offset: 0x0000758F
		public JsonPrimitive(ushort value)
		{
			this.Value = value;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000093A3 File Offset: 0x000075A3
		public JsonPrimitive(DateTimeOffset value)
		{
			this.Value = value;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000093B7 File Offset: 0x000075B7
		public JsonPrimitive(Guid value)
		{
			this.Value = value;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000093CB File Offset: 0x000075CB
		public JsonPrimitive(TimeSpan value)
		{
			this.Value = value;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009344 File Offset: 0x00007544
		public JsonPrimitive(Uri value)
		{
			this.Value = value;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009344 File Offset: 0x00007544
		public JsonPrimitive(object value)
		{
			this.Value = value;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000093DF File Offset: 0x000075DF
		public object Value { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0001F90C File Offset: 0x0001DB0C
		public override JsonType JsonType
		{
			get
			{
				if (this.Value == null)
				{
					return JsonType.String;
				}
				TypeCode typeCode = Type.GetTypeCode(this.Value.GetType());
				switch (typeCode)
				{
				case TypeCode.Object:
				case TypeCode.Char:
					return JsonType.String;
				case TypeCode.DBNull:
					break;
				case TypeCode.Boolean:
					return JsonType.Boolean;
				default:
					if (typeCode == TypeCode.DateTime || typeCode == TypeCode.String)
					{
						return JsonType.String;
					}
					break;
				}
				return JsonType.Number;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001F95C File Offset: 0x0001DB5C
		public override void Save(Stream stream, bool parsing)
		{
			JsonType jsonType = this.JsonType;
			if (jsonType == JsonType.String)
			{
				stream.WriteByte(34);
				byte[] bytes = Encoding.UTF8.GetBytes(base.EscapeString(this.Value.ToString()));
				stream.Write(bytes, 0, bytes.Length);
				stream.WriteByte(34);
				return;
			}
			if (jsonType != JsonType.Boolean)
			{
				byte[] bytes2 = Encoding.UTF8.GetBytes(this.GetFormattedString());
				stream.Write(bytes2, 0, bytes2.Length);
				return;
			}
			if ((bool)this.Value)
			{
				stream.Write(JsonPrimitive.true_bytes, 0, 4);
				return;
			}
			stream.Write(JsonPrimitive.false_bytes, 0, 5);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001F9F4 File Offset: 0x0001DBF4
		public string GetFormattedString()
		{
			JsonType jsonType = this.JsonType;
			if (jsonType != JsonType.String)
			{
				if (jsonType != JsonType.Number)
				{
					throw new InvalidOperationException();
				}
				string text = (this.Value is float || this.Value is double) ? ((IFormattable)this.Value).ToString("R", NumberFormatInfo.InvariantInfo) : ((IFormattable)this.Value).ToString("G", NumberFormatInfo.InvariantInfo);
				if (!(text == "NaN") && !(text == "Infinity") && !(text == "-Infinity"))
				{
					return text;
				}
				return "\"" + text + "\"";
			}
			else if (!(this.Value is string) && this.Value != null)
			{
				if (this.Value is char)
				{
					return this.Value.ToString();
				}
				string str = "GetFormattedString from value type ";
				Type type = this.Value.GetType();
				throw new NotImplementedException(str + ((type != null) ? type.ToString() : null));
			}
			else
			{
				string text2 = this.Value as string;
				if (string.IsNullOrEmpty(text2))
				{
					return "null";
				}
				return text2.Trim(new char[]
				{
					'"'
				});
			}
		}

		// Token: 0x040000AF RID: 175
		private static readonly byte[] true_bytes = Encoding.UTF8.GetBytes("true");

		// Token: 0x040000B0 RID: 176
		private static readonly byte[] false_bytes = Encoding.UTF8.GetBytes("false");
	}
}
