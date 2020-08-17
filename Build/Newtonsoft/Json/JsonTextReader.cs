using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001CF RID: 463
	[NullableContext(1)]
	[Nullable(0)]
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000CD8 RID: 3288 RVA: 0x0000FA00 File Offset: 0x0000DC00
		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this._lineNumber = 1;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0000FA24 File Offset: 0x0000DC24
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		[Nullable(2)]
		public JsonNameTable PropertyNameTable { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0000FA35 File Offset: 0x0000DC35
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x0000FA3D File Offset: 0x0000DC3D
		[Nullable(2)]
		public IArrayPool<char> ArrayPool
		{
			[NullableContext(2)]
			get
			{
				return this._arrayPool;
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._arrayPool = value;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0000FA54 File Offset: 0x0000DC54
		private void EnsureBufferNotEmpty()
		{
			if (this._stringBuffer.IsEmpty)
			{
				this._stringBuffer = new StringBuffer(this._arrayPool, 1024);
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0000FA79 File Offset: 0x0000DC79
		private void SetNewLine(bool hasNextChar)
		{
			if (hasNextChar && this._chars[this._charPos] == '\n')
			{
				this._charPos++;
			}
			this.OnNewLine(this._charPos);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0000FAA9 File Offset: 0x0000DCA9
		private void OnNewLine(int pos)
		{
			this._lineNumber++;
			this._lineStartPos = pos;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
		private void ParseString(char quote, ReadType readType)
		{
			this._charPos++;
			this.ShiftBufferIfNeeded();
			this.ReadStringIntoBuffer(quote);
			this.ParseReadString(quote, readType);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00050E3C File Offset: 0x0004F03C
		private void ParseReadString(char quote, ReadType readType)
		{
			base.SetPostValueState(true);
			switch (readType)
			{
			case ReadType.ReadAsInt32:
			case ReadType.ReadAsDecimal:
			case ReadType.ReadAsBoolean:
				return;
			case ReadType.ReadAsBytes:
			{
				byte[] value;
				Guid guid;
				if (this._stringReference.Length == 0)
				{
					value = CollectionUtils.ArrayEmpty<byte>();
				}
				else if (this._stringReference.Length == 36 && ConvertUtils.TryConvertGuid(this._stringReference.ToString(), out guid))
				{
					value = guid.ToByteArray();
				}
				else
				{
					value = Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length);
				}
				base.SetToken(JsonToken.Bytes, value, false);
				return;
			}
			case ReadType.ReadAsString:
			{
				string value2 = this._stringReference.ToString();
				base.SetToken(JsonToken.String, value2, false);
				this._quoteChar = quote;
				return;
			}
			}
			if (this._dateParseHandling != DateParseHandling.None)
			{
				DateParseHandling dateParseHandling;
				if (readType == ReadType.ReadAsDateTime)
				{
					dateParseHandling = DateParseHandling.DateTime;
				}
				else if (readType == ReadType.ReadAsDateTimeOffset)
				{
					dateParseHandling = DateParseHandling.DateTimeOffset;
				}
				else
				{
					dateParseHandling = this._dateParseHandling;
				}
				DateTimeOffset dateTimeOffset;
				if (dateParseHandling == DateParseHandling.DateTime)
				{
					DateTime dateTime;
					if (DateTimeUtils.TryParseDateTime(this._stringReference, base.DateTimeZoneHandling, base.DateFormatString, base.Culture, out dateTime))
					{
						base.SetToken(JsonToken.Date, dateTime, false);
						return;
					}
				}
				else if (DateTimeUtils.TryParseDateTimeOffset(this._stringReference, base.DateFormatString, base.Culture, out dateTimeOffset))
				{
					base.SetToken(JsonToken.Date, dateTimeOffset, false);
					return;
				}
			}
			base.SetToken(JsonToken.String, this._stringReference.ToString(), false);
			this._quoteChar = quote;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0000FAE5 File Offset: 0x0000DCE5
		private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00050FC4 File Offset: 0x0004F1C4
		private void ShiftBufferIfNeeded()
		{
			int num = this._chars.Length;
			if ((double)(num - this._charPos) <= (double)num * 0.1 || num >= 1073741823)
			{
				int num2 = this._charsUsed - this._charPos;
				if (num2 > 0)
				{
					JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num2);
				}
				this._lineStartPos -= this._charPos;
				this._charPos = 0;
				this._charsUsed = num2;
				this._chars[this._charsUsed] = '\0';
			}
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
		private int ReadData(bool append)
		{
			return this.ReadData(append, 0);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00051054 File Offset: 0x0004F254
		private void PrepareBufferForReadData(bool append, int charsRequired)
		{
			if (this._charsUsed + charsRequired >= this._chars.Length - 1)
			{
				if (append)
				{
					int num = this._chars.Length * 2;
					int minSize = Math.Max((num < 0) ? int.MaxValue : num, this._charsUsed + charsRequired + 1);
					char[] array = BufferUtils.RentBuffer(this._arrayPool, minSize);
					JsonTextReader.BlockCopyChars(this._chars, 0, array, 0, this._chars.Length);
					BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
					this._chars = array;
					return;
				}
				int num2 = this._charsUsed - this._charPos;
				if (num2 + charsRequired + 1 >= this._chars.Length)
				{
					char[] array2 = BufferUtils.RentBuffer(this._arrayPool, num2 + charsRequired + 1);
					if (num2 > 0)
					{
						JsonTextReader.BlockCopyChars(this._chars, this._charPos, array2, 0, num2);
					}
					BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
					this._chars = array2;
				}
				else if (num2 > 0)
				{
					JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, num2);
				}
				this._lineStartPos -= this._charPos;
				this._charPos = 0;
				this._charsUsed = num2;
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00051180 File Offset: 0x0004F380
		private int ReadData(bool append, int charsRequired)
		{
			if (this._isEndOfFile)
			{
				return 0;
			}
			this.PrepareBufferForReadData(append, charsRequired);
			int count = this._chars.Length - this._charsUsed - 1;
			int num = this._reader.Read(this._chars, this._charsUsed, count);
			this._charsUsed += num;
			if (num == 0)
			{
				this._isEndOfFile = true;
			}
			this._chars[this._charsUsed] = '\0';
			return num;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0000FB02 File Offset: 0x0000DD02
		private bool EnsureChars(int relativePosition, bool append)
		{
			return this._charPos + relativePosition < this._charsUsed || this.ReadChars(relativePosition, append);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000511F4 File Offset: 0x0004F3F4
		private bool ReadChars(int relativePosition, bool append)
		{
			if (this._isEndOfFile)
			{
				return false;
			}
			int num = this._charPos + relativePosition - this._charsUsed + 1;
			int num2 = 0;
			do
			{
				int num3 = this.ReadData(append, num - num2);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
			}
			while (num2 < num);
			return num2 >= num;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00051240 File Offset: 0x0004F440
		public override bool Read()
		{
			this.EnsureBuffer();
			for (;;)
			{
				switch (this._currentState)
				{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
					goto IL_5D;
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
					goto IL_54;
				case JsonReader.State.PostValue:
					if (!this.ParsePostValue(false))
					{
						continue;
					}
					return true;
				case JsonReader.State.Finished:
					goto IL_85;
				}
				break;
			}
			goto IL_64;
			IL_54:
			return this.ParseObject();
			IL_5D:
			return this.ParseValue();
			IL_64:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
			IL_85:
			if (!this.EnsureChars(0, false))
			{
				base.SetToken(JsonToken.None);
				return false;
			}
			this.EatWhitespace();
			if (this._isEndOfFile)
			{
				base.SetToken(JsonToken.None);
				return false;
			}
			if (this._chars[this._charPos] == '/')
			{
				this.ParseComment(true);
				return true;
			}
			throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0000FB1E File Offset: 0x0000DD1E
		public override int? ReadAsInt32()
		{
			return (int?)this.ReadNumberValue(ReadType.ReadAsInt32);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0000FB2C File Offset: 0x0000DD2C
		public override DateTime? ReadAsDateTime()
		{
			return (DateTime?)this.ReadStringValue(ReadType.ReadAsDateTime);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0000FB3A File Offset: 0x0000DD3A
		[NullableContext(2)]
		public override string ReadAsString()
		{
			return (string)this.ReadStringValue(ReadType.ReadAsString);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00051340 File Offset: 0x0004F540
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			this.EnsureBuffer();
			bool flag = false;
			switch (this._currentState)
			{
			case JsonReader.State.Start:
			case JsonReader.State.Property:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.Array:
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
				break;
			case JsonReader.State.Complete:
			case JsonReader.State.ObjectStart:
			case JsonReader.State.Object:
			case JsonReader.State.Closed:
			case JsonReader.State.Error:
				goto IL_222;
			case JsonReader.State.PostValue:
				if (this.ParsePostValue(true))
				{
					return null;
				}
				break;
			case JsonReader.State.Finished:
				this.ReadFinished();
				return null;
			default:
				goto IL_222;
			}
			char c;
			do
			{
				c = this._chars[this._charPos];
				if (c <= '\'')
				{
					if (c <= '\r')
					{
						if (c != '\0')
						{
							switch (c)
							{
							case '\t':
								break;
							case '\n':
								this.ProcessLineFeed();
								continue;
							case '\v':
							case '\f':
								goto IL_A1;
							case '\r':
								this.ProcessCarriageReturn(false);
								continue;
							default:
								goto IL_A1;
							}
						}
						else
						{
							if (this.ReadNullChar())
							{
								goto Block_13;
							}
							continue;
						}
					}
					else if (c != ' ')
					{
						if (c != '"' && c != '\'')
						{
							goto IL_A1;
						}
						goto IL_165;
					}
					this._charPos++;
					continue;
				}
				if (c <= '[')
				{
					if (c == ',')
					{
						this.ProcessValueComma();
						continue;
					}
					if (c == '/')
					{
						this.ParseComment(false);
						continue;
					}
					if (c == '[')
					{
						goto IL_1B9;
					}
				}
				else
				{
					if (c == ']')
					{
						goto IL_1E5;
					}
					if (c == 'n')
					{
						goto IL_1DD;
					}
					if (c == '{')
					{
						this._charPos++;
						base.SetToken(JsonToken.StartObject);
						base.ReadIntoWrappedTypeObject();
						flag = true;
						continue;
					}
				}
				IL_A1:
				this._charPos++;
			}
			while (char.IsWhiteSpace(c));
			throw this.CreateUnexpectedCharacterException(c);
			Block_13:
			base.SetToken(JsonToken.None, null, false);
			return null;
			IL_165:
			this.ParseString(c, ReadType.ReadAsBytes);
			byte[] array = (byte[])this.Value;
			if (flag)
			{
				base.ReaderReadAndAssert();
				if (this.TokenType != JsonToken.EndObject)
				{
					throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
				}
				base.SetToken(JsonToken.Bytes, array, false);
			}
			return array;
			IL_1B9:
			this._charPos++;
			base.SetToken(JsonToken.StartArray);
			return base.ReadArrayIntoByteArray();
			IL_1DD:
			this.HandleNull();
			return null;
			IL_1E5:
			this._charPos++;
			if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
			{
				if (this._currentState != JsonReader.State.PostValue)
				{
					throw this.CreateUnexpectedCharacterException(c);
				}
			}
			base.SetToken(JsonToken.EndArray);
			return null;
			IL_222:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00051598 File Offset: 0x0004F798
		[NullableContext(2)]
		private object ReadStringValue(ReadType readType)
		{
			this.EnsureBuffer();
			switch (this._currentState)
			{
			case JsonReader.State.Start:
			case JsonReader.State.Property:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.Array:
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
				break;
			case JsonReader.State.Complete:
			case JsonReader.State.ObjectStart:
			case JsonReader.State.Object:
			case JsonReader.State.Closed:
			case JsonReader.State.Error:
				goto IL_2D1;
			case JsonReader.State.PostValue:
				if (this.ParsePostValue(true))
				{
					return null;
				}
				break;
			case JsonReader.State.Finished:
				this.ReadFinished();
				return null;
			default:
				goto IL_2D1;
			}
			char c;
			do
			{
				c = this._chars[this._charPos];
				if (c <= 'I')
				{
					if (c <= '\r')
					{
						if (c != '\0')
						{
							switch (c)
							{
							case '\t':
								break;
							case '\n':
								this.ProcessLineFeed();
								continue;
							case '\v':
							case '\f':
								goto IL_89;
							case '\r':
								this.ProcessCarriageReturn(false);
								continue;
							default:
								goto IL_89;
							}
						}
						else
						{
							if (this.ReadNullChar())
							{
								goto Block_12;
							}
							continue;
						}
					}
					else
					{
						switch (c)
						{
						case ' ':
							break;
						case '!':
						case '#':
						case '$':
						case '%':
						case '&':
						case '(':
						case ')':
						case '*':
						case '+':
							goto IL_89;
						case '"':
						case '\'':
							goto IL_1BD;
						case ',':
							this.ProcessValueComma();
							continue;
						case '-':
							goto IL_1CD;
						case '.':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							goto IL_200;
						case '/':
							this.ParseComment(false);
							continue;
						default:
							if (c != 'I')
							{
								goto IL_89;
							}
							goto IL_1B5;
						}
					}
					this._charPos++;
					continue;
				}
				if (c <= ']')
				{
					if (c == 'N')
					{
						goto IL_265;
					}
					if (c == ']')
					{
						goto IL_228;
					}
				}
				else
				{
					if (c == 'f')
					{
						goto IL_27D;
					}
					if (c == 'n')
					{
						goto IL_275;
					}
					if (c == 't')
					{
						goto IL_27D;
					}
				}
				IL_89:
				this._charPos++;
			}
			while (char.IsWhiteSpace(c));
			throw this.CreateUnexpectedCharacterException(c);
			Block_12:
			base.SetToken(JsonToken.None, null, false);
			return null;
			IL_1B5:
			return this.ParseNumberPositiveInfinity(readType);
			IL_1BD:
			this.ParseString(c, readType);
			return this.FinishReadQuotedStringValue(readType);
			IL_1CD:
			if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
			{
				return this.ParseNumberNegativeInfinity(readType);
			}
			this.ParseNumber(readType);
			return this.Value;
			IL_200:
			if (readType != ReadType.ReadAsString)
			{
				this._charPos++;
				throw this.CreateUnexpectedCharacterException(c);
			}
			this.ParseNumber(ReadType.ReadAsString);
			return this.Value;
			IL_228:
			this._charPos++;
			if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
			{
				if (this._currentState != JsonReader.State.PostValue)
				{
					throw this.CreateUnexpectedCharacterException(c);
				}
			}
			base.SetToken(JsonToken.EndArray);
			return null;
			IL_265:
			return this.ParseNumberNaN(readType);
			IL_275:
			this.HandleNull();
			return null;
			IL_27D:
			if (readType != ReadType.ReadAsString)
			{
				this._charPos++;
				throw this.CreateUnexpectedCharacterException(c);
			}
			string text = (c == 't') ? JsonConvert.True : JsonConvert.False;
			if (!this.MatchValueWithTrailingSeparator(text))
			{
				throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
			}
			base.SetToken(JsonToken.String, text);
			return text;
			IL_2D1:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000518A0 File Offset: 0x0004FAA0
		[NullableContext(2)]
		private object FinishReadQuotedStringValue(ReadType readType)
		{
			switch (readType)
			{
			case ReadType.ReadAsBytes:
			case ReadType.ReadAsString:
				return this.Value;
			case ReadType.ReadAsDateTime:
			{
				object value = this.Value;
				if (value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					return dateTime;
				}
				return base.ReadDateTimeString((string)this.Value);
			}
			case ReadType.ReadAsDateTimeOffset:
			{
				object value = this.Value;
				if (value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					return dateTimeOffset;
				}
				return base.ReadDateTimeOffsetString((string)this.Value);
			}
			}
			throw new ArgumentOutOfRangeException("readType");
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0000FB48 File Offset: 0x0000DD48
		private JsonReaderException CreateUnexpectedCharacterException(char c)
		{
			return JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00051944 File Offset: 0x0004FB44
		public override bool? ReadAsBoolean()
		{
			this.EnsureBuffer();
			switch (this._currentState)
			{
			case JsonReader.State.Start:
			case JsonReader.State.Property:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.Array:
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
				break;
			case JsonReader.State.Complete:
			case JsonReader.State.ObjectStart:
			case JsonReader.State.Object:
			case JsonReader.State.Closed:
			case JsonReader.State.Error:
				goto IL_2D1;
			case JsonReader.State.PostValue:
				if (this.ParsePostValue(true))
				{
					return null;
				}
				break;
			case JsonReader.State.Finished:
				this.ReadFinished();
				return null;
			default:
				goto IL_2D1;
			}
			char c;
			do
			{
				c = this._chars[this._charPos];
				if (c <= '9')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.ProcessLineFeed();
							continue;
						case '\v':
						case '\f':
							goto IL_89;
						case '\r':
							this.ProcessCarriageReturn(false);
							continue;
						default:
							switch (c)
							{
							case ' ':
								break;
							case '!':
							case '#':
							case '$':
							case '%':
							case '&':
							case '(':
							case ')':
							case '*':
							case '+':
								goto IL_89;
							case '"':
							case '\'':
								goto IL_198;
							case ',':
								this.ProcessValueComma();
								continue;
							case '-':
							case '.':
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
								goto IL_1B8;
							case '/':
								this.ParseComment(false);
								continue;
							default:
								goto IL_89;
							}
							break;
						}
						this._charPos++;
						continue;
					}
					if (!this.ReadNullChar())
					{
						continue;
					}
					goto IL_214;
				}
				else if (c <= 'f')
				{
					if (c == ']')
					{
						goto IL_227;
					}
					if (c == 'f')
					{
						goto IL_274;
					}
				}
				else
				{
					if (c == 'n')
					{
						goto IL_2C1;
					}
					if (c == 't')
					{
						goto IL_274;
					}
				}
				IL_89:
				this._charPos++;
			}
			while (char.IsWhiteSpace(c));
			throw this.CreateUnexpectedCharacterException(c);
			IL_198:
			this.ParseString(c, ReadType.Read);
			return base.ReadBooleanString(this._stringReference.ToString());
			IL_1B8:
			this.ParseNumber(ReadType.Read);
			object value = this.Value;
			bool flag;
			if (value is System.Numerics.BigInteger)
			{
				System.Numerics.BigInteger left = (System.Numerics.BigInteger)value;
				flag = (left != 0L);
			}
			else
			{
				flag = Convert.ToBoolean(this.Value, CultureInfo.InvariantCulture);
			}
			base.SetToken(JsonToken.Boolean, flag, false);
			return new bool?(flag);
			IL_214:
			base.SetToken(JsonToken.None, null, false);
			return null;
			IL_227:
			this._charPos++;
			if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
			{
				if (this._currentState != JsonReader.State.PostValue)
				{
					throw this.CreateUnexpectedCharacterException(c);
				}
			}
			base.SetToken(JsonToken.EndArray);
			return null;
			IL_274:
			bool flag2;
			string value2 = (flag2 = (c == 't')) ? JsonConvert.True : JsonConvert.False;
			if (!this.MatchValueWithTrailingSeparator(value2))
			{
				throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
			}
			base.SetToken(JsonToken.Boolean, flag2);
			return new bool?(flag2);
			IL_2C1:
			this.HandleNull();
			return null;
			IL_2D1:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0000FB65 File Offset: 0x0000DD65
		private void ProcessValueComma()
		{
			this._charPos++;
			if (this._currentState != JsonReader.State.PostValue)
			{
				base.SetToken(JsonToken.Undefined);
				JsonReaderException ex = this.CreateUnexpectedCharacterException(',');
				this._charPos--;
				throw ex;
			}
			base.SetStateBasedOnCurrent();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00051C54 File Offset: 0x0004FE54
		[NullableContext(2)]
		private object ReadNumberValue(ReadType readType)
		{
			this.EnsureBuffer();
			switch (this._currentState)
			{
			case JsonReader.State.Start:
			case JsonReader.State.Property:
			case JsonReader.State.ArrayStart:
			case JsonReader.State.Array:
			case JsonReader.State.ConstructorStart:
			case JsonReader.State.Constructor:
				break;
			case JsonReader.State.Complete:
			case JsonReader.State.ObjectStart:
			case JsonReader.State.Object:
			case JsonReader.State.Closed:
			case JsonReader.State.Error:
				goto IL_246;
			case JsonReader.State.PostValue:
				if (this.ParsePostValue(true))
				{
					return null;
				}
				break;
			case JsonReader.State.Finished:
				this.ReadFinished();
				return null;
			default:
				goto IL_246;
			}
			char c;
			do
			{
				c = this._chars[this._charPos];
				if (c <= '9')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.ProcessLineFeed();
							continue;
						case '\v':
						case '\f':
							goto IL_81;
						case '\r':
							this.ProcessCarriageReturn(false);
							continue;
						default:
							switch (c)
							{
							case ' ':
								break;
							case '!':
							case '#':
							case '$':
							case '%':
							case '&':
							case '(':
							case ')':
							case '*':
							case '+':
								goto IL_81;
							case '"':
							case '\'':
								goto IL_18D;
							case ',':
								this.ProcessValueComma();
								continue;
							case '-':
								goto IL_19D;
							case '.':
							case '0':
							case '1':
							case '2':
							case '3':
							case '4':
							case '5':
							case '6':
							case '7':
							case '8':
							case '9':
								goto IL_1D0;
							case '/':
								this.ParseComment(false);
								continue;
							default:
								goto IL_81;
							}
							break;
						}
						this._charPos++;
						continue;
					}
					if (!this.ReadNullChar())
					{
						continue;
					}
					goto IL_1DE;
				}
				else if (c <= 'N')
				{
					if (c == 'I')
					{
						goto IL_1F1;
					}
					if (c == 'N')
					{
						goto IL_1E9;
					}
				}
				else
				{
					if (c == ']')
					{
						goto IL_209;
					}
					if (c == 'n')
					{
						goto IL_201;
					}
				}
				IL_81:
				this._charPos++;
			}
			while (char.IsWhiteSpace(c));
			throw this.CreateUnexpectedCharacterException(c);
			IL_18D:
			this.ParseString(c, readType);
			return this.FinishReadQuotedNumber(readType);
			IL_19D:
			if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
			{
				return this.ParseNumberNegativeInfinity(readType);
			}
			this.ParseNumber(readType);
			return this.Value;
			IL_1D0:
			this.ParseNumber(readType);
			return this.Value;
			IL_1DE:
			base.SetToken(JsonToken.None, null, false);
			return null;
			IL_1E9:
			return this.ParseNumberNaN(readType);
			IL_1F1:
			return this.ParseNumberPositiveInfinity(readType);
			IL_201:
			this.HandleNull();
			return null;
			IL_209:
			this._charPos++;
			if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart)
			{
				if (this._currentState != JsonReader.State.PostValue)
				{
					throw this.CreateUnexpectedCharacterException(c);
				}
			}
			base.SetToken(JsonToken.EndArray);
			return null;
			IL_246:
			throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00051ED0 File Offset: 0x000500D0
		[NullableContext(2)]
		private object FinishReadQuotedNumber(ReadType readType)
		{
			if (readType == ReadType.ReadAsInt32)
			{
				return base.ReadInt32String(this._stringReference.ToString());
			}
			if (readType == ReadType.ReadAsDecimal)
			{
				return base.ReadDecimalString(this._stringReference.ToString());
			}
			if (readType != ReadType.ReadAsDouble)
			{
				throw new ArgumentOutOfRangeException("readType");
			}
			return base.ReadDoubleString(this._stringReference.ToString());
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0000FBA3 File Offset: 0x0000DDA3
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return (DateTimeOffset?)this.ReadStringValue(ReadType.ReadAsDateTimeOffset);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0000FBB1 File Offset: 0x0000DDB1
		public override decimal? ReadAsDecimal()
		{
			return (decimal?)this.ReadNumberValue(ReadType.ReadAsDecimal);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0000FBBF File Offset: 0x0000DDBF
		public override double? ReadAsDouble()
		{
			return (double?)this.ReadNumberValue(ReadType.ReadAsDouble);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00051F4C File Offset: 0x0005014C
		private void HandleNull()
		{
			if (!this.EnsureChars(1, true))
			{
				this._charPos = this._charsUsed;
				throw base.CreateUnexpectedEndException();
			}
			if (this._chars[this._charPos + 1] == 'u')
			{
				this.ParseNull();
				return;
			}
			this._charPos += 2;
			throw this.CreateUnexpectedCharacterException(this._chars[this._charPos - 1]);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00051FB8 File Offset: 0x000501B8
		private void ReadFinished()
		{
			if (this.EnsureChars(0, false))
			{
				this.EatWhitespace();
				if (this._isEndOfFile)
				{
					return;
				}
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				this.ParseComment(false);
			}
			base.SetToken(JsonToken.None);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0000FBCD File Offset: 0x0000DDCD
		private bool ReadNullChar()
		{
			if (this._charsUsed == this._charPos)
			{
				if (this.ReadData(false) == 0)
				{
					this._isEndOfFile = true;
					return true;
				}
			}
			else
			{
				this._charPos++;
			}
			return false;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0000FBFE File Offset: 0x0000DDFE
		private void EnsureBuffer()
		{
			if (this._chars == null)
			{
				this._chars = BufferUtils.RentBuffer(this._arrayPool, 1024);
				this._chars[0] = '\0';
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00052028 File Offset: 0x00050228
		private void ReadStringIntoBuffer(char quote)
		{
			int num = this._charPos;
			int charPos = this._charPos;
			int lastWritePosition = this._charPos;
			this._stringBuffer.Position = 0;
			char c2;
			for (;;)
			{
				char c = this._chars[num++];
				if (c <= '\r')
				{
					if (c != '\0')
					{
						if (c != '\n')
						{
							if (c == '\r')
							{
								this._charPos = num - 1;
								this.ProcessCarriageReturn(true);
								num = this._charPos;
							}
						}
						else
						{
							this._charPos = num - 1;
							this.ProcessLineFeed();
							num = this._charPos;
						}
					}
					else if (this._charsUsed == num - 1)
					{
						num--;
						if (this.ReadData(true) == 0)
						{
							goto IL_289;
						}
					}
				}
				else if (c != '"' && c != '\'')
				{
					if (c == '\\')
					{
						this._charPos = num;
						if (!this.EnsureChars(0, true))
						{
							goto IL_2AC;
						}
						int writeToPosition = num - 1;
						c2 = this._chars[num];
						num++;
						char c3;
						if (c2 <= '\\')
						{
							if (c2 <= '\'')
							{
								if (c2 != '"' && c2 != '\'')
								{
									break;
								}
							}
							else if (c2 != '/')
							{
								if (c2 != '\\')
								{
									break;
								}
								c3 = '\\';
								goto IL_1F3;
							}
							c3 = c2;
						}
						else if (c2 <= 'f')
						{
							if (c2 != 'b')
							{
								if (c2 != 'f')
								{
									break;
								}
								c3 = '\f';
							}
							else
							{
								c3 = '\b';
							}
						}
						else
						{
							if (c2 != 'n')
							{
								switch (c2)
								{
								case 'r':
									c3 = '\r';
									goto IL_1F3;
								case 't':
									c3 = '\t';
									goto IL_1F3;
								case 'u':
									this._charPos = num;
									c3 = this.ParseUnicode();
									if (StringUtils.IsLowSurrogate(c3))
									{
										c3 = '�';
									}
									else if (StringUtils.IsHighSurrogate(c3))
									{
										bool flag;
										do
										{
											flag = false;
											if (this.EnsureChars(2, true) && this._chars[this._charPos] == '\\' && this._chars[this._charPos + 1] == 'u')
											{
												char writeChar = c3;
												this._charPos += 2;
												c3 = this.ParseUnicode();
												if (!StringUtils.IsLowSurrogate(c3))
												{
													if (StringUtils.IsHighSurrogate(c3))
													{
														writeChar = '�';
														flag = true;
													}
													else
													{
														writeChar = '�';
													}
												}
												this.EnsureBufferNotEmpty();
												this.WriteCharToBuffer(writeChar, lastWritePosition, writeToPosition);
												lastWritePosition = this._charPos;
											}
											else
											{
												c3 = '�';
											}
										}
										while (flag);
									}
									num = this._charPos;
									goto IL_1F3;
								}
								break;
							}
							c3 = '\n';
						}
						IL_1F3:
						this.EnsureBufferNotEmpty();
						this.WriteCharToBuffer(c3, lastWritePosition, writeToPosition);
						lastWritePosition = num;
					}
				}
				else if (this._chars[num - 1] == quote)
				{
					goto Block_24;
				}
			}
			goto IL_2C8;
			Block_24:
			this.FinishReadStringIntoBuffer(num - 1, charPos, lastWritePosition);
			return;
			IL_289:
			this._charPos = num;
			throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
			IL_2AC:
			throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
			IL_2C8:
			this._charPos = num;
			throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, "\\" + c2.ToString()));
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00052338 File Offset: 0x00050538
		private void FinishReadStringIntoBuffer(int charPos, int initialPosition, int lastWritePosition)
		{
			if (initialPosition == lastWritePosition)
			{
				this._stringReference = new StringReference(this._chars, initialPosition, charPos - initialPosition);
			}
			else
			{
				this.EnsureBufferNotEmpty();
				if (charPos > lastWritePosition)
				{
					this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, charPos - lastWritePosition);
				}
				this._stringReference = new StringReference(this._stringBuffer.InternalBuffer, 0, this._stringBuffer.Position);
			}
			this._charPos = charPos + 1;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0000FC27 File Offset: 0x0000DE27
		private void WriteCharToBuffer(char writeChar, int lastWritePosition, int writeToPosition)
		{
			if (writeToPosition > lastWritePosition)
			{
				this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, writeToPosition - lastWritePosition);
			}
			this._stringBuffer.Append(this._arrayPool, writeChar);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000523B0 File Offset: 0x000505B0
		private char ConvertUnicode(bool enoughChars)
		{
			if (!enoughChars)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing Unicode escape sequence.");
			}
			int value;
			if (!ConvertUtils.TryHexTextToInt(this._chars, this._charPos, this._charPos + 4, out value))
			{
				throw JsonReaderException.Create(this, "Invalid Unicode escape sequence: \\u{0}.".FormatWith(CultureInfo.InvariantCulture, new string(this._chars, this._charPos, 4)));
			}
			char result = Convert.ToChar(value);
			this._charPos += 4;
			return result;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0000FC5A File Offset: 0x0000DE5A
		private char ParseUnicode()
		{
			return this.ConvertUnicode(this.EnsureChars(4, true));
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00052428 File Offset: 0x00050628
		private void ReadNumberIntoBuffer()
		{
			int num = this._charPos;
			for (;;)
			{
				char c = this._chars[num];
				if (c == '\0')
				{
					this._charPos = num;
					if (this._charsUsed != num)
					{
						return;
					}
					if (this.ReadData(true) == 0)
					{
						break;
					}
				}
				else
				{
					if (this.ReadNumberCharIntoBuffer(c, num))
					{
						return;
					}
					num++;
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00052478 File Offset: 0x00050678
		private bool ReadNumberCharIntoBuffer(char currentChar, int charPos)
		{
			if (currentChar <= 'X')
			{
				switch (currentChar)
				{
				case '+':
				case '-':
				case '.':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
					return false;
				case ',':
				case '/':
				case ':':
				case ';':
				case '<':
				case '=':
				case '>':
				case '?':
				case '@':
					break;
				default:
					if (currentChar == 'X')
					{
						return false;
					}
					break;
				}
			}
			else
			{
				switch (currentChar)
				{
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
					return false;
				default:
					if (currentChar == 'x')
					{
						return false;
					}
					break;
				}
			}
			this._charPos = charPos;
			if (!char.IsWhiteSpace(currentChar) && currentChar != ',' && currentChar != '}' && currentChar != ']' && currentChar != ')')
			{
				if (currentChar != '/')
				{
					throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
				}
			}
			return true;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0000FC6A File Offset: 0x0000DE6A
		private void ClearRecentString()
		{
			this._stringBuffer.Position = 0;
			this._stringReference = default(StringReference);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00052580 File Offset: 0x00050780
		private bool ParsePostValue(bool ignoreComments)
		{
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				if (c <= ')')
				{
					if (c <= '\r')
					{
						if (c != '\0')
						{
							switch (c)
							{
							case '\t':
								break;
							case '\n':
								this.ProcessLineFeed();
								continue;
							case '\v':
							case '\f':
								goto IL_49;
							case '\r':
								this.ProcessCarriageReturn(false);
								continue;
							default:
								goto IL_49;
							}
						}
						else
						{
							if (this._charsUsed != this._charPos)
							{
								this._charPos++;
								continue;
							}
							if (this.ReadData(false) == 0)
							{
								goto Block_11;
							}
							continue;
						}
					}
					else if (c != ' ')
					{
						if (c != ')')
						{
							goto IL_49;
						}
						goto IL_FD;
					}
					this._charPos++;
					continue;
				}
				if (c <= '/')
				{
					if (c == ',')
					{
						goto IL_117;
					}
					if (c == '/')
					{
						this.ParseComment(!ignoreComments);
						if (!ignoreComments)
						{
							break;
						}
						continue;
					}
				}
				else
				{
					if (c == ']')
					{
						goto IL_179;
					}
					if (c == '}')
					{
						goto IL_161;
					}
				}
				IL_49:
				if (!char.IsWhiteSpace(c))
				{
					goto IL_12D;
				}
				this._charPos++;
			}
			return true;
			Block_11:
			this._currentState = JsonReader.State.Finished;
			return false;
			IL_FD:
			this._charPos++;
			base.SetToken(JsonToken.EndConstructor);
			return true;
			IL_117:
			this._charPos++;
			base.SetStateBasedOnCurrent();
			return false;
			IL_12D:
			if (base.SupportMultipleContent && this.Depth == 0)
			{
				base.SetStateBasedOnCurrent();
				return false;
			}
			throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
			IL_161:
			this._charPos++;
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_179:
			this._charPos++;
			base.SetToken(JsonToken.EndArray);
			return true;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00052720 File Offset: 0x00050920
		private bool ParseObject()
		{
			for (;;)
			{
				char c = this._chars[this._charPos];
				if (c > '\r')
				{
					if (c == ' ')
					{
						goto IL_43;
					}
					if (c == '/')
					{
						goto IL_D9;
					}
					if (c == '}')
					{
						goto IL_C1;
					}
				}
				else if (c != '\0')
				{
					switch (c)
					{
					case '\t':
						goto IL_43;
					case '\n':
						this.ProcessLineFeed();
						continue;
					case '\r':
						this.ProcessCarriageReturn(false);
						continue;
					}
				}
				else
				{
					if (this._charsUsed != this._charPos)
					{
						this._charPos++;
						continue;
					}
					if (this.ReadData(false) == 0)
					{
						break;
					}
					continue;
				}
				if (char.IsWhiteSpace(c))
				{
					this._charPos++;
					continue;
				}
				goto IL_BA;
				IL_43:
				this._charPos++;
			}
			return false;
			IL_BA:
			return this.ParseProperty();
			IL_C1:
			base.SetToken(JsonToken.EndObject);
			this._charPos++;
			return true;
			IL_D9:
			this.ParseComment(true);
			return true;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00052810 File Offset: 0x00050A10
		private bool ParseProperty()
		{
			char c = this._chars[this._charPos];
			char c2;
			if (c != '"')
			{
				if (c != '\'')
				{
					if (this.ValidIdentifierChar(c))
					{
						c2 = '\0';
						this.ShiftBufferIfNeeded();
						this.ParseUnquotedProperty();
						goto IL_78;
					}
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
			}
			this._charPos++;
			c2 = c;
			this.ShiftBufferIfNeeded();
			this.ReadStringIntoBuffer(c2);
			IL_78:
			string text;
			if (this.PropertyNameTable != null)
			{
				text = this.PropertyNameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length);
				if (text == null)
				{
					text = this._stringReference.ToString();
				}
			}
			else
			{
				text = this._stringReference.ToString();
			}
			this.EatWhitespace();
			if (this._chars[this._charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			base.SetToken(JsonToken.PropertyName, text);
			this._quoteChar = c2;
			this.ClearRecentString();
			return true;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0000FC84 File Offset: 0x0000DE84
		private bool ValidIdentifierChar(char value)
		{
			return char.IsLetterOrDigit(value) || value == '_' || value == '$';
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00052958 File Offset: 0x00050B58
		private void ParseUnquotedProperty()
		{
			int charPos = this._charPos;
			for (;;)
			{
				char c = this._chars[this._charPos];
				if (c == '\0')
				{
					if (this._charsUsed != this._charPos)
					{
						goto IL_4B;
					}
					if (this.ReadData(true) == 0)
					{
						goto IL_3F;
					}
				}
				else if (this.ReadUnquotedPropertyReportIfDone(c, charPos))
				{
					break;
				}
			}
			return;
			IL_3F:
			throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
			IL_4B:
			this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000529CC File Offset: 0x00050BCC
		private bool ReadUnquotedPropertyReportIfDone(char currentChar, int initialPosition)
		{
			if (this.ValidIdentifierChar(currentChar))
			{
				this._charPos++;
				return false;
			}
			if (!char.IsWhiteSpace(currentChar))
			{
				if (currentChar != ':')
				{
					throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, currentChar));
				}
			}
			this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
			return true;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00052A38 File Offset: 0x00050C38
		private bool ParseValue()
		{
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				if (c > 'N')
				{
					if (c <= 'f')
					{
						if (c == '[')
						{
							goto IL_1E5;
						}
						if (c == ']')
						{
							goto IL_1CD;
						}
						if (c == 'f')
						{
							goto IL_1C5;
						}
					}
					else if (c <= 't')
					{
						if (c == 'n')
						{
							goto IL_204;
						}
						if (c == 't')
						{
							goto IL_1FC;
						}
					}
					else
					{
						if (c == 'u')
						{
							goto IL_29D;
						}
						if (c == '{')
						{
							goto IL_286;
						}
					}
				}
				else if (c <= ' ')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
							this.ProcessLineFeed();
							continue;
						case '\v':
						case '\f':
							goto IL_59;
						case '\r':
							this.ProcessCarriageReturn(false);
							continue;
						default:
							if (c != ' ')
							{
								goto IL_59;
							}
							break;
						}
						this._charPos++;
						continue;
					}
					if (this._charsUsed != this._charPos)
					{
						this._charPos++;
						continue;
					}
					if (this.ReadData(false) == 0)
					{
						break;
					}
					continue;
				}
				else if (c <= '/')
				{
					if (c == '"')
					{
						goto IL_1A7;
					}
					switch (c)
					{
					case '\'':
						goto IL_1A7;
					case ')':
						goto IL_14C;
					case ',':
						goto IL_164;
					case '-':
						goto IL_16E;
					case '/':
						goto IL_19E;
					}
				}
				else
				{
					if (c == 'I')
					{
						goto IL_1BB;
					}
					if (c == 'N')
					{
						goto IL_1B1;
					}
				}
				IL_59:
				if (!char.IsWhiteSpace(c))
				{
					goto IL_261;
				}
				this._charPos++;
			}
			return false;
			IL_14C:
			this._charPos++;
			base.SetToken(JsonToken.EndConstructor);
			return true;
			IL_164:
			base.SetToken(JsonToken.Undefined);
			return true;
			IL_16E:
			if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
			{
				this.ParseNumberNegativeInfinity(ReadType.Read);
			}
			else
			{
				this.ParseNumber(ReadType.Read);
			}
			return true;
			IL_19E:
			this.ParseComment(true);
			return true;
			IL_1A7:
			this.ParseString(c, ReadType.Read);
			return true;
			IL_1B1:
			this.ParseNumberNaN(ReadType.Read);
			return true;
			IL_1BB:
			this.ParseNumberPositiveInfinity(ReadType.Read);
			return true;
			IL_1C5:
			this.ParseFalse();
			return true;
			IL_1CD:
			this._charPos++;
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_1E5:
			this._charPos++;
			base.SetToken(JsonToken.StartArray);
			return true;
			IL_1FC:
			this.ParseTrue();
			return true;
			IL_204:
			if (this.EnsureChars(1, true))
			{
				char c2 = this._chars[this._charPos + 1];
				if (c2 == 'u')
				{
					this.ParseNull();
				}
				else
				{
					if (c2 != 'e')
					{
						throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
					}
					this.ParseConstructor();
				}
				return true;
			}
			this._charPos++;
			throw base.CreateUnexpectedEndException();
			IL_261:
			if (!char.IsNumber(c) && c != '-')
			{
				if (c != '.')
				{
					throw this.CreateUnexpectedCharacterException(c);
				}
			}
			this.ParseNumber(ReadType.Read);
			return true;
			IL_286:
			this._charPos++;
			base.SetToken(JsonToken.StartObject);
			return true;
			IL_29D:
			this.ParseUndefined();
			return true;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0000FC9A File Offset: 0x0000DE9A
		private void ProcessLineFeed()
		{
			this._charPos++;
			this.OnNewLine(this._charPos);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0000FCB6 File Offset: 0x0000DEB6
		private void ProcessCarriageReturn(bool append)
		{
			this._charPos++;
			this.SetNewLine(this.EnsureChars(1, append));
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00052CEC File Offset: 0x00050EEC
		private void EatWhitespace()
		{
			for (;;)
			{
				char c = this._chars[this._charPos];
				if (c == '\0')
				{
					if (this._charsUsed == this._charPos)
					{
						if (this.ReadData(false) == 0)
						{
							break;
						}
					}
					else
					{
						this._charPos++;
					}
				}
				else if (c != '\n')
				{
					if (c != '\r')
					{
						if (c != ' ' && !char.IsWhiteSpace(c))
						{
							return;
						}
						this._charPos++;
					}
					else
					{
						this.ProcessCarriageReturn(false);
					}
				}
				else
				{
					this.ProcessLineFeed();
				}
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00052D6C File Offset: 0x00050F6C
		private void ParseConstructor()
		{
			if (!this.MatchValueWithTrailingSeparator("new"))
			{
				throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
			}
			this.EatWhitespace();
			int charPos = this._charPos;
			char c;
			for (;;)
			{
				c = this._chars[this._charPos];
				if (c == '\0')
				{
					if (this._charsUsed != this._charPos)
					{
						goto IL_6D;
					}
					if (this.ReadData(true) == 0)
					{
						break;
					}
				}
				else
				{
					if (!char.IsLetterOrDigit(c))
					{
						goto IL_84;
					}
					this._charPos++;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
			IL_6D:
			int charPos2 = this._charPos;
			this._charPos++;
			goto IL_D8;
			IL_84:
			if (c == '\r')
			{
				charPos2 = this._charPos;
				this.ProcessCarriageReturn(true);
			}
			else if (c == '\n')
			{
				charPos2 = this._charPos;
				this.ProcessLineFeed();
			}
			else if (char.IsWhiteSpace(c))
			{
				charPos2 = this._charPos;
				this._charPos++;
			}
			else
			{
				if (c != '(')
				{
					throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
				charPos2 = this._charPos;
			}
			IL_D8:
			this._stringReference = new StringReference(this._chars, charPos, charPos2 - charPos);
			string value = this._stringReference.ToString();
			this.EatWhitespace();
			if (this._chars[this._charPos] != '(')
			{
				throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
			}
			this._charPos++;
			this.ClearRecentString();
			base.SetToken(JsonToken.StartConstructor, value);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00052EFC File Offset: 0x000510FC
		private void ParseNumber(ReadType readType)
		{
			this.ShiftBufferIfNeeded();
			char firstChar = this._chars[this._charPos];
			int charPos = this._charPos;
			this.ReadNumberIntoBuffer();
			this.ParseReadNumber(readType, firstChar, charPos);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00052F34 File Offset: 0x00051134
		private void ParseReadNumber(ReadType readType, char firstChar, int initialPosition)
		{
			base.SetPostValueState(true);
			this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
			bool flag = char.IsDigit(firstChar) && this._stringReference.Length == 1;
			bool flag2 = firstChar == '0' && this._stringReference.Length > 1 && this._stringReference.Chars[this._stringReference.StartIndex + 1] != '.' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'e' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'E';
			object value;
			JsonToken newToken;
			switch (readType)
			{
			case ReadType.Read:
			case ReadType.ReadAsInt64:
			{
				if (flag)
				{
					value = (long)((ulong)firstChar - 48UL);
					newToken = JsonToken.Integer;
					goto IL_62D;
				}
				if (flag2)
				{
					string text = this._stringReference.ToString();
					try
					{
						value = (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text, 16) : Convert.ToInt64(text, 8));
					}
					catch (Exception ex)
					{
						throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text), ex);
					}
					newToken = JsonToken.Integer;
					goto IL_62D;
				}
				long num;
				ParseResult parseResult = ConvertUtils.Int64TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num);
				if (parseResult == ParseResult.Success)
				{
					value = num;
					newToken = JsonToken.Integer;
					goto IL_62D;
				}
				if (parseResult != ParseResult.Overflow)
				{
					if (this._floatParseHandling == FloatParseHandling.Decimal)
					{
						decimal num2;
						parseResult = ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num2);
						if (parseResult != ParseResult.Success)
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						value = num2;
					}
					else
					{
						double num3;
						if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out num3))
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						value = num3;
					}
					newToken = JsonToken.Float;
					goto IL_62D;
				}
				string text2 = this._stringReference.ToString();
				if (text2.Length > 380)
				{
					throw this.ThrowReaderError("JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
				}
				value = JsonTextReader.BigIntegerParse(text2, CultureInfo.InvariantCulture);
				newToken = JsonToken.Integer;
				goto IL_62D;
			}
			case ReadType.ReadAsInt32:
				if (flag)
				{
					value = (int)(firstChar - '0');
				}
				else
				{
					if (flag2)
					{
						string text3 = this._stringReference.ToString();
						try
						{
							value = (text3.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(text3, 16) : Convert.ToInt32(text3, 8));
							goto IL_183;
						}
						catch (Exception ex2)
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, text3), ex2);
						}
					}
					int num4;
					ParseResult parseResult2 = ConvertUtils.Int32TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num4);
					if (parseResult2 == ParseResult.Success)
					{
						value = num4;
					}
					else
					{
						if (parseResult2 == ParseResult.Overflow)
						{
							throw this.ThrowReaderError("JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
						}
						throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
					}
				}
				IL_183:
				newToken = JsonToken.Integer;
				goto IL_62D;
			case ReadType.ReadAsString:
			{
				string text4 = this._stringReference.ToString();
				if (flag2)
				{
					try
					{
						if (text4.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
						{
							Convert.ToInt64(text4, 16);
						}
						else
						{
							Convert.ToInt64(text4, 8);
						}
						goto IL_457;
					}
					catch (Exception ex3)
					{
						throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text4), ex3);
					}
				}
				double num5;
				if (!double.TryParse(text4, NumberStyles.Float, CultureInfo.InvariantCulture, out num5))
				{
					throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
				}
				IL_457:
				newToken = JsonToken.String;
				value = text4;
				goto IL_62D;
			}
			case ReadType.ReadAsDecimal:
				if (flag)
				{
					value = firstChar - 48m;
				}
				else
				{
					if (flag2)
					{
						string text5 = this._stringReference.ToString();
						try
						{
							value = Convert.ToDecimal(text5.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text5, 16) : Convert.ToInt64(text5, 8));
							goto IL_51A;
						}
						catch (Exception ex4)
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text5), ex4);
						}
					}
					decimal num6;
					if (ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num6) != ParseResult.Success)
					{
						throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
					}
					value = num6;
				}
				IL_51A:
				newToken = JsonToken.Float;
				goto IL_62D;
			case ReadType.ReadAsDouble:
				if (flag)
				{
					value = (double)firstChar - 48.0;
				}
				else
				{
					if (flag2)
					{
						string text6 = this._stringReference.ToString();
						try
						{
							value = Convert.ToDouble(text6.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text6, 16) : Convert.ToInt64(text6, 8));
							goto IL_600;
						}
						catch (Exception ex5)
						{
							throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, text6), ex5);
						}
					}
					double num7;
					if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out num7))
					{
						throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith(CultureInfo.InvariantCulture, this._stringReference.ToString()), null);
					}
					value = num7;
				}
				IL_600:
				newToken = JsonToken.Float;
				goto IL_62D;
			}
			throw JsonReaderException.Create(this, "Cannot read number value as type.");
			IL_62D:
			this.ClearRecentString();
			base.SetToken(newToken, value, false);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		private JsonReaderException ThrowReaderError(string message, [Nullable(2)] Exception ex = null)
		{
			base.SetToken(JsonToken.Undefined, null, false);
			return JsonReaderException.Create(this, message, ex);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
		private static object BigIntegerParse(string number, CultureInfo culture)
		{
			return System.Numerics.BigInteger.Parse(number, culture);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000535C0 File Offset: 0x000517C0
		private void ParseComment(bool setToken)
		{
			this._charPos++;
			if (!this.EnsureChars(1, false))
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			bool flag;
			if (this._chars[this._charPos] == '*')
			{
				flag = false;
			}
			else
			{
				if (this._chars[this._charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, this._chars[this._charPos]));
				}
				flag = true;
			}
			this._charPos++;
			int charPos = this._charPos;
			for (;;)
			{
				char c = this._chars[this._charPos];
				if (c <= '\n')
				{
					if (c != '\0')
					{
						if (c == '\n')
						{
							if (!flag)
							{
								this.ProcessLineFeed();
								continue;
							}
							goto IL_117;
						}
					}
					else
					{
						if (this._charsUsed != this._charPos)
						{
							this._charPos++;
							continue;
						}
						if (this.ReadData(true) == 0)
						{
							goto Block_13;
						}
						continue;
					}
				}
				else if (c != '\r')
				{
					if (c == '*')
					{
						this._charPos++;
						if (!flag && this.EnsureChars(0, true) && this._chars[this._charPos] == '/')
						{
							break;
						}
						continue;
					}
				}
				else
				{
					if (!flag)
					{
						this.ProcessCarriageReturn(true);
						continue;
					}
					goto IL_18B;
				}
				this._charPos++;
			}
			this.EndComment(setToken, charPos, this._charPos - 1);
			this._charPos++;
			return;
			Block_13:
			if (!flag)
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			this.EndComment(setToken, charPos, this._charPos);
			return;
			IL_117:
			this.EndComment(setToken, charPos, this._charPos);
			return;
			IL_18B:
			this.EndComment(setToken, charPos, this._charPos);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000FCF6 File Offset: 0x0000DEF6
		private void EndComment(bool setToken, int initialPosition, int endPosition)
		{
			if (setToken)
			{
				base.SetToken(JsonToken.Comment, new string(this._chars, initialPosition, endPosition - initialPosition));
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0000FD11 File Offset: 0x0000DF11
		private bool MatchValue(string value)
		{
			return this.MatchValue(this.EnsureChars(value.Length - 1, true), value);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00053768 File Offset: 0x00051968
		private bool MatchValue(bool enoughChars, string value)
		{
			if (!enoughChars)
			{
				this._charPos = this._charsUsed;
				throw base.CreateUnexpectedEndException();
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (this._chars[this._charPos + i] != value[i])
				{
					this._charPos += i;
					return false;
				}
			}
			this._charPos += value.Length;
			return true;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0000FD29 File Offset: 0x0000DF29
		private bool MatchValueWithTrailingSeparator(string value)
		{
			return this.MatchValue(value) && (!this.EnsureChars(0, false) || this.IsSeparator(this._chars[this._charPos]) || this._chars[this._charPos] == '\0');
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000537DC File Offset: 0x000519DC
		private bool IsSeparator(char c)
		{
			if (c <= ')')
			{
				switch (c)
				{
				case '\t':
				case '\n':
				case '\r':
					break;
				case '\v':
				case '\f':
					goto IL_8C;
				default:
					if (c != ' ')
					{
						if (c != ')')
						{
							goto IL_8C;
						}
						if (base.CurrentState == JsonReader.State.Constructor || base.CurrentState == JsonReader.State.ConstructorStart)
						{
							return true;
						}
						return false;
					}
					break;
				}
				return true;
			}
			if (c <= '/')
			{
				if (c != ',')
				{
					if (c != '/')
					{
						goto IL_8C;
					}
					if (!this.EnsureChars(1, false))
					{
						return false;
					}
					char c2 = this._chars[this._charPos + 1];
					return c2 == '*' || c2 == '/';
				}
			}
			else if (c != ']')
			{
				if (c != '}')
				{
					goto IL_8C;
				}
			}
			return true;
			IL_8C:
			if (char.IsWhiteSpace(c))
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0000FD69 File Offset: 0x0000DF69
		private void ParseTrue()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.True))
			{
				throw JsonReaderException.Create(this, "Error parsing boolean value.");
			}
			base.SetToken(JsonToken.Boolean, true);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0000FD92 File Offset: 0x0000DF92
		private void ParseNull()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.Null))
			{
				throw JsonReaderException.Create(this, "Error parsing null value.");
			}
			base.SetToken(JsonToken.Null);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0000FDB5 File Offset: 0x0000DFB5
		private void ParseUndefined()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.Undefined))
			{
				throw JsonReaderException.Create(this, "Error parsing undefined value.");
			}
			base.SetToken(JsonToken.Undefined);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		private void ParseFalse()
		{
			if (!this.MatchValueWithTrailingSeparator(JsonConvert.False))
			{
				throw JsonReaderException.Create(this, "Error parsing boolean value.");
			}
			base.SetToken(JsonToken.Boolean, false);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0000FE01 File Offset: 0x0000E001
		private object ParseNumberNegativeInfinity(ReadType readType)
		{
			return this.ParseNumberNegativeInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity));
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00053884 File Offset: 0x00051A84
		private object ParseNumberNegativeInfinity(ReadType readType, bool matched)
		{
			if (matched)
			{
				if (readType != ReadType.Read)
				{
					if (readType == ReadType.ReadAsString)
					{
						base.SetToken(JsonToken.String, JsonConvert.NegativeInfinity);
						return JsonConvert.NegativeInfinity;
					}
					if (readType != ReadType.ReadAsDouble)
					{
						goto IL_2B;
					}
				}
				if (this._floatParseHandling == FloatParseHandling.Double)
				{
					base.SetToken(JsonToken.Float, double.NegativeInfinity);
					return double.NegativeInfinity;
				}
				IL_2B:
				throw JsonReaderException.Create(this, "Cannot read -Infinity value.");
			}
			throw JsonReaderException.Create(this, "Error parsing -Infinity value.");
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0000FE15 File Offset: 0x0000E015
		private object ParseNumberPositiveInfinity(ReadType readType)
		{
			return this.ParseNumberPositiveInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity));
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000538F8 File Offset: 0x00051AF8
		private object ParseNumberPositiveInfinity(ReadType readType, bool matched)
		{
			if (matched)
			{
				if (readType != ReadType.Read)
				{
					if (readType == ReadType.ReadAsString)
					{
						base.SetToken(JsonToken.String, JsonConvert.PositiveInfinity);
						return JsonConvert.PositiveInfinity;
					}
					if (readType != ReadType.ReadAsDouble)
					{
						goto IL_2B;
					}
				}
				if (this._floatParseHandling == FloatParseHandling.Double)
				{
					base.SetToken(JsonToken.Float, double.PositiveInfinity);
					return double.PositiveInfinity;
				}
				IL_2B:
				throw JsonReaderException.Create(this, "Cannot read Infinity value.");
			}
			throw JsonReaderException.Create(this, "Error parsing Infinity value.");
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0000FE29 File Offset: 0x0000E029
		private object ParseNumberNaN(ReadType readType)
		{
			return this.ParseNumberNaN(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NaN));
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0005396C File Offset: 0x00051B6C
		private object ParseNumberNaN(ReadType readType, bool matched)
		{
			if (matched)
			{
				if (readType != ReadType.Read)
				{
					if (readType == ReadType.ReadAsString)
					{
						base.SetToken(JsonToken.String, JsonConvert.NaN);
						return JsonConvert.NaN;
					}
					if (readType != ReadType.ReadAsDouble)
					{
						goto IL_2B;
					}
				}
				if (this._floatParseHandling == FloatParseHandling.Double)
				{
					base.SetToken(JsonToken.Float, double.NaN);
					return double.NaN;
				}
				IL_2B:
				throw JsonReaderException.Create(this, "Cannot read NaN value.");
			}
			throw JsonReaderException.Create(this, "Error parsing NaN value.");
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x000539E0 File Offset: 0x00051BE0
		public override void Close()
		{
			base.Close();
			if (this._chars != null)
			{
				BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
				this._chars = null;
			}
			if (base.CloseInput)
			{
				TextReader reader = this._reader;
				if (reader != null)
				{
					reader.Close();
				}
			}
			this._stringBuffer.Clear(this._arrayPool);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00009F16 File Offset: 0x00008116
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0000FE3D File Offset: 0x0000E03D
		public int LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start && this.LinePosition == 0 && this.TokenType != JsonToken.Comment)
				{
					return 0;
				}
				return this._lineNumber;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0000FE60 File Offset: 0x0000E060
		public int LinePosition
		{
			get
			{
				return this._charPos - this._lineStartPos;
			}
		}

		// Token: 0x0400085E RID: 2142
		private const char UnicodeReplacementChar = '�';

		// Token: 0x0400085F RID: 2143
		private const int MaximumJavascriptIntegerCharacterLength = 380;

		// Token: 0x04000860 RID: 2144
		private const int LargeBufferLength = 1073741823;

		// Token: 0x04000861 RID: 2145
		private readonly TextReader _reader;

		// Token: 0x04000862 RID: 2146
		[Nullable(2)]
		private char[] _chars;

		// Token: 0x04000863 RID: 2147
		private int _charsUsed;

		// Token: 0x04000864 RID: 2148
		private int _charPos;

		// Token: 0x04000865 RID: 2149
		private int _lineStartPos;

		// Token: 0x04000866 RID: 2150
		private int _lineNumber;

		// Token: 0x04000867 RID: 2151
		private bool _isEndOfFile;

		// Token: 0x04000868 RID: 2152
		private StringBuffer _stringBuffer;

		// Token: 0x04000869 RID: 2153
		private StringReference _stringReference;

		// Token: 0x0400086A RID: 2154
		[Nullable(2)]
		private IArrayPool<char> _arrayPool;
	}
}
