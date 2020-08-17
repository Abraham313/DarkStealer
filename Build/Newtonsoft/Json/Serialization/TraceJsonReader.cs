using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200028D RID: 653
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x060012FA RID: 4858 RVA: 0x000677F4 File Offset: 0x000659F4
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00013DD9 File Offset: 0x00011FD9
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00013DE6 File Offset: 0x00011FE6
		public override bool Read()
		{
			bool result = this._innerReader.Read();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00013DF9 File Offset: 0x00011FF9
		public override int? ReadAsInt32()
		{
			int? result = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00013E0C File Offset: 0x0001200C
		[NullableContext(2)]
		public override string ReadAsString()
		{
			string result = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00013E1F File Offset: 0x0001201F
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			byte[] result = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00013E32 File Offset: 0x00012032
		public override decimal? ReadAsDecimal()
		{
			decimal? result = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00013E45 File Offset: 0x00012045
		public override double? ReadAsDouble()
		{
			double? result = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00013E58 File Offset: 0x00012058
		public override bool? ReadAsBoolean()
		{
			bool? result = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00013E6B File Offset: 0x0001206B
		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00013E7E File Offset: 0x0001207E
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return result;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00013E91 File Offset: 0x00012091
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x00013EA7 File Offset: 0x000120A7
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x00013EB4 File Offset: 0x000120B4
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x00013EC1 File Offset: 0x000120C1
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x00013ECE File Offset: 0x000120CE
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00013EDC File Offset: 0x000120DC
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x00013EE9 File Offset: 0x000120E9
		[Nullable(2)]
		public override object Value
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00013EF6 File Offset: 0x000120F6
		[Nullable(2)]
		public override Type ValueType
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00013F03 File Offset: 0x00012103
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00067858 File Offset: 0x00065A58
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0006787C File Offset: 0x00065A7C
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x000678A0 File Offset: 0x00065AA0
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x04000B0C RID: 2828
		private readonly JsonReader _innerReader;

		// Token: 0x04000B0D RID: 2829
		private readonly JsonTextWriter _textWriter;

		// Token: 0x04000B0E RID: 2830
		private readonly StringWriter _sw;
	}
}
