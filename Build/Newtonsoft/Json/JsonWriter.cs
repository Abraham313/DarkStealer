using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001D6 RID: 470
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x06000DB9 RID: 3513 RVA: 0x000556D8 File Offset: 0x000538D8
		internal static JsonWriter.State[][] BuildStateArray()
		{
			List<JsonWriter.State[]> list = JsonWriter.StateArrayTempate.ToList<JsonWriter.State[]>();
			JsonWriter.State[] item = JsonWriter.StateArrayTempate[0];
			JsonWriter.State[] item2 = JsonWriter.StateArrayTempate[7];
			foreach (ulong num in EnumUtils.GetEnumValuesAndNames(typeof(JsonToken)).Values)
			{
				if (list.Count <= (int)num)
				{
					JsonToken jsonToken = (JsonToken)num;
					if (jsonToken - JsonToken.Integer > 5 && jsonToken - JsonToken.Date > 1)
					{
						list.Add(item);
					}
					else
					{
						list.Add(item2);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0005576C File Offset: 0x0005396C
		static JsonWriter()
		{
			JsonWriter.StateArray = JsonWriter.BuildStateArray();
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00010606 File Offset: 0x0000E806
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0001060E File Offset: 0x0000E80E
		public bool CloseOutput { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00010617 File Offset: 0x0000E817
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x0001061F File Offset: 0x0000E81F
		public bool AutoCompleteOnClose { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00055940 File Offset: 0x00053B40
		protected internal int Top
		{
			get
			{
				List<JsonPosition> stack = this._stack;
				int num = (stack != null) ? stack.Count : 0;
				if (this.Peek() != JsonContainerType.None)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00055970 File Offset: 0x00053B70
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + this._currentState.ToString(), null);
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000559E4 File Offset: 0x00053BE4
		internal string ContainerPath
		{
			get
			{
				if (this._currentPosition.Type != JsonContainerType.None && this._stack != null)
				{
					return JsonPosition.BuildPath(this._stack, null);
				}
				return string.Empty;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00055A20 File Offset: 0x00053C20
		public string Path
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? currentPosition = (this._currentState != JsonWriter.State.ArrayStart && this._currentState != JsonWriter.State.ConstructorStart && this._currentState != JsonWriter.State.ObjectStart) ? new JsonPosition?(this._currentPosition) : null;
				return JsonPosition.BuildPath(this._stack, currentPosition);
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00010628 File Offset: 0x0000E828
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x00010630 File Offset: 0x0000E830
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				if (value < Formatting.None || value > Formatting.Indented)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._formatting = value;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0001064C File Offset: 0x0000E84C
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00010654 File Offset: 0x0000E854
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling;
			}
			set
			{
				if (value < DateFormatHandling.IsoDateFormat || value > DateFormatHandling.MicrosoftDateFormat)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateFormatHandling = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00010670 File Offset: 0x0000E870
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00010678 File Offset: 0x0000E878
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

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00010694 File Offset: 0x0000E894
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0001069C File Offset: 0x0000E89C
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling;
			}
			set
			{
				if (value < StringEscapeHandling.Default || value > StringEscapeHandling.EscapeHtml)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._stringEscapeHandling = value;
				this.OnStringEscapeHandlingChanged();
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00009B58 File Offset: 0x00007D58
		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x000106BE File Offset: 0x0000E8BE
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x000106C6 File Offset: 0x0000E8C6
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling;
			}
			set
			{
				if (value < FloatFormatHandling.String || value > FloatFormatHandling.DefaultValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatFormatHandling = value;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000106E2 File Offset: 0x0000E8E2
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x000106EA File Offset: 0x0000E8EA
		[Nullable(2)]
		public string DateFormatString
		{
			[NullableContext(2)]
			get
			{
				return this._dateFormatString;
			}
			[NullableContext(2)]
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000106F3 File Offset: 0x0000E8F3
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00010704 File Offset: 0x0000E904
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0001070D File Offset: 0x0000E90D
		protected JsonWriter()
		{
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this.CloseOutput = true;
			this.AutoCompleteOnClose = true;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00010738 File Offset: 0x0000E938
		internal void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00010757 File Offset: 0x0000E957
		private void Push(JsonContainerType value)
		{
			if (this._currentPosition.Type != JsonContainerType.None)
			{
				if (this._stack == null)
				{
					this._stack = new List<JsonPosition>();
				}
				this._stack.Add(this._currentPosition);
			}
			this._currentPosition = new JsonPosition(value);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00055A88 File Offset: 0x00053C88
		private JsonContainerType Pop()
		{
			ref JsonPosition currentPosition = this._currentPosition;
			if (this._stack != null && this._stack.Count > 0)
			{
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				this._currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00010796 File Offset: 0x0000E996
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x06000DD7 RID: 3543
		public abstract void Flush();

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000107A3 File Offset: 0x0000E9A3
		public virtual void Close()
		{
			if (this.AutoCompleteOnClose)
			{
				this.AutoCompleteAll();
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x000107B3 File Offset: 0x0000E9B3
		public virtual void WriteStartObject()
		{
			this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x000107BD File Offset: 0x0000E9BD
		public virtual void WriteEndObject()
		{
			this.InternalWriteEnd(JsonContainerType.Object);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x000107C6 File Offset: 0x0000E9C6
		public virtual void WriteStartArray()
		{
			this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x000107D0 File Offset: 0x0000E9D0
		public virtual void WriteEndArray()
		{
			this.InternalWriteEnd(JsonContainerType.Array);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x000107D9 File Offset: 0x0000E9D9
		public virtual void WriteStartConstructor(string name)
		{
			this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x000107E3 File Offset: 0x0000E9E3
		public virtual void WriteEndConstructor()
		{
			this.InternalWriteEnd(JsonContainerType.Constructor);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000107EC File Offset: 0x0000E9EC
		public virtual void WritePropertyName(string name)
		{
			this.InternalWritePropertyName(name);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000107F5 File Offset: 0x0000E9F5
		public virtual void WritePropertyName(string name, bool escape)
		{
			this.WritePropertyName(name);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x000107FE File Offset: 0x0000E9FE
		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0001080C File Offset: 0x0000EA0C
		public void WriteToken(JsonReader reader)
		{
			this.WriteToken(reader, true);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00010816 File Offset: 0x0000EA16
		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this.WriteToken(reader, writeChildren, true, true);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00055AFC File Offset: 0x00053CFC
		[NullableContext(2)]
		public void WriteToken(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
				this.WriteStartObject();
				return;
			case JsonToken.StartArray:
				this.WriteStartArray();
				return;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteStartConstructor(value.ToString());
				return;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WritePropertyName(value.ToString());
				return;
			case JsonToken.Comment:
				this.WriteComment((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Raw:
				this.WriteRawValue((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is System.Numerics.BigInteger)
				{
					System.Numerics.BigInteger bigInteger = (System.Numerics.BigInteger)value;
					this.WriteValue(bigInteger);
					return;
				}
				this.WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					decimal value2 = (decimal)value;
					this.WriteValue(value2);
					return;
				}
				if (value is double)
				{
					double value3 = (double)value;
					this.WriteValue(value3);
					return;
				}
				if (value is float)
				{
					float value4 = (float)value;
					this.WriteValue(value4);
					return;
				}
				this.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(value.ToString());
				return;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Null:
				this.WriteNull();
				return;
			case JsonToken.Undefined:
				this.WriteUndefined();
				return;
			case JsonToken.EndObject:
				this.WriteEndObject();
				return;
			case JsonToken.EndArray:
				this.WriteEndArray();
				return;
			case JsonToken.EndConstructor:
				this.WriteEndConstructor();
				return;
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					DateTimeOffset value5 = (DateTimeOffset)value;
					this.WriteValue(value5);
					return;
				}
				this.WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					Guid value6 = (Guid)value;
					this.WriteValue(value6);
					return;
				}
				this.WriteValue((byte[])value);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0001082D File Offset: 0x0000EA2D
		public void WriteToken(JsonToken token)
		{
			this.WriteToken(token, null);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00055D2C File Offset: 0x00053F2C
		internal virtual void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			int num = this.CalculateWriteTokenInitialDepth(reader);
			for (;;)
			{
				if (!writeDateConstructorAsDate || reader.TokenType != JsonToken.StartConstructor)
				{
					goto IL_0A;
				}
				object value = reader.Value;
				if (!string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
				{
					goto IL_0A;
				}
				this.WriteConstructorDate(reader);
				IL_29:
				if (num - 1 >= reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) || !writeChildren)
				{
					break;
				}
				if (!reader.Read())
				{
					break;
				}
				continue;
				IL_0A:
				if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					this.WriteToken(reader.TokenType, reader.Value);
					goto IL_29;
				}
				goto IL_29;
			}
			if (this.IsWriteTokenIncomplete(reader, writeChildren, num))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00055DDC File Offset: 0x00053FDC
		private bool IsWriteTokenIncomplete(JsonReader reader, bool writeChildren, int initialDepth)
		{
			int num = this.CalculateWriteTokenFinalDepth(reader);
			return initialDepth < num || (writeChildren && initialDepth == num && JsonTokenUtils.IsStartToken(reader.TokenType));
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00055E0C File Offset: 0x0005400C
		private int CalculateWriteTokenInitialDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsStartToken(tokenType))
			{
				return reader.Depth + 1;
			}
			return reader.Depth;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00055E3C File Offset: 0x0005403C
		private int CalculateWriteTokenFinalDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsEndToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth - 1;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00055E6C File Offset: 0x0005406C
		private void WriteConstructorDate(JsonReader reader)
		{
			DateTime value;
			string message;
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out value, out message))
			{
				throw JsonWriterException.Create(this, message, null);
			}
			this.WriteValue(value);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00055E98 File Offset: 0x00054098
		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				this.WriteEndObject();
				return;
			case JsonContainerType.Array:
				this.WriteEndArray();
				return;
			case JsonContainerType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type.ToString(), null);
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00010837 File Offset: 0x0000EA37
		private void AutoCompleteAll()
		{
			while (this.Top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0001084B File Offset: 0x0000EA4B
		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				return JsonToken.EndObject;
			case JsonContainerType.Array:
				return JsonToken.EndArray;
			case JsonContainerType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw JsonWriterException.Create(this, "No close token for type: " + type.ToString(), null);
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00055EEC File Offset: 0x000540EC
		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = this.CalculateLevelsToComplete(type);
			for (int i = 0; i < num; i++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteNull();
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
				this.UpdateCurrentState();
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00055F58 File Offset: 0x00054158
		private int CalculateLevelsToComplete(JsonContainerType type)
		{
			int num = 0;
			if (this._currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = this.Top - 2;
				for (int i = num2; i >= 0; i--)
				{
					int index = num2 - i;
					if (this._stack[index].Type == type)
					{
						num = i + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			return num;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00055FC0 File Offset: 0x000541C0
		private void UpdateCurrentState()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this._currentState = JsonWriter.State.Start;
				return;
			case JsonContainerType.Object:
				this._currentState = JsonWriter.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonWriter.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonWriter.State.Array;
				return;
			default:
				throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType.ToString(), null);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00009B58 File Offset: 0x00007D58
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00009B58 File Offset: 0x00007D58
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00009B58 File Offset: 0x00007D58
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00009B58 File Offset: 0x00007D58
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00056028 File Offset: 0x00054228
		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			JsonWriter.State state = JsonWriter.StateArray[(int)tokenBeingWritten][(int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()), null);
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			if (this._formatting == Formatting.Indented)
			{
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteIndentSpace();
				}
				if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start))
				{
					this.WriteIndent();
				}
			}
			this._currentState = state;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00010888 File Offset: 0x0000EA88
		public virtual void WriteNull()
		{
			this.InternalWriteValue(JsonToken.Null);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00010892 File Offset: 0x0000EA92
		public virtual void WriteUndefined()
		{
			this.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0001089C File Offset: 0x0000EA9C
		[NullableContext(2)]
		public virtual void WriteRaw(string json)
		{
			this.InternalWriteRaw();
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x000108A4 File Offset: 0x0000EAA4
		[NullableContext(2)]
		public virtual void WriteRawValue(string json)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x000108BB File Offset: 0x0000EABB
		[NullableContext(2)]
		public virtual void WriteValue(string value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public virtual void WriteValue(int value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000108C5 File Offset: 0x0000EAC5
		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public virtual void WriteValue(long value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000108C5 File Offset: 0x0000EAC5
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000108CE File Offset: 0x0000EACE
		public virtual void WriteValue(float value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000108CE File Offset: 0x0000EACE
		public virtual void WriteValue(double value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000108D7 File Offset: 0x0000EAD7
		public virtual void WriteValue(bool value)
		{
			this.InternalWriteValue(JsonToken.Boolean);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public virtual void WriteValue(short value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000108C5 File Offset: 0x0000EAC5
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000108BB File Offset: 0x0000EABB
		public virtual void WriteValue(char value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public virtual void WriteValue(byte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000108C5 File Offset: 0x0000EAC5
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000108CE File Offset: 0x0000EACE
		public virtual void WriteValue(decimal value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000108E1 File Offset: 0x0000EAE1
		public virtual void WriteValue(DateTime value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000108E1 File Offset: 0x0000EAE1
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000108BB File Offset: 0x0000EABB
		public virtual void WriteValue(Guid value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x000108BB File Offset: 0x0000EABB
		public virtual void WriteValue(TimeSpan value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000108EB File Offset: 0x0000EAEB
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0001090A File Offset: 0x0000EB0A
		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00010929 File Offset: 0x0000EB29
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00010948 File Offset: 0x0000EB48
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00010967 File Offset: 0x0000EB67
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00010986 File Offset: 0x0000EB86
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000109A5 File Offset: 0x0000EBA5
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000109C4 File Offset: 0x0000EBC4
		public virtual void WriteValue(short? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000109E3 File Offset: 0x0000EBE3
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00010A02 File Offset: 0x0000EC02
		public virtual void WriteValue(char? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00010A21 File Offset: 0x0000EC21
		public virtual void WriteValue(byte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00010A40 File Offset: 0x0000EC40
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00010A5F File Offset: 0x0000EC5F
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00010A7E File Offset: 0x0000EC7E
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00010A9D File Offset: 0x0000EC9D
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00010ABC File Offset: 0x0000ECBC
		public virtual void WriteValue(Guid? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00010ADB File Offset: 0x0000ECDB
		public virtual void WriteValue(TimeSpan? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00010AFA File Offset: 0x0000ECFA
		[NullableContext(2)]
		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.Bytes);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00010B0E File Offset: 0x0000ED0E
		[NullableContext(2)]
		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00010B28 File Offset: 0x0000ED28
		[NullableContext(2)]
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is System.Numerics.BigInteger)
			{
				throw JsonWriter.CreateUnsupportedTypeException(this, value);
			}
			JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00010B56 File Offset: 0x0000ED56
		[NullableContext(2)]
		public virtual void WriteComment(string text)
		{
			this.InternalWriteComment();
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00010B5E File Offset: 0x0000ED5E
		public virtual void WriteWhitespace(string ws)
		{
			this.InternalWriteWhitespace(ws);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00010B67 File Offset: 0x0000ED67
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00010B76 File Offset: 0x0000ED76
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonWriter.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000560FC File Offset: 0x000542FC
		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			for (;;)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					goto IL_D6;
				case PrimitiveTypeCode.CharNullable:
					goto IL_E3;
				case PrimitiveTypeCode.Boolean:
					goto IL_103;
				case PrimitiveTypeCode.BooleanNullable:
					goto IL_110;
				case PrimitiveTypeCode.SByte:
					goto IL_130;
				case PrimitiveTypeCode.SByteNullable:
					goto IL_13D;
				case PrimitiveTypeCode.Int16:
					goto IL_15D;
				case PrimitiveTypeCode.Int16Nullable:
					goto IL_16A;
				case PrimitiveTypeCode.UInt16:
					goto IL_18A;
				case PrimitiveTypeCode.UInt16Nullable:
					goto IL_197;
				case PrimitiveTypeCode.Int32:
					goto IL_1B8;
				case PrimitiveTypeCode.Int32Nullable:
					goto IL_1C5;
				case PrimitiveTypeCode.Byte:
					goto IL_1E6;
				case PrimitiveTypeCode.ByteNullable:
					goto IL_1F3;
				case PrimitiveTypeCode.UInt32:
					goto IL_214;
				case PrimitiveTypeCode.UInt32Nullable:
					goto IL_221;
				case PrimitiveTypeCode.Int64:
					goto IL_242;
				case PrimitiveTypeCode.Int64Nullable:
					goto IL_24F;
				case PrimitiveTypeCode.UInt64:
					goto IL_270;
				case PrimitiveTypeCode.UInt64Nullable:
					goto IL_27D;
				case PrimitiveTypeCode.Single:
					goto IL_29E;
				case PrimitiveTypeCode.SingleNullable:
					goto IL_2AB;
				case PrimitiveTypeCode.Double:
					goto IL_2CC;
				case PrimitiveTypeCode.DoubleNullable:
					goto IL_2D9;
				case PrimitiveTypeCode.DateTime:
					goto IL_2FA;
				case PrimitiveTypeCode.DateTimeNullable:
					goto IL_307;
				case PrimitiveTypeCode.DateTimeOffset:
					goto IL_328;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					goto IL_335;
				case PrimitiveTypeCode.Decimal:
					goto IL_356;
				case PrimitiveTypeCode.DecimalNullable:
					goto IL_363;
				case PrimitiveTypeCode.Guid:
					goto IL_384;
				case PrimitiveTypeCode.GuidNullable:
					goto IL_391;
				case PrimitiveTypeCode.TimeSpan:
					goto IL_3B2;
				case PrimitiveTypeCode.TimeSpanNullable:
					goto IL_3BF;
				case PrimitiveTypeCode.BigInteger:
					goto IL_3E0;
				case PrimitiveTypeCode.BigIntegerNullable:
					goto IL_3F2;
				case PrimitiveTypeCode.Uri:
					goto IL_418;
				case PrimitiveTypeCode.String:
					goto IL_425;
				case PrimitiveTypeCode.Bytes:
					goto IL_432;
				case PrimitiveTypeCode.DBNull:
					goto IL_43F;
				default:
				{
					IConvertible convertible = value as IConvertible;
					if (convertible == null)
					{
						goto IL_C4;
					}
					JsonWriter.ResolveConvertibleValue(convertible, out typeCode, out value);
					break;
				}
				}
			}
			IL_C4:
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			throw JsonWriter.CreateUnsupportedTypeException(writer, value);
			IL_D6:
			writer.WriteValue((char)value);
			return;
			IL_E3:
			writer.WriteValue((value == null) ? null : new char?((char)value));
			return;
			IL_103:
			writer.WriteValue((bool)value);
			return;
			IL_110:
			writer.WriteValue((value == null) ? null : new bool?((bool)value));
			return;
			IL_130:
			writer.WriteValue((sbyte)value);
			return;
			IL_13D:
			writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
			return;
			IL_15D:
			writer.WriteValue((short)value);
			return;
			IL_16A:
			writer.WriteValue((value == null) ? null : new short?((short)value));
			return;
			IL_18A:
			writer.WriteValue((ushort)value);
			return;
			IL_197:
			writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
			return;
			IL_1B8:
			writer.WriteValue((int)value);
			return;
			IL_1C5:
			writer.WriteValue((value == null) ? null : new int?((int)value));
			return;
			IL_1E6:
			writer.WriteValue((byte)value);
			return;
			IL_1F3:
			writer.WriteValue((value == null) ? null : new byte?((byte)value));
			return;
			IL_214:
			writer.WriteValue((uint)value);
			return;
			IL_221:
			writer.WriteValue((value == null) ? null : new uint?((uint)value));
			return;
			IL_242:
			writer.WriteValue((long)value);
			return;
			IL_24F:
			writer.WriteValue((value == null) ? null : new long?((long)value));
			return;
			IL_270:
			writer.WriteValue((ulong)value);
			return;
			IL_27D:
			writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
			return;
			IL_29E:
			writer.WriteValue((float)value);
			return;
			IL_2AB:
			writer.WriteValue((value == null) ? null : new float?((float)value));
			return;
			IL_2CC:
			writer.WriteValue((double)value);
			return;
			IL_2D9:
			writer.WriteValue((value == null) ? null : new double?((double)value));
			return;
			IL_2FA:
			writer.WriteValue((DateTime)value);
			return;
			IL_307:
			writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
			return;
			IL_328:
			writer.WriteValue((DateTimeOffset)value);
			return;
			IL_335:
			writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
			return;
			IL_356:
			writer.WriteValue((decimal)value);
			return;
			IL_363:
			writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
			return;
			IL_384:
			writer.WriteValue((Guid)value);
			return;
			IL_391:
			writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
			return;
			IL_3B2:
			writer.WriteValue((TimeSpan)value);
			return;
			IL_3BF:
			writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
			return;
			IL_3E0:
			writer.WriteValue((System.Numerics.BigInteger)value);
			return;
			IL_3F2:
			writer.WriteValue((value == null) ? null : new System.Numerics.BigInteger?((System.Numerics.BigInteger)value));
			return;
			IL_418:
			writer.WriteValue((Uri)value);
			return;
			IL_425:
			writer.WriteValue((string)value);
			return;
			IL_432:
			writer.WriteValue((byte[])value);
			return;
			IL_43F:
			writer.WriteNull();
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00056550 File Offset: 0x00054750
		private static void ResolveConvertibleValue(IConvertible convertible, out PrimitiveTypeCode typeCode, out object value)
		{
			TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
			typeCode = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode);
			Type conversionType = (typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type;
			value = convertible.ToType(conversionType, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00010B8E File Offset: 0x0000ED8E
		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000565A4 File Offset: 0x000547A4
		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				this.InternalWriteStart(token, JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this.InternalWriteStart(token, JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this.InternalWriteStart(token, JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
			{
				string text = value as string;
				if (text == null)
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				this.InternalWritePropertyName(text);
				return;
			}
			case JsonToken.Comment:
				this.InternalWriteComment();
				return;
			case JsonToken.Raw:
				this.InternalWriteRaw();
				return;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.InternalWriteValue(token);
				return;
			case JsonToken.EndObject:
				this.InternalWriteEnd(JsonContainerType.Object);
				return;
			case JsonToken.EndArray:
				this.InternalWriteEnd(JsonContainerType.Array);
				return;
			case JsonToken.EndConstructor:
				this.InternalWriteEnd(JsonContainerType.Constructor);
				return;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00010BAC File Offset: 0x0000EDAC
		internal void InternalWriteEnd(JsonContainerType container)
		{
			this.AutoCompleteClose(container);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00010BB5 File Offset: 0x0000EDB5
		internal void InternalWritePropertyName(string name)
		{
			this._currentPosition.PropertyName = name;
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00009B58 File Offset: 0x00007D58
		internal void InternalWriteRaw()
		{
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00010BCA File Offset: 0x0000EDCA
		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
			this.Push(container);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		internal void InternalWriteValue(JsonToken token)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00010BEF File Offset: 0x0000EDEF
		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00010C09 File Offset: 0x0000EE09
		internal void InternalWriteComment()
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x040008A6 RID: 2214
		private static readonly JsonWriter.State[][] StateArray;

		// Token: 0x040008A7 RID: 2215
		internal static readonly JsonWriter.State[][] StateArrayTempate = new JsonWriter.State[][]
		{
			new JsonWriter.State[]
			{
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Property,
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Object,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Array,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			}
		};

		// Token: 0x040008A8 RID: 2216
		[Nullable(2)]
		private List<JsonPosition> _stack;

		// Token: 0x040008A9 RID: 2217
		private JsonPosition _currentPosition;

		// Token: 0x040008AA RID: 2218
		private JsonWriter.State _currentState;

		// Token: 0x040008AB RID: 2219
		private Formatting _formatting;

		// Token: 0x040008AE RID: 2222
		private DateFormatHandling _dateFormatHandling;

		// Token: 0x040008AF RID: 2223
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x040008B0 RID: 2224
		private StringEscapeHandling _stringEscapeHandling;

		// Token: 0x040008B1 RID: 2225
		private FloatFormatHandling _floatFormatHandling;

		// Token: 0x040008B2 RID: 2226
		[Nullable(2)]
		private string _dateFormatString;

		// Token: 0x040008B3 RID: 2227
		[Nullable(2)]
		private CultureInfo _culture;

		// Token: 0x020001D7 RID: 471
		[NullableContext(0)]
		internal enum State
		{
			// Token: 0x040008B5 RID: 2229
			Start,
			// Token: 0x040008B6 RID: 2230
			Property,
			// Token: 0x040008B7 RID: 2231
			ObjectStart,
			// Token: 0x040008B8 RID: 2232
			Object,
			// Token: 0x040008B9 RID: 2233
			ArrayStart,
			// Token: 0x040008BA RID: 2234
			Array,
			// Token: 0x040008BB RID: 2235
			ConstructorStart,
			// Token: 0x040008BC RID: 2236
			Constructor,
			// Token: 0x040008BD RID: 2237
			Closed,
			// Token: 0x040008BE RID: 2238
			Error
		}
	}
}
