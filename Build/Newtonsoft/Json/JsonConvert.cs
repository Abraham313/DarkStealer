using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001B8 RID: 440
	[NullableContext(1)]
	[Nullable(0)]
	public static class JsonConvert
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0000E92C File Offset: 0x0000CB2C
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0000E933 File Offset: 0x0000CB33
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static Func<JsonSerializerSettings> DefaultSettings { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x06000B47 RID: 2887 RVA: 0x0000E93B File Offset: 0x0000CB3B
		public static string ToString(DateTime value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0004E5C0 File Offset: 0x0004C7C0
		public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
		{
			DateTime value2 = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeString(stringWriter, value2, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0000E945 File Offset: 0x0000CB45
		public static string ToString(DateTimeOffset value)
		{
			return JsonConvert.ToString(value, DateFormatHandling.IsoDateFormat);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0004E620 File Offset: 0x0004C820
		public static string ToString(DateTimeOffset value, DateFormatHandling format)
		{
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0000E94E File Offset: 0x0000CB4E
		public static string ToString(bool value)
		{
			if (!value)
			{
				return JsonConvert.False;
			}
			return JsonConvert.True;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0000E95E File Offset: 0x0000CB5E
		public static string ToString(char value)
		{
			return JsonConvert.ToString(char.ToString(value));
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0000E96B File Offset: 0x0000CB6B
		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0000E978 File Offset: 0x0000CB78
		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0000E987 File Offset: 0x0000CB87
		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0000E996 File Offset: 0x0000CB96
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0000E9A5 File Offset: 0x0000CBA5
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0000E9B4 File Offset: 0x0000CBB4
		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		private static string ToStringInternal(System.Numerics.BigInteger value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0000E9D2 File Offset: 0x0000CBD2
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0000E9E1 File Offset: 0x0000CBE1
		public static string ToString(float value)
		{
			return JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0000E9FB File Offset: 0x0000CBFB
		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat((double)value, JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0000EA1F File Offset: 0x0000CC1F
		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || (!double.IsInfinity(value) && !double.IsNaN(value)))
			{
				return text;
			}
			if (floatFormatHandling != FloatFormatHandling.DefaultValue)
			{
				return quoteChar.ToString() + text + quoteChar.ToString();
			}
			if (nullable)
			{
				return JsonConvert.Null;
			}
			return "0.0";
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0000EA5F File Offset: 0x0000CC5F
		public static string ToString(double value)
		{
			return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0000EA78 File Offset: 0x0000CC78
		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return JsonConvert.EnsureFloatFormat(value, JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0004E678 File Offset: 0x0004C878
		private static string EnsureDecimalPlace(double value, string text)
		{
			if (!double.IsNaN(value) && !double.IsInfinity(value) && text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				if (text.IndexOf('e') == -1)
				{
					return text + ".0";
				}
			}
			return text;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0000EA9A File Offset: 0x0000CC9A
		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0000EAC3 File Offset: 0x0000CCC3
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0000EAD2 File Offset: 0x0000CCD2
		public static string ToString(decimal value)
		{
			return JsonConvert.EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0000EAE6 File Offset: 0x0000CCE6
		public static string ToString(Guid value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004E6C8 File Offset: 0x0004C8C8
		internal static string ToString(Guid value, char quoteChar)
		{
			string str = value.ToString("D", CultureInfo.InvariantCulture);
			string text = quoteChar.ToString(CultureInfo.InvariantCulture);
			return text + str + text;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0000EAF0 File Offset: 0x0000CCF0
		public static string ToString(TimeSpan value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0000EAFA File Offset: 0x0000CCFA
		internal static string ToString(TimeSpan value, char quoteChar)
		{
			return JsonConvert.ToString(value.ToString(), quoteChar);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0000EB0F File Offset: 0x0000CD0F
		public static string ToString([Nullable(2)] Uri value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0000EB28 File Offset: 0x0000CD28
		internal static string ToString(Uri value, char quoteChar)
		{
			return JsonConvert.ToString(value.OriginalString, quoteChar);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0000EB36 File Offset: 0x0000CD36
		public static string ToString([Nullable(2)] string value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0000EB40 File Offset: 0x0000CD40
		public static string ToString([Nullable(2)] string value, char delimiter)
		{
			return JsonConvert.ToString(value, delimiter, StringEscapeHandling.Default);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0000EB4A File Offset: 0x0000CD4A
		public static string ToString([Nullable(2)] string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'')
			{
				throw new ArgumentException("Delimiter must be a single or double quote.", "delimiter");
			}
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0004E6FC File Offset: 0x0004C8FC
		public static string ToString([Nullable(2)] object value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.Char:
				return JsonConvert.ToString((char)value);
			case PrimitiveTypeCode.Boolean:
				return JsonConvert.ToString((bool)value);
			case PrimitiveTypeCode.SByte:
				return JsonConvert.ToString((sbyte)value);
			case PrimitiveTypeCode.Int16:
				return JsonConvert.ToString((short)value);
			case PrimitiveTypeCode.UInt16:
				return JsonConvert.ToString((ushort)value);
			case PrimitiveTypeCode.Int32:
				return JsonConvert.ToString((int)value);
			case PrimitiveTypeCode.Byte:
				return JsonConvert.ToString((byte)value);
			case PrimitiveTypeCode.UInt32:
				return JsonConvert.ToString((uint)value);
			case PrimitiveTypeCode.Int64:
				return JsonConvert.ToString((long)value);
			case PrimitiveTypeCode.UInt64:
				return JsonConvert.ToString((ulong)value);
			case PrimitiveTypeCode.Single:
				return JsonConvert.ToString((float)value);
			case PrimitiveTypeCode.Double:
				return JsonConvert.ToString((double)value);
			case PrimitiveTypeCode.DateTime:
				return JsonConvert.ToString((DateTime)value);
			case PrimitiveTypeCode.DateTimeOffset:
				return JsonConvert.ToString((DateTimeOffset)value);
			case PrimitiveTypeCode.Decimal:
				return JsonConvert.ToString((decimal)value);
			case PrimitiveTypeCode.Guid:
				return JsonConvert.ToString((Guid)value);
			case PrimitiveTypeCode.TimeSpan:
				return JsonConvert.ToString((TimeSpan)value);
			case PrimitiveTypeCode.BigInteger:
				return JsonConvert.ToStringInternal((System.Numerics.BigInteger)value);
			case PrimitiveTypeCode.Uri:
				return JsonConvert.ToString((Uri)value);
			case PrimitiveTypeCode.String:
				return JsonConvert.ToString((string)value);
			case PrimitiveTypeCode.DBNull:
				return JsonConvert.Null;
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0000EB6F File Offset: 0x0000CD6F
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value)
		{
			return JsonConvert.SerializeObject(value, null, null);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0000EB79 File Offset: 0x0000CD79
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, Formatting formatting)
		{
			return JsonConvert.SerializeObject(value, formatting, null);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004E8DC File Offset: 0x0004CADC
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, params JsonConverter[] converters)
		{
			JsonSerializerSettings jsonSerializerSettings;
			if (converters != null && converters.Length != 0)
			{
				(jsonSerializerSettings = new JsonSerializerSettings()).Converters = converters;
			}
			else
			{
				jsonSerializerSettings = null;
			}
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.SerializeObject(value, null, settings);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004E908 File Offset: 0x0004CB08
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, Formatting formatting, params JsonConverter[] converters)
		{
			JsonSerializerSettings jsonSerializerSettings;
			if (converters != null && converters.Length != 0)
			{
				(jsonSerializerSettings = new JsonSerializerSettings()).Converters = converters;
			}
			else
			{
				jsonSerializerSettings = null;
			}
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.SerializeObject(value, null, formatting, settings);
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0000EB83 File Offset: 0x0000CD83
		[DebuggerStepThrough]
		public static string SerializeObject([Nullable(2)] object value, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, settings);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0004E938 File Offset: 0x0004CB38
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Type type, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0000EB8D File Offset: 0x0000CD8D
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(value, null, formatting, settings);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0004E954 File Offset: 0x0004CB54
		[NullableContext(2)]
		[DebuggerStepThrough]
		[return: Nullable(1)]
		public static string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			jsonSerializer.Formatting = formatting;
			return JsonConvert.SerializeObjectInternal(value, type, jsonSerializer);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0004E978 File Offset: 0x0004CB78
		private static string SerializeObjectInternal([Nullable(2)] object value, [Nullable(2)] Type type, JsonSerializer jsonSerializer)
		{
			StringWriter stringWriter = new StringWriter(new StringBuilder(256), CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = jsonSerializer.Formatting;
				jsonSerializer.Serialize(jsonTextWriter, value, type);
			}
			return stringWriter.ToString();
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0000EB98 File Offset: 0x0000CD98
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value)
		{
			return JsonConvert.DeserializeObject(value, null, null);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0000EBA2 File Offset: 0x0000CDA2
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject(value, null, settings);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0000EBAC File Offset: 0x0000CDAC
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, Type type)
		{
			return JsonConvert.DeserializeObject(value, type, null);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0000EBB6 File Offset: 0x0000CDB6
		[DebuggerStepThrough]
		public static T DeserializeObject<[Nullable(2)] T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, null);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0000EBBF File Offset: 0x0000CDBF
		[DebuggerStepThrough]
		public static T DeserializeAnonymousType<[Nullable(2)] T>(string value, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
		[DebuggerStepThrough]
		public static T DeserializeAnonymousType<[Nullable(2)] T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0000EBD0 File Offset: 0x0000CDD0
		[DebuggerStepThrough]
		[return: MaybeNull]
		public static T DeserializeObject<[Nullable(2)] T>(string value, params JsonConverter[] converters)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), converters));
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		[DebuggerStepThrough]
		[return: MaybeNull]
		public static T DeserializeObject<[Nullable(2)] T>(string value, [Nullable(2)] JsonSerializerSettings settings)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), settings));
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0004E9D8 File Offset: 0x0004CBD8
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			JsonSerializerSettings jsonSerializerSettings;
			if (converters != null && converters.Length != 0)
			{
				(jsonSerializerSettings = new JsonSerializerSettings()).Converters = converters;
			}
			else
			{
				jsonSerializerSettings = null;
			}
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.DeserializeObject(value, type, settings);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0004EA04 File Offset: 0x0004CC04
		[NullableContext(2)]
		public static object DeserializeObject([Nullable(1)] string value, Type type, JsonSerializerSettings settings)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			if (!jsonSerializer.IsCheckAdditionalContentSet())
			{
				jsonSerializer.CheckAdditionalContent = true;
			}
			object result;
			using (JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(value)))
			{
				result = jsonSerializer.Deserialize(jsonTextReader, type);
			}
			return result;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0000EC00 File Offset: 0x0000CE00
		[DebuggerStepThrough]
		public static void PopulateObject(string value, object target)
		{
			JsonConvert.PopulateObject(value, target, null);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0004EA64 File Offset: 0x0004CC64
		public static void PopulateObject(string value, object target, [Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(value)))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (settings != null && settings.CheckAdditionalContent)
				{
					while (jsonReader.Read())
					{
						if (jsonReader.TokenType != JsonToken.Comment)
						{
							throw JsonSerializationException.Create(jsonReader, "Additional text found in JSON string after finishing deserializing object.");
						}
					}
				}
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0000EC0A File Offset: 0x0000CE0A
		public static string SerializeXmlNode([Nullable(2)] XmlNode node)
		{
			return JsonConvert.SerializeXmlNode(node, Formatting.None);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0004EAD8 File Offset: 0x0004CCD8
		public static string SerializeXmlNode([Nullable(2)] XmlNode node, Formatting formatting)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0004EAFC File Offset: 0x0004CCFC
		public static string SerializeXmlNode([Nullable(2)] XmlNode node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0000EC13 File Offset: 0x0000CE13
		[return: Nullable(2)]
		public static XmlDocument DeserializeXmlNode(string value)
		{
			return JsonConvert.DeserializeXmlNode(value, null);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0000EC1C File Offset: 0x0000CE1C
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, false);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0000EC26 File Offset: 0x0000CE26
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			return JsonConvert.DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute, false);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0004EB28 File Offset: 0x0004CD28
		[NullableContext(2)]
		public static XmlDocument DeserializeXmlNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			xmlNodeConverter.EncodeSpecialCharacters = encodeSpecialCharacters;
			return (XmlDocument)JsonConvert.DeserializeObject(value, typeof(XmlDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0000EC31 File Offset: 0x0000CE31
		public static string SerializeXNode([Nullable(2)] XObject node)
		{
			return JsonConvert.SerializeXNode(node, Formatting.None);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0000EC3A File Offset: 0x0000CE3A
		public static string SerializeXNode([Nullable(2)] XObject node, Formatting formatting)
		{
			return JsonConvert.SerializeXNode(node, formatting, false);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0004EAFC File Offset: 0x0004CCFC
		public static string SerializeXNode([Nullable(2)] XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter
			{
				OmitRootObject = omitRootObject
			};
			return JsonConvert.SerializeObject(node, formatting, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0000EC44 File Offset: 0x0000CE44
		[return: Nullable(2)]
		public static XDocument DeserializeXNode(string value)
		{
			return JsonConvert.DeserializeXNode(value, null);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0000EC4D File Offset: 0x0000CE4D
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, false);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0000EC57 File Offset: 0x0000CE57
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			return JsonConvert.DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute, false);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0004EB70 File Offset: 0x0004CD70
		[NullableContext(2)]
		public static XDocument DeserializeXNode([Nullable(1)] string value, string deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			xmlNodeConverter.EncodeSpecialCharacters = encodeSpecialCharacters;
			return (XDocument)JsonConvert.DeserializeObject(value, typeof(XDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x040007B3 RID: 1971
		public static readonly string True = "true";

		// Token: 0x040007B4 RID: 1972
		public static readonly string False = "false";

		// Token: 0x040007B5 RID: 1973
		public static readonly string Null = "null";

		// Token: 0x040007B6 RID: 1974
		public static readonly string Undefined = "undefined";

		// Token: 0x040007B7 RID: 1975
		public static readonly string PositiveInfinity = "Infinity";

		// Token: 0x040007B8 RID: 1976
		public static readonly string NegativeInfinity = "-Infinity";

		// Token: 0x040007B9 RID: 1977
		public static readonly string NaN = "NaN";
	}
}
