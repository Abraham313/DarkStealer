using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000300 RID: 768
	[NullableContext(1)]
	[Nullable(0)]
	public class StringEnumConverter : JsonConverter
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x000175AB File Offset: 0x000157AB
		// (set) Token: 0x06001801 RID: 6145 RVA: 0x000175BD File Offset: 0x000157BD
		[Obsolete("StringEnumConverter.CamelCaseText is obsolete. Set StringEnumConverter.NamingStrategy with CamelCaseNamingStrategy instead.")]
		public bool CamelCaseText
		{
			get
			{
				return this.NamingStrategy is CamelCaseNamingStrategy;
			}
			set
			{
				if (value)
				{
					if (this.NamingStrategy is CamelCaseNamingStrategy)
					{
						return;
					}
					this.NamingStrategy = new CamelCaseNamingStrategy();
					return;
				}
				else
				{
					if (!(this.NamingStrategy is CamelCaseNamingStrategy))
					{
						return;
					}
					this.NamingStrategy = null;
					return;
				}
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x000175F1 File Offset: 0x000157F1
		// (set) Token: 0x06001803 RID: 6147 RVA: 0x000175F9 File Offset: 0x000157F9
		[Nullable(2)]
		public NamingStrategy NamingStrategy { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x00017602 File Offset: 0x00015802
		// (set) Token: 0x06001805 RID: 6149 RVA: 0x0001760A File Offset: 0x0001580A
		public bool AllowIntegerValues { get; set; } = true;

		// Token: 0x06001806 RID: 6150 RVA: 0x00017613 File Offset: 0x00015813
		public StringEnumConverter()
		{
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00017622 File Offset: 0x00015822
		[Obsolete("StringEnumConverter(bool) is obsolete. Create a converter with StringEnumConverter(NamingStrategy, bool) instead.")]
		public StringEnumConverter(bool camelCaseText)
		{
			if (camelCaseText)
			{
				this.NamingStrategy = new CamelCaseNamingStrategy();
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0001763F File Offset: 0x0001583F
		public StringEnumConverter(NamingStrategy namingStrategy, bool allowIntegerValues = true)
		{
			this.NamingStrategy = namingStrategy;
			this.AllowIntegerValues = allowIntegerValues;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0001765C File Offset: 0x0001585C
		public StringEnumConverter(Type namingStrategyType)
		{
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, null);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00017683 File Offset: 0x00015883
		public StringEnumConverter(Type namingStrategyType, object[] namingStrategyParameters)
		{
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000176AA File Offset: 0x000158AA
		public StringEnumConverter(Type namingStrategyType, object[] namingStrategyParameters, bool allowIntegerValues)
		{
			ValidationUtils.ArgumentNotNull(namingStrategyType, "namingStrategyType");
			this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
			this.AllowIntegerValues = allowIntegerValues;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00074998 File Offset: 0x00072B98
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string value2;
			if (EnumUtils.TryToString(@enum.GetType(), value, this.NamingStrategy, out value2))
			{
				writer.WriteValue(value2);
				return;
			}
			if (!this.AllowIntegerValues)
			{
				throw JsonSerializationException.Create(null, writer.ContainerPath, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, @enum.ToString("D")), null);
			}
			writer.WriteValue(value);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00074A0C File Offset: 0x00072C0C
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Null)
			{
				bool flag;
				Type type = (flag = ReflectionUtils.IsNullableType(objectType)) ? Nullable.GetUnderlyingType(objectType) : objectType;
				try
				{
					if (reader.TokenType == JsonToken.String)
					{
						object value = reader.Value;
						string value2 = (value != null) ? value.ToString() : null;
						if (StringUtils.IsNullOrEmpty(value2) && flag)
						{
							return null;
						}
						return EnumUtils.ParseEnum(type, this.NamingStrategy, value2, !this.AllowIntegerValues);
					}
					else if (reader.TokenType == JsonToken.Integer)
					{
						if (!this.AllowIntegerValues)
						{
							throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, reader.Value));
						}
						return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
					}
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(reader.Value), objectType), ex);
				}
				throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			if (!ReflectionUtils.IsNullableType(objectType))
			{
				throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			return null;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000176D8 File Offset: 0x000158D8
		public override bool CanConvert(Type objectType)
		{
			return (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum();
		}
	}
}
