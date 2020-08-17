using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001C6 RID: 454
	[NullableContext(2)]
	[Nullable(0)]
	public abstract class JsonReader : IDisposable
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0000EF4F File Offset: 0x0000D14F
		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0000EF57 File Offset: 0x0000D157
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0000EF5F File Offset: 0x0000D15F
		public bool CloseInput { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0000EF68 File Offset: 0x0000D168
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0000EF70 File Offset: 0x0000D170
		public bool SupportMultipleContent { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0000EF79 File Offset: 0x0000D179
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0000EF81 File Offset: 0x0000D181
		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0000EF8A File Offset: 0x0000D18A
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0000EF92 File Offset: 0x0000D192
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._dateTimeZoneHandling;
			}
			set
			{
				if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0000EFAE File Offset: 0x0000D1AE
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0000EFB6 File Offset: 0x0000D1B6
		public DateParseHandling DateParseHandling
		{
			get
			{
				return this._dateParseHandling;
			}
			set
			{
				if (value < DateParseHandling.None || value > DateParseHandling.DateTimeOffset)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateParseHandling = value;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0000EFD2 File Offset: 0x0000D1D2
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0000EFDA File Offset: 0x0000D1DA
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling;
			}
			set
			{
				if (value < FloatParseHandling.Double || value > FloatParseHandling.Decimal)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatParseHandling = value;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0000EFF6 File Offset: 0x0000D1F6
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0000EFFE File Offset: 0x0000D1FE
		public string DateFormatString
		{
			get
			{
				return this._dateFormatString;
			}
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0000F007 File Offset: 0x0000D207
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0004EF30 File Offset: 0x0004D130
		public int? MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
			set
			{
				int? num = value;
				if (num.GetValueOrDefault() <= 0 & num != null)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				this._maxDepth = value;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0000F00F File Offset: 0x0000D20F
		public virtual JsonToken TokenType
		{
			get
			{
				return this._tokenType;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0000F017 File Offset: 0x0000D217
		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0000F01F File Offset: 0x0000D21F
		public virtual Type ValueType
		{
			get
			{
				object value = this._value;
				if (value == null)
				{
					return null;
				}
				return value.GetType();
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x0004EF70 File Offset: 0x0004D170
		public virtual int Depth
		{
			get
			{
				List<JsonPosition> stack = this._stack;
				int num = (stack != null) ? stack.Count : 0;
				if (!JsonTokenUtils.IsStartToken(this.TokenType) && this._currentPosition.Type != JsonContainerType.None)
				{
					return num + 1;
				}
				return num;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0004EFB0 File Offset: 0x0004D1B0
		[Nullable(1)]
		public virtual string Path
		{
			[NullableContext(1)]
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? currentPosition = (this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.ConstructorStart && this._currentState != JsonReader.State.ObjectStart) ? new JsonPosition?(this._currentPosition) : null;
				return JsonPosition.BuildPath(this._stack, currentPosition);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0000F032 File Offset: 0x0000D232
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x0000F043 File Offset: 0x0000D243
		[Nullable(1)]
		public CultureInfo Culture
		{
			[NullableContext(1)]
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			[NullableContext(1)]
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0000F04C File Offset: 0x0000D24C
		internal JsonPosition GetPosition(int depth)
		{
			if (this._stack != null && depth < this._stack.Count)
			{
				return this._stack[depth];
			}
			return this._currentPosition;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0000F077 File Offset: 0x0000D277
		protected JsonReader()
		{
			this._currentState = JsonReader.State.Start;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this._dateParseHandling = DateParseHandling.DateTime;
			this._floatParseHandling = FloatParseHandling.Double;
			this.CloseInput = true;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0004F018 File Offset: 0x0004D218
		private void Push(JsonContainerType value)
		{
			this.UpdateScopeWithFinishedValue();
			if (this._currentPosition.Type == JsonContainerType.None)
			{
				this._currentPosition = new JsonPosition(value);
				return;
			}
			if (this._stack == null)
			{
				this._stack = new List<JsonPosition>();
			}
			this._stack.Add(this._currentPosition);
			this._currentPosition = new JsonPosition(value);
			if (this._maxDepth != null)
			{
				int num = this.Depth + 1;
				int? maxDepth = this._maxDepth;
				if ((num > maxDepth.GetValueOrDefault() & maxDepth != null) && !this._hasExceededMaxDepth)
				{
					this._hasExceededMaxDepth = true;
					throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, this._maxDepth));
				}
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0004F0D4 File Offset: 0x0004D2D4
		private JsonContainerType Pop()
		{
			JsonPosition currentPosition;
			if (this._stack != null && this._stack.Count > 0)
			{
				currentPosition = this._currentPosition;
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				currentPosition = this._currentPosition;
				this._currentPosition = default(JsonPosition);
			}
			if (this._maxDepth != null)
			{
				int depth = this.Depth;
				int? maxDepth = this._maxDepth;
				if (depth <= maxDepth.GetValueOrDefault() & maxDepth != null)
				{
					this._hasExceededMaxDepth = false;
				}
			}
			return currentPosition.Type;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0000F0A2 File Offset: 0x0000D2A2
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x06000BFE RID: 3070
		public abstract bool Read();

		// Token: 0x06000BFF RID: 3071 RVA: 0x0004F188 File Offset: 0x0004D388
		public virtual int? ReadAsInt32()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					int num;
					if (value is int)
					{
						num = (int)value;
						return new int?(num);
					}
					if (value is System.Numerics.BigInteger)
					{
						System.Numerics.BigInteger value2 = (System.Numerics.BigInteger)value;
						num = (int)value2;
					}
					else
					{
						try
						{
							num = Convert.ToInt32(value, CultureInfo.InvariantCulture);
						}
						catch (Exception ex)
						{
							throw JsonReaderException.Create(this, "Could not convert to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, value), ex);
						}
					}
					this.SetToken(JsonToken.Integer, num, false);
					return new int?(num);
				}
				case JsonToken.String:
				{
					string s = (string)this.Value;
					return this.ReadInt32String(s);
				}
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_E2;
				}
				throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_E2:
			return null;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0004F290 File Offset: 0x0004D490
		internal int? ReadInt32String(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			int num;
			if (!int.TryParse(s, NumberStyles.Integer, this.Culture, out num))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Integer, num, false);
			return new int?(num);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0004F300 File Offset: 0x0004D500
		public virtual string ReadAsString()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken == JsonToken.None)
				{
					goto IL_A1;
				}
				if (contentToken == JsonToken.String)
				{
					return (string)this.Value;
				}
			}
			else
			{
				if (contentToken == JsonToken.Null)
				{
					goto IL_A1;
				}
				if (contentToken == JsonToken.EndArray)
				{
					goto IL_A1;
				}
			}
			if (JsonTokenUtils.IsPrimitiveToken(contentToken))
			{
				object value = this.Value;
				if (value != null)
				{
					IFormattable formattable = value as IFormattable;
					string text;
					if (formattable != null)
					{
						text = formattable.ToString(null, this.Culture);
					}
					else
					{
						Uri uri = value as Uri;
						text = ((uri != null) ? uri.OriginalString : value.ToString());
					}
					this.SetToken(JsonToken.String, text, false);
					return text;
				}
			}
			throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			IL_A1:
			return null;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004F3B0 File Offset: 0x0004D5B0
		public virtual byte[] ReadAsBytes()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				switch (contentToken)
				{
				case JsonToken.None:
					goto IL_127;
				case JsonToken.StartObject:
				{
					this.ReadIntoWrappedTypeObject();
					byte[] array = this.ReadAsBytes();
					this.ReaderReadAndAssert();
					if (this.TokenType != JsonToken.EndObject)
					{
						throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
					}
					this.SetToken(JsonToken.Bytes, array, false);
					return array;
				}
				case JsonToken.StartArray:
					return this.ReadArrayIntoByteArray();
				default:
					if (contentToken == JsonToken.String)
					{
						string text = (string)this.Value;
						byte[] array2;
						Guid guid;
						if (text.Length == 0)
						{
							array2 = CollectionUtils.ArrayEmpty<byte>();
						}
						else if (ConvertUtils.TryConvertGuid(text, out guid))
						{
							array2 = guid.ToByteArray();
						}
						else
						{
							array2 = Convert.FromBase64String(text);
						}
						this.SetToken(JsonToken.Bytes, array2, false);
						return array2;
					}
					break;
				}
			}
			else
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					goto IL_127;
				}
				if (contentToken == JsonToken.Bytes)
				{
					object value = this.Value;
					if (value is Guid)
					{
						byte[] array3 = ((Guid)value).ToByteArray();
						this.SetToken(JsonToken.Bytes, array3, false);
						return array3;
					}
					return (byte[])this.Value;
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			IL_127:
			return null;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0004F4E8 File Offset: 0x0004D6E8
		[NullableContext(1)]
		internal byte[] ReadArrayIntoByteArray()
		{
			List<byte> list = new List<byte>();
			do
			{
				if (!this.Read())
				{
					this.SetToken(JsonToken.None);
				}
			}
			while (!this.ReadArrayElementIntoByteArrayReportDone(list));
			byte[] array = list.ToArray();
			this.SetToken(JsonToken.Bytes, array, false);
			return array;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0004F530 File Offset: 0x0004D730
		[NullableContext(1)]
		private bool ReadArrayElementIntoByteArrayReportDone(List<byte> buffer)
		{
			JsonToken tokenType = this.TokenType;
			if (tokenType <= JsonToken.Comment)
			{
				if (tokenType == JsonToken.None)
				{
					throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
				}
				if (tokenType == JsonToken.Comment)
				{
					return false;
				}
			}
			else
			{
				if (tokenType == JsonToken.Integer)
				{
					buffer.Add(Convert.ToByte(this.Value, CultureInfo.InvariantCulture));
					return false;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return true;
				}
			}
			throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0004F5A4 File Offset: 0x0004D7A4
		public virtual double? ReadAsDouble()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					double num;
					if (value is double)
					{
						num = (double)value;
						return new double?(num);
					}
					if (value is System.Numerics.BigInteger)
					{
						System.Numerics.BigInteger value2 = (System.Numerics.BigInteger)value;
						num = (double)value2;
					}
					else
					{
						num = Convert.ToDouble(value, CultureInfo.InvariantCulture);
					}
					this.SetToken(JsonToken.Float, num, false);
					return new double?(num);
				}
				case JsonToken.String:
					return this.ReadDoubleString((string)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_BF;
				}
				throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_BF:
			return null;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0004F67C File Offset: 0x0004D87C
		internal double? ReadDoubleString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			double num;
			if (!double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, this.Culture, out num))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Float, num, false);
			return new double?(num);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0004F6F0 File Offset: 0x0004D8F0
		public virtual bool? ReadAsBoolean()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
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
					this.SetToken(JsonToken.Boolean, flag, false);
					return new bool?(flag);
				}
				case JsonToken.String:
					return this.ReadBooleanString((string)this.Value);
				case JsonToken.Boolean:
					return new bool?((bool)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_C9;
				}
				throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_C9:
			return null;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0004F7D0 File Offset: 0x0004D9D0
		internal bool? ReadBooleanString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			bool flag;
			if (!bool.TryParse(s, out flag))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Boolean, flag, false);
			return new bool?(flag);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0004F83C File Offset: 0x0004DA3C
		public virtual decimal? ReadAsDecimal()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken != JsonToken.None)
			{
				switch (contentToken)
				{
				case JsonToken.Integer:
				case JsonToken.Float:
				{
					object value = this.Value;
					decimal num;
					if (value is decimal)
					{
						num = (decimal)value;
						return new decimal?(num);
					}
					if (value is System.Numerics.BigInteger)
					{
						System.Numerics.BigInteger value2 = (System.Numerics.BigInteger)value;
						num = (decimal)value2;
					}
					else
					{
						try
						{
							num = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
						}
						catch (Exception ex)
						{
							throw JsonReaderException.Create(this, "Could not convert to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, value), ex);
						}
					}
					this.SetToken(JsonToken.Float, num, false);
					return new decimal?(num);
				}
				case JsonToken.String:
					return this.ReadDecimalString((string)this.Value);
				case JsonToken.Null:
				case JsonToken.EndArray:
					goto IL_DE;
				}
				throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			}
			IL_DE:
			return null;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0004F940 File Offset: 0x0004DB40
		internal decimal? ReadDecimalString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			decimal num;
			if (decimal.TryParse(s, NumberStyles.Number, this.Culture, out num))
			{
				this.SetToken(JsonToken.Float, num, false);
				return new decimal?(num);
			}
			if (ConvertUtils.DecimalTryParse(s.ToCharArray(), 0, s.Length, out num) == ParseResult.Success)
			{
				this.SetToken(JsonToken.Float, num, false);
				return new decimal?(num);
			}
			this.SetToken(JsonToken.String, s, false);
			throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0004F9E0 File Offset: 0x0004DBE0
		public virtual DateTime? ReadAsDateTime()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken == JsonToken.None)
				{
					goto IL_97;
				}
				if (contentToken == JsonToken.String)
				{
					return this.ReadDateTimeString((string)this.Value);
				}
			}
			else
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					goto IL_97;
				}
				if (contentToken == JsonToken.Date)
				{
					object value = this.Value;
					if (value is DateTimeOffset)
					{
						this.SetToken(JsonToken.Date, ((DateTimeOffset)value).DateTime, false);
					}
					return new DateTime?((DateTime)this.Value);
				}
			}
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, this.TokenType));
			IL_97:
			return null;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0004FA90 File Offset: 0x0004DC90
		internal DateTime? ReadDateTimeString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			DateTime dateTime;
			if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
			{
				dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
				this.SetToken(JsonToken.Date, dateTime, false);
				return new DateTime?(dateTime);
			}
			if (!DateTime.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
			{
				throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
			this.SetToken(JsonToken.Date, dateTime, false);
			return new DateTime?(dateTime);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0004FB48 File Offset: 0x0004DD48
		public virtual DateTimeOffset? ReadAsDateTimeOffset()
		{
			JsonToken contentToken = this.GetContentToken();
			if (contentToken <= JsonToken.String)
			{
				if (contentToken == JsonToken.None)
				{
					goto IL_95;
				}
				if (contentToken == JsonToken.String)
				{
					string s = (string)this.Value;
					return this.ReadDateTimeOffsetString(s);
				}
			}
			else
			{
				if (contentToken == JsonToken.Null || contentToken == JsonToken.EndArray)
				{
					goto IL_95;
				}
				if (contentToken == JsonToken.Date)
				{
					object value = this.Value;
					if (value is DateTime)
					{
						DateTime dateTime = (DateTime)value;
						this.SetToken(JsonToken.Date, new DateTimeOffset(dateTime), false);
					}
					return new DateTimeOffset?((DateTimeOffset)this.Value);
				}
			}
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, contentToken));
			IL_95:
			return null;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0004FBF4 File Offset: 0x0004DDF4
		internal DateTimeOffset? ReadDateTimeOffsetString(string s)
		{
			if (StringUtils.IsNullOrEmpty(s))
			{
				this.SetToken(JsonToken.Null, null, false);
				return null;
			}
			DateTimeOffset dateTimeOffset;
			if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
			{
				this.SetToken(JsonToken.Date, dateTimeOffset, false);
				return new DateTimeOffset?(dateTimeOffset);
			}
			if (!DateTimeOffset.TryParse(s, this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
			{
				this.SetToken(JsonToken.String, s, false);
				throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, s));
			}
			this.SetToken(JsonToken.Date, dateTimeOffset, false);
			return new DateTimeOffset?(dateTimeOffset);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0000F0AF File Offset: 0x0000D2AF
		internal void ReaderReadAndAssert()
		{
			if (!this.Read())
			{
				throw this.CreateUnexpectedEndException();
			}
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		[NullableContext(1)]
		internal JsonReaderException CreateUnexpectedEndException()
		{
			return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0004FC94 File Offset: 0x0004DE94
		internal void ReadIntoWrappedTypeObject()
		{
			this.ReaderReadAndAssert();
			if (this.Value != null && this.Value.ToString() == "$type")
			{
				this.ReaderReadAndAssert();
				if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
				{
					this.ReaderReadAndAssert();
					if (this.Value.ToString() == "$value")
					{
						return;
					}
				}
			}
			throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0004FD28 File Offset: 0x0004DF28
		public void Skip()
		{
			if (this.TokenType == JsonToken.PropertyName)
			{
				this.Read();
			}
			if (JsonTokenUtils.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0000F0CD File Offset: 0x0000D2CD
		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null, true);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0000F0D8 File Offset: 0x0000D2D8
		protected void SetToken(JsonToken newToken, object value)
		{
			this.SetToken(newToken, value, true);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0004FD6C File Offset: 0x0004DF6C
		protected void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			this._tokenType = newToken;
			this._value = value;
			switch (newToken)
			{
			case JsonToken.StartObject:
				this._currentState = JsonReader.State.ObjectStart;
				this.Push(JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this._currentState = JsonReader.State.ArrayStart;
				this.Push(JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this._currentState = JsonReader.State.ConstructorStart;
				this.Push(JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
				this._currentState = JsonReader.State.Property;
				this._currentPosition.PropertyName = (string)value;
				return;
			case JsonToken.Comment:
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.SetPostValueState(updateIndex);
				break;
			case JsonToken.EndObject:
				this.ValidateEnd(JsonToken.EndObject);
				return;
			case JsonToken.EndArray:
				this.ValidateEnd(JsonToken.EndArray);
				return;
			case JsonToken.EndConstructor:
				this.ValidateEnd(JsonToken.EndConstructor);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0000F0E3 File Offset: 0x0000D2E3
		internal void SetPostValueState(bool updateIndex)
		{
			if (this.Peek() == JsonContainerType.None && !this.SupportMultipleContent)
			{
				this.SetFinished();
			}
			else
			{
				this._currentState = JsonReader.State.PostValue;
			}
			if (updateIndex)
			{
				this.UpdateScopeWithFinishedValue();
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0000F10D File Offset: 0x0000D30D
		private void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0004FE40 File Offset: 0x0004E040
		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (this.Peek() == JsonContainerType.None && !this.SupportMultipleContent)
			{
				this.SetFinished();
				return;
			}
			this._currentState = JsonReader.State.PostValue;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0004FEA0 File Offset: 0x0004E0A0
		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this.SetFinished();
				return;
			case JsonContainerType.Object:
				this._currentState = JsonReader.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonReader.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonReader.State.Constructor;
				return;
			default:
				throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0000F12C File Offset: 0x0000D32C
		private void SetFinished()
		{
			this._currentState = (this.SupportMultipleContent ? JsonReader.State.Start : JsonReader.State.Finished);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0000F141 File Offset: 0x0000D341
		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JsonContainerType.Object;
			case JsonToken.EndArray:
				return JsonContainerType.Array;
			case JsonToken.EndConstructor:
				return JsonContainerType.Constructor;
			default:
				throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
			}
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0000F179 File Offset: 0x0000D379
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0000F188 File Offset: 0x0000D388
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._tokenType = JsonToken.None;
			this._value = null;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0000F1B7 File Offset: 0x0000D3B7
		internal void ReadAndAssert()
		{
			if (!this.Read())
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0000F1CD File Offset: 0x0000D3CD
		internal void ReadForTypeAndAssert(JsonContract contract, bool hasConverter)
		{
			if (!this.ReadForType(contract, hasConverter))
			{
				throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0004FF08 File Offset: 0x0004E108
		internal bool ReadForType(JsonContract contract, bool hasConverter)
		{
			if (hasConverter)
			{
				return this.Read();
			}
			if (contract != null)
			{
				switch (contract.InternalReadType)
				{
				case ReadType.Read:
					goto IL_48;
				case ReadType.ReadAsInt32:
					this.ReadAsInt32();
					break;
				case ReadType.ReadAsInt64:
				{
					bool result = this.ReadAndMoveToContent();
					if (this.TokenType == JsonToken.Undefined)
					{
						string format = "An undefined token is not a valid {0}.";
						IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
						Type arg;
						if (contract != null)
						{
							if ((arg = contract.UnderlyingType) != null)
							{
								goto IL_8D;
							}
						}
						arg = typeof(long);
						IL_8D:
						throw JsonReaderException.Create(this, format.FormatWith(invariantCulture, arg));
					}
					return result;
				}
				case ReadType.ReadAsBytes:
					this.ReadAsBytes();
					break;
				case ReadType.ReadAsString:
					this.ReadAsString();
					break;
				case ReadType.ReadAsDecimal:
					this.ReadAsDecimal();
					break;
				case ReadType.ReadAsDateTime:
					this.ReadAsDateTime();
					break;
				case ReadType.ReadAsDateTimeOffset:
					this.ReadAsDateTimeOffset();
					break;
				case ReadType.ReadAsDouble:
					this.ReadAsDouble();
					break;
				case ReadType.ReadAsBoolean:
					this.ReadAsBoolean();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				return this.TokenType > JsonToken.None;
			}
			IL_48:
			return this.ReadAndMoveToContent();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0000F1E5 File Offset: 0x0000D3E5
		internal bool ReadAndMoveToContent()
		{
			return this.Read() && this.MoveToContent();
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		internal bool MoveToContent()
		{
			JsonToken tokenType = this.TokenType;
			while (tokenType == JsonToken.None || tokenType == JsonToken.Comment)
			{
				if (!this.Read())
				{
					return false;
				}
				tokenType = this.TokenType;
			}
			return true;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0005002C File Offset: 0x0004E22C
		private JsonToken GetContentToken()
		{
			while (this.Read())
			{
				JsonToken tokenType = this.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					return tokenType;
				}
			}
			this.SetToken(JsonToken.None);
			return JsonToken.None;
		}

		// Token: 0x040007DC RID: 2012
		private JsonToken _tokenType;

		// Token: 0x040007DD RID: 2013
		private object _value;

		// Token: 0x040007DE RID: 2014
		internal char _quoteChar;

		// Token: 0x040007DF RID: 2015
		internal JsonReader.State _currentState;

		// Token: 0x040007E0 RID: 2016
		private JsonPosition _currentPosition;

		// Token: 0x040007E1 RID: 2017
		private CultureInfo _culture;

		// Token: 0x040007E2 RID: 2018
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x040007E3 RID: 2019
		private int? _maxDepth;

		// Token: 0x040007E4 RID: 2020
		private bool _hasExceededMaxDepth;

		// Token: 0x040007E5 RID: 2021
		internal DateParseHandling _dateParseHandling;

		// Token: 0x040007E6 RID: 2022
		internal FloatParseHandling _floatParseHandling;

		// Token: 0x040007E7 RID: 2023
		private string _dateFormatString;

		// Token: 0x040007E8 RID: 2024
		private List<JsonPosition> _stack;

		// Token: 0x020001C7 RID: 455
		[NullableContext(0)]
		protected internal enum State
		{
			// Token: 0x040007EC RID: 2028
			Start,
			// Token: 0x040007ED RID: 2029
			Complete,
			// Token: 0x040007EE RID: 2030
			Property,
			// Token: 0x040007EF RID: 2031
			ObjectStart,
			// Token: 0x040007F0 RID: 2032
			Object,
			// Token: 0x040007F1 RID: 2033
			ArrayStart,
			// Token: 0x040007F2 RID: 2034
			Array,
			// Token: 0x040007F3 RID: 2035
			Closed,
			// Token: 0x040007F4 RID: 2036
			PostValue,
			// Token: 0x040007F5 RID: 2037
			ConstructorStart,
			// Token: 0x040007F6 RID: 2038
			Constructor,
			// Token: 0x040007F7 RID: 2039
			Error,
			// Token: 0x040007F8 RID: 2040
			Finished
		}
	}
}
