using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020002FF RID: 767
	[NullableContext(1)]
	[Nullable(0)]
	public class RegexConverter : JsonConverter
	{
		// Token: 0x060017F6 RID: 6134 RVA: 0x000746DC File Offset: 0x000728DC
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Regex regex = (Regex)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			if (bsonWriter != null)
			{
				this.WriteBson(bsonWriter, regex);
				return;
			}
			this.WriteJson(writer, regex, serializer);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00013680 File Offset: 0x00011880
		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00074718 File Offset: 0x00072918
		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string text = null;
			if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
			{
				text += "i";
			}
			if (this.HasFlag(regex.Options, RegexOptions.Multiline))
			{
				text += "m";
			}
			if (this.HasFlag(regex.Options, RegexOptions.Singleline))
			{
				text += "s";
			}
			text += "u";
			if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x000747B0 File Offset: 0x000729B0
		private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Options") : "Options");
			serializer.Serialize(writer, regex.Options);
			writer.WriteEndObject();
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0007482C File Offset: 0x00072A2C
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadRegexObject(reader, serializer);
			}
			if (tokenType == JsonToken.String)
			{
				return this.ReadRegexString(reader);
			}
			if (tokenType != JsonToken.Null)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
			}
			return null;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00074870 File Offset: 0x00072A70
		private object ReadRegexString(JsonReader reader)
		{
			string text = (string)reader.Value;
			if (text.Length > 0 && text[0] == '/')
			{
				int num = text.LastIndexOf('/');
				if (num > 0)
				{
					string pattern = text.Substring(1, num - 1);
					RegexOptions regexOptions = MiscellaneousUtils.GetRegexOptions(text.Substring(num + 1));
					return new Regex(pattern, regexOptions);
				}
			}
			throw JsonSerializationException.Create(reader, "Regex pattern must be enclosed by slashes.");
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x000748D8 File Offset: 0x00072AD8
		private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
		{
			string text = null;
			RegexOptions? regexOptions = null;
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndObject)
						{
							if (text == null)
							{
								throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
							}
							return new Regex(text, regexOptions.GetValueOrDefault());
						}
					}
				}
				else
				{
					string a = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
					}
					if (string.Equals(a, "Pattern", StringComparison.OrdinalIgnoreCase))
					{
						text = (string)reader.Value;
					}
					else if (string.Equals(a, "Options", StringComparison.OrdinalIgnoreCase))
					{
						regexOptions = new RegexOptions?(serializer.Deserialize<RegexOptions>(reader));
					}
					else
					{
						reader.Skip();
					}
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0001757C File Offset: 0x0001577C
		public override bool CanConvert(Type objectType)
		{
			return objectType.Name == "Regex" && this.IsRegex(objectType);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00017599 File Offset: 0x00015799
		private bool IsRegex(Type objectType)
		{
			return objectType == typeof(Regex);
		}

		// Token: 0x04000CEC RID: 3308
		private const string PatternName = "Pattern";

		// Token: 0x04000CED RID: 3309
		private const string OptionsName = "Options";
	}
}
