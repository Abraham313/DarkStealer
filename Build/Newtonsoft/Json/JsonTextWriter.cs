using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001D0 RID: 464
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonTextWriter : JsonWriter
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0000FE6F File Offset: 0x0000E06F
		[Nullable(1)]
		private Base64Encoder Base64Encoder
		{
			[NullableContext(1)]
			get
			{
				if (this._base64Encoder == null)
				{
					this._base64Encoder = new Base64Encoder(this._writer);
				}
				return this._base64Encoder;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x0000FE90 File Offset: 0x0000E090
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x0000FE98 File Offset: 0x0000E098
		public IArrayPool<char> ArrayPool
		{
			get
			{
				return this._arrayPool;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._arrayPool = value;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0000FEAF File Offset: 0x0000E0AF
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x0000FEB7 File Offset: 0x0000E0B7
		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0000FECF File Offset: 0x0000E0CF
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x0000FED7 File Offset: 0x0000E0D7
		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
				this.UpdateCharEscapeFlags();
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0000FEFB File Offset: 0x0000E0FB
		// (set) Token: 0x06000D2F RID: 3375 RVA: 0x0000FF03 File Offset: 0x0000E103
		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				if (value != this._indentChar)
				{
					this._indentChar = value;
					this._indentChars = null;
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0000FF1C File Offset: 0x0000E11C
		// (set) Token: 0x06000D31 RID: 3377 RVA: 0x0000FF24 File Offset: 0x0000E124
		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00053A40 File Offset: 0x00051C40
		[NullableContext(1)]
		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0000FF2D File Offset: 0x0000E12D
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0000FF3A File Offset: 0x0000E13A
		public override void Close()
		{
			base.Close();
			this.CloseBufferAndWriter();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0000FF48 File Offset: 0x0000E148
		private void CloseBufferAndWriter()
		{
			if (this._writeBuffer != null)
			{
				BufferUtils.ReturnBuffer(this._arrayPool, this._writeBuffer);
				this._writeBuffer = null;
			}
			if (base.CloseOutput)
			{
				TextWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0000FF82 File Offset: 0x0000E182
		public override void WriteStartObject()
		{
			base.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			this._writer.Write('{');
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0000FF99 File Offset: 0x0000E199
		public override void WriteStartArray()
		{
			base.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			this._writer.Write('[');
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0000FFB0 File Offset: 0x0000E1B0
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write('(');
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00053A8C File Offset: 0x00051C8C
		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				this._writer.Write('}');
				return;
			case JsonToken.EndArray:
				this._writer.Write(']');
				return;
			case JsonToken.EndConstructor:
				this._writer.Write(')');
				return;
			default:
				throw JsonWriterException.Create(this, "Invalid JsonToken: " + token.ToString(), null);
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0000FFE3 File Offset: 0x0000E1E3
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			base.InternalWritePropertyName(name);
			this.WriteEscapedString(name, this._quoteName);
			this._writer.Write(':');
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00053AF8 File Offset: 0x00051CF8
		[NullableContext(1)]
		public override void WritePropertyName(string name, bool escape)
		{
			base.InternalWritePropertyName(name);
			if (escape)
			{
				this.WriteEscapedString(name, this._quoteName);
			}
			else
			{
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
				this._writer.Write(name);
				if (this._quoteName)
				{
					this._writer.Write(this._quoteChar);
				}
			}
			this._writer.Write(':');
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00010006 File Offset: 0x0000E206
		internal override void OnStringEscapeHandlingChanged()
		{
			this.UpdateCharEscapeFlags();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0001000E File Offset: 0x0000E20E
		private void UpdateCharEscapeFlags()
		{
			this._charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, this._quoteChar);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00053B6C File Offset: 0x00051D6C
		protected override void WriteIndent()
		{
			int num = base.Top * this._indentation;
			int num2 = this.SetIndentChars();
			this._writer.Write(this._indentChars, 0, num2 + Math.Min(num, 12));
			while ((num -= 12) > 0)
			{
				this._writer.Write(this._indentChars, num2, Math.Min(num, 12));
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00053BD0 File Offset: 0x00051DD0
		private int SetIndentChars()
		{
			string newLine = this._writer.NewLine;
			int length = newLine.Length;
			bool flag;
			if (flag = (this._indentChars != null && this._indentChars.Length == 12 + length))
			{
				for (int num = 0; num != length; num++)
				{
					if (newLine[num] != this._indentChars[num])
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				this._indentChars = (newLine + new string(this._indentChar, 12)).ToCharArray();
			}
			return length;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00010027 File Offset: 0x0000E227
		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00010036 File Offset: 0x0000E236
		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00010045 File Offset: 0x0000E245
		[NullableContext(1)]
		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00053C54 File Offset: 0x00051E54
		public override void WriteValue(object value)
		{
			if (value is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger bigInteger = (System.Numerics.BigInteger)value;
				base.InternalWriteValue(JsonToken.Integer);
				this.WriteValueInternal(bigInteger.ToString(CultureInfo.InvariantCulture), JsonToken.String);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00010053 File Offset: 0x0000E253
		public override void WriteNull()
		{
			base.InternalWriteValue(JsonToken.Null);
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0001006A File Offset: 0x0000E26A
		public override void WriteUndefined()
		{
			base.InternalWriteValue(JsonToken.Undefined);
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00010081 File Offset: 0x0000E281
		public override void WriteRaw(string json)
		{
			base.InternalWriteRaw();
			this._writer.Write(json);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00010095 File Offset: 0x0000E295
		public override void WriteValue(string value)
		{
			base.InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
				return;
			}
			this.WriteEscapedString(value, true);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000100B8 File Offset: 0x0000E2B8
		[NullableContext(1)]
		private void WriteEscapedString(string value, bool quote)
		{
			this.EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, base.StringEscapeHandling, this._arrayPool, ref this._writeBuffer);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000100EB File Offset: 0x0000E2EB
		public override void WriteValue(int value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000100FB File Offset: 0x0000E2FB
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((long)((ulong)value));
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0001010C File Offset: 0x0000E30C
		public override void WriteValue(long value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0001011C File Offset: 0x0000E31C
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue(value, false);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0001012D File Offset: 0x0000E32D
		public override void WriteValue(float value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00010150 File Offset: 0x0000E350
		public override void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00010189 File Offset: 0x0000E389
		public override void WriteValue(double value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000101AC File Offset: 0x0000E3AC
		public override void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000101E5 File Offset: 0x0000E3E5
		public override void WriteValue(bool value)
		{
			base.InternalWriteValue(JsonToken.Boolean);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x000100EB File Offset: 0x0000E2EB
		public override void WriteValue(short value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000100EB File Offset: 0x0000E2EB
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000101FD File Offset: 0x0000E3FD
		public override void WriteValue(char value)
		{
			base.InternalWriteValue(JsonToken.String);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000100EB File Offset: 0x0000E2EB
		public override void WriteValue(byte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000100EB File Offset: 0x0000E2EB
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.InternalWriteValue(JsonToken.Integer);
			this.WriteIntegerValue((int)value);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00010215 File Offset: 0x0000E415
		public override void WriteValue(decimal value)
		{
			base.InternalWriteValue(JsonToken.Float);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00053C94 File Offset: 0x00051E94
		public override void WriteValue(DateTime value)
		{
			base.InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (StringUtils.IsNullOrEmpty(base.DateFormatString))
			{
				int count = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, count);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00053D20 File Offset: 0x00051F20
		private int WriteValueToBuffer(DateTime value)
		{
			this.EnsureWriteBuffer();
			this._writeBuffer[0] = this._quoteChar;
			int result = DateTimeUtils.WriteDateTimeString(this._writeBuffer, 1, value, null, value.Kind, base.DateFormatHandling);
			this._writeBuffer[result++] = this._quoteChar;
			return result;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00053D7C File Offset: 0x00051F7C
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.Bytes);
			this._writer.Write(this._quoteChar);
			this.Base64Encoder.Encode(value, 0, value.Length);
			this.Base64Encoder.Flush();
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00053DD8 File Offset: 0x00051FD8
		public override void WriteValue(DateTimeOffset value)
		{
			base.InternalWriteValue(JsonToken.Date);
			if (StringUtils.IsNullOrEmpty(base.DateFormatString))
			{
				int count = this.WriteValueToBuffer(value);
				this._writer.Write(this._writeBuffer, 0, count);
				return;
			}
			this._writer.Write(this._quoteChar);
			this._writer.Write(value.ToString(base.DateFormatString, base.Culture));
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00053E58 File Offset: 0x00052058
		private int WriteValueToBuffer(DateTimeOffset value)
		{
			this.EnsureWriteBuffer();
			this._writeBuffer[0] = this._quoteChar;
			int result = DateTimeUtils.WriteDateTimeString(this._writeBuffer, 1, (base.DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, base.DateFormatHandling);
			this._writeBuffer[result++] = this._quoteChar;
			return result;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00053EC8 File Offset: 0x000520C8
		public override void WriteValue(Guid value)
		{
			base.InternalWriteValue(JsonToken.String);
			string value2 = value.ToString("D", CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(value2);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00053F20 File Offset: 0x00052120
		public override void WriteValue(TimeSpan value)
		{
			base.InternalWriteValue(JsonToken.String);
			string value2 = value.ToString(null, CultureInfo.InvariantCulture);
			this._writer.Write(this._quoteChar);
			this._writer.Write(value2);
			this._writer.Write(this._quoteChar);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0001022B File Offset: 0x0000E42B
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
			this.WriteEscapedString(value.OriginalString, true);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00010252 File Offset: 0x0000E452
		public override void WriteComment(string text)
		{
			base.InternalWriteComment();
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00010286 File Offset: 0x0000E486
		[NullableContext(1)]
		public override void WriteWhitespace(string ws)
		{
			base.InternalWriteWhitespace(ws);
			this._writer.Write(ws);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0001029B File Offset: 0x0000E49B
		private void EnsureWriteBuffer()
		{
			if (this._writeBuffer == null)
			{
				this._writeBuffer = BufferUtils.RentBuffer(this._arrayPool, 35);
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00053F74 File Offset: 0x00052174
		private void WriteIntegerValue(long value)
		{
			if (value >= 0L && value <= 9L)
			{
				this._writer.Write((char)(48L + value));
				return;
			}
			bool flag = value < 0L;
			this.WriteIntegerValue((ulong)(flag ? (-(ulong)value) : value), flag);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00053FD0 File Offset: 0x000521D0
		private void WriteIntegerValue(ulong value, bool negative)
		{
			if (!negative & value <= 9UL)
			{
				this._writer.Write((char)(48UL + value));
				return;
			}
			int count = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, count);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00054028 File Offset: 0x00052228
		private int WriteNumberToBuffer(ulong value, bool negative)
		{
			if (value <= 4294967295UL)
			{
				return this.WriteNumberToBuffer((uint)value, negative);
			}
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength(value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				ulong num3 = value / 10UL;
				ulong num4 = value - num3 * 10UL;
				this._writeBuffer[--num2] = (char)(48UL + num4);
				value = num3;
			}
			while (value != 0UL);
			return num;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000540A4 File Offset: 0x000522A4
		private void WriteIntegerValue(int value)
		{
			if (value >= 0 && value <= 9)
			{
				this._writer.Write((char)(48 + value));
				return;
			}
			bool flag = value < 0;
			this.WriteIntegerValue((uint)(flag ? (-(uint)value) : value), flag);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000540E0 File Offset: 0x000522E0
		private void WriteIntegerValue(uint value, bool negative)
		{
			if (!negative & value <= 9U)
			{
				this._writer.Write((char)(48U + value));
				return;
			}
			int count = this.WriteNumberToBuffer(value, negative);
			this._writer.Write(this._writeBuffer, 0, count);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0005412C File Offset: 0x0005232C
		private int WriteNumberToBuffer(uint value, bool negative)
		{
			this.EnsureWriteBuffer();
			int num = MathUtils.IntLength((ulong)value);
			if (negative)
			{
				num++;
				this._writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				uint num3 = value / 10U;
				uint num4 = value - num3 * 10U;
				this._writeBuffer[--num2] = (char)(48U + num4);
				value = num3;
			}
			while (value != 0U);
			return num;
		}

		// Token: 0x0400086C RID: 2156
		private const int IndentCharBufferSize = 12;

		// Token: 0x0400086D RID: 2157
		[Nullable(1)]
		private readonly TextWriter _writer;

		// Token: 0x0400086E RID: 2158
		private Base64Encoder _base64Encoder;

		// Token: 0x0400086F RID: 2159
		private char _indentChar;

		// Token: 0x04000870 RID: 2160
		private int _indentation;

		// Token: 0x04000871 RID: 2161
		private char _quoteChar;

		// Token: 0x04000872 RID: 2162
		private bool _quoteName;

		// Token: 0x04000873 RID: 2163
		private bool[] _charEscapeFlags;

		// Token: 0x04000874 RID: 2164
		private char[] _writeBuffer;

		// Token: 0x04000875 RID: 2165
		private IArrayPool<char> _arrayPool;

		// Token: 0x04000876 RID: 2166
		private char[] _indentChars;
	}
}
