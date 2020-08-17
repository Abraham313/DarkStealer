using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002FC RID: 764
	[NullableContext(1)]
	[Nullable(0)]
	public class IsoDateTimeConverter : DateTimeConverterBase
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x000174F9 File Offset: 0x000156F9
		// (set) Token: 0x060017E5 RID: 6117 RVA: 0x00017501 File Offset: 0x00015701
		public DateTimeStyles DateTimeStyles
		{
			get
			{
				return this._dateTimeStyles;
			}
			set
			{
				this._dateTimeStyles = value;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0001750A File Offset: 0x0001570A
		// (set) Token: 0x060017E7 RID: 6119 RVA: 0x0001751B File Offset: 0x0001571B
		[Nullable(2)]
		public string DateTimeFormat
		{
			[NullableContext(2)]
			get
			{
				return this._dateTimeFormat ?? string.Empty;
			}
			[NullableContext(2)]
			set
			{
				this._dateTimeFormat = (StringUtils.IsNullOrEmpty(value) ? null : value);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x0001752F File Offset: 0x0001572F
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00017540 File Offset: 0x00015740
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.CurrentCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000740C0 File Offset: 0x000722C0
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			string value2;
			if (value is DateTime)
			{
				DateTime dateTime = (DateTime)value;
				if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				value2 = dateTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			else
			{
				if (!(value is DateTimeOffset))
				{
					throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)));
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
				if ((this._dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (this._dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
				{
					dateTimeOffset = dateTimeOffset.ToUniversalTime();
				}
				value2 = dateTimeOffset.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			writer.WriteValue(value2);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00074190 File Offset: 0x00072390
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			else
			{
				Type left = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
				if (reader.TokenType == JsonToken.Date)
				{
					if (left == typeof(DateTimeOffset))
					{
						if (!(reader.Value is DateTimeOffset))
						{
							return new DateTimeOffset((DateTime)reader.Value);
						}
						return reader.Value;
					}
					else
					{
						object value = reader.Value;
						if (value is DateTimeOffset)
						{
							return ((DateTimeOffset)value).DateTime;
						}
						return reader.Value;
					}
				}
				else
				{
					if (reader.TokenType != JsonToken.String)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
					}
					object value2 = reader.Value;
					string text = (value2 != null) ? value2.ToString() : null;
					if (StringUtils.IsNullOrEmpty(text) && flag)
					{
						return null;
					}
					if (left == typeof(DateTimeOffset))
					{
						if (!StringUtils.IsNullOrEmpty(this._dateTimeFormat))
						{
							return DateTimeOffset.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
						}
						return DateTimeOffset.Parse(text, this.Culture, this._dateTimeStyles);
					}
					else
					{
						if (!StringUtils.IsNullOrEmpty(this._dateTimeFormat))
						{
							return DateTime.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
						}
						return DateTime.Parse(text, this.Culture, this._dateTimeStyles);
					}
				}
			}
		}

		// Token: 0x04000CE5 RID: 3301
		private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x04000CE6 RID: 3302
		private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;

		// Token: 0x04000CE7 RID: 3303
		[Nullable(2)]
		private string _dateTimeFormat;

		// Token: 0x04000CE8 RID: 3304
		[Nullable(2)]
		private CultureInfo _culture;
	}
}
