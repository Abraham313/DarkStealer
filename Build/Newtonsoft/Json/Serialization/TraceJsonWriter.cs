using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200028E RID: 654
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonWriter : JsonWriter
	{
		// Token: 0x06001311 RID: 4881 RVA: 0x000678C4 File Offset: 0x00065AC4
		public TraceJsonWriter(JsonWriter innerWriter)
		{
			this._innerWriter = innerWriter;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Serialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
			this._textWriter.Culture = innerWriter.Culture;
			this._textWriter.DateFormatHandling = innerWriter.DateFormatHandling;
			this._textWriter.DateFormatString = innerWriter.DateFormatString;
			this._textWriter.DateTimeZoneHandling = innerWriter.DateTimeZoneHandling;
			this._textWriter.FloatFormatHandling = innerWriter.FloatFormatHandling;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00013F10 File Offset: 0x00012110
		public string GetSerializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00013F1D File Offset: 0x0001211D
		public override void WriteValue(decimal value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00013F3E File Offset: 0x0001213E
		public override void WriteValue(decimal? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00013F75 File Offset: 0x00012175
		public override void WriteValue(bool value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00013F96 File Offset: 0x00012196
		public override void WriteValue(bool? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00013FCD File Offset: 0x000121CD
		public override void WriteValue(byte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00013FEE File Offset: 0x000121EE
		public override void WriteValue(byte? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00014025 File Offset: 0x00012225
		public override void WriteValue(char value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00014046 File Offset: 0x00012246
		public override void WriteValue(char? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0001407D File Offset: 0x0001227D
		[NullableContext(2)]
		public override void WriteValue(byte[] value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000140A8 File Offset: 0x000122A8
		public override void WriteValue(DateTime value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000140C9 File Offset: 0x000122C9
		public override void WriteValue(DateTime? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00014100 File Offset: 0x00012300
		public override void WriteValue(DateTimeOffset value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00014121 File Offset: 0x00012321
		public override void WriteValue(DateTimeOffset? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00014158 File Offset: 0x00012358
		public override void WriteValue(double value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00014179 File Offset: 0x00012379
		public override void WriteValue(double? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x000141B0 File Offset: 0x000123B0
		public override void WriteUndefined()
		{
			this._textWriter.WriteUndefined();
			this._innerWriter.WriteUndefined();
			base.WriteUndefined();
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000141CE File Offset: 0x000123CE
		public override void WriteNull()
		{
			this._textWriter.WriteNull();
			this._innerWriter.WriteNull();
			base.WriteUndefined();
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000141EC File Offset: 0x000123EC
		public override void WriteValue(float value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0001420D File Offset: 0x0001240D
		public override void WriteValue(float? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00014244 File Offset: 0x00012444
		public override void WriteValue(Guid value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00014265 File Offset: 0x00012465
		public override void WriteValue(Guid? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0001429C File Offset: 0x0001249C
		public override void WriteValue(int value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x000142BD File Offset: 0x000124BD
		public override void WriteValue(int? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000142F4 File Offset: 0x000124F4
		public override void WriteValue(long value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00014315 File Offset: 0x00012515
		public override void WriteValue(long? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0006797C File Offset: 0x00065B7C
		[NullableContext(2)]
		public override void WriteValue(object value)
		{
			if (value is System.Numerics.BigInteger)
			{
				this._textWriter.WriteValue(value);
				this._innerWriter.WriteValue(value);
				base.InternalWriteValue(JsonToken.Integer);
				return;
			}
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0001434C File Offset: 0x0001254C
		public override void WriteValue(sbyte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0001436D File Offset: 0x0001256D
		public override void WriteValue(sbyte? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000143A4 File Offset: 0x000125A4
		public override void WriteValue(short value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000143C5 File Offset: 0x000125C5
		public override void WriteValue(short? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000143FC File Offset: 0x000125FC
		[NullableContext(2)]
		public override void WriteValue(string value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0001441D File Offset: 0x0001261D
		public override void WriteValue(TimeSpan value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0001443E File Offset: 0x0001263E
		public override void WriteValue(TimeSpan? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00014475 File Offset: 0x00012675
		public override void WriteValue(uint value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00014496 File Offset: 0x00012696
		public override void WriteValue(uint? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x000144CD File Offset: 0x000126CD
		public override void WriteValue(ulong value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000144EE File Offset: 0x000126EE
		public override void WriteValue(ulong? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00014525 File Offset: 0x00012725
		[NullableContext(2)]
		public override void WriteValue(Uri value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value == null)
			{
				base.WriteUndefined();
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00014556 File Offset: 0x00012756
		public override void WriteValue(ushort value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00014577 File Offset: 0x00012777
		public override void WriteValue(ushort? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			if (value != null)
			{
				base.WriteValue(value.GetValueOrDefault());
				return;
			}
			base.WriteUndefined();
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000145AE File Offset: 0x000127AE
		public override void WriteWhitespace(string ws)
		{
			this._textWriter.WriteWhitespace(ws);
			this._innerWriter.WriteWhitespace(ws);
			base.WriteWhitespace(ws);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000145CF File Offset: 0x000127CF
		[NullableContext(2)]
		public override void WriteComment(string text)
		{
			this._textWriter.WriteComment(text);
			this._innerWriter.WriteComment(text);
			base.WriteComment(text);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000145F0 File Offset: 0x000127F0
		public override void WriteStartArray()
		{
			this._textWriter.WriteStartArray();
			this._innerWriter.WriteStartArray();
			base.WriteStartArray();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0001460E File Offset: 0x0001280E
		public override void WriteEndArray()
		{
			this._textWriter.WriteEndArray();
			this._innerWriter.WriteEndArray();
			base.WriteEndArray();
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0001462C File Offset: 0x0001282C
		public override void WriteStartConstructor(string name)
		{
			this._textWriter.WriteStartConstructor(name);
			this._innerWriter.WriteStartConstructor(name);
			base.WriteStartConstructor(name);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0001464D File Offset: 0x0001284D
		public override void WriteEndConstructor()
		{
			this._textWriter.WriteEndConstructor();
			this._innerWriter.WriteEndConstructor();
			base.WriteEndConstructor();
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0001466B File Offset: 0x0001286B
		public override void WritePropertyName(string name)
		{
			this._textWriter.WritePropertyName(name);
			this._innerWriter.WritePropertyName(name);
			base.WritePropertyName(name);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0001468C File Offset: 0x0001288C
		public override void WritePropertyName(string name, bool escape)
		{
			this._textWriter.WritePropertyName(name, escape);
			this._innerWriter.WritePropertyName(name, escape);
			base.WritePropertyName(name);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000146AF File Offset: 0x000128AF
		public override void WriteStartObject()
		{
			this._textWriter.WriteStartObject();
			this._innerWriter.WriteStartObject();
			base.WriteStartObject();
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000146CD File Offset: 0x000128CD
		public override void WriteEndObject()
		{
			this._textWriter.WriteEndObject();
			this._innerWriter.WriteEndObject();
			base.WriteEndObject();
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000146EB File Offset: 0x000128EB
		[NullableContext(2)]
		public override void WriteRawValue(string json)
		{
			this._textWriter.WriteRawValue(json);
			this._innerWriter.WriteRawValue(json);
			base.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0001470D File Offset: 0x0001290D
		[NullableContext(2)]
		public override void WriteRaw(string json)
		{
			this._textWriter.WriteRaw(json);
			this._innerWriter.WriteRaw(json);
			base.WriteRaw(json);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0001472E File Offset: 0x0001292E
		public override void Close()
		{
			this._textWriter.Close();
			this._innerWriter.Close();
			base.Close();
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0001474C File Offset: 0x0001294C
		public override void Flush()
		{
			this._textWriter.Flush();
			this._innerWriter.Flush();
		}

		// Token: 0x04000B0F RID: 2831
		private readonly JsonWriter _innerWriter;

		// Token: 0x04000B10 RID: 2832
		private readonly JsonTextWriter _textWriter;

		// Token: 0x04000B11 RID: 2833
		private readonly StringWriter _sw;
	}
}
