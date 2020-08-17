using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200027F RID: 639
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001272 RID: 4722 RVA: 0x00013758 File Offset: 0x00011958
		// (remove) Token: 0x06001273 RID: 4723 RVA: 0x00013766 File Offset: 0x00011966
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x00013774 File Offset: 0x00011974
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x00013781 File Offset: 0x00011981
		[Nullable(2)]
		public override IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.ReferenceResolver;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0001378F File Offset: 0x0001198F
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0001379C File Offset: 0x0001199C
		[Nullable(2)]
		public override ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.TraceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x000137AA File Offset: 0x000119AA
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x000137B7 File Offset: 0x000119B7
		[Nullable(2)]
		public override IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.EqualityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.EqualityComparer = value;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x000137C5 File Offset: 0x000119C5
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x000137D2 File Offset: 0x000119D2
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x000137DF File Offset: 0x000119DF
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x000137ED File Offset: 0x000119ED
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x000137FA File Offset: 0x000119FA
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00013808 File Offset: 0x00011A08
		// (set) Token: 0x06001280 RID: 4736 RVA: 0x00013815 File Offset: 0x00011A15
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00013823 File Offset: 0x00011A23
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00013830 File Offset: 0x00011A30
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x0001383E File Offset: 0x00011A3E
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x0001384B File Offset: 0x00011A4B
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x00013859 File Offset: 0x00011A59
		// (set) Token: 0x06001286 RID: 4742 RVA: 0x00013866 File Offset: 0x00011A66
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00013874 File Offset: 0x00011A74
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00013881 File Offset: 0x00011A81
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x0001388F File Offset: 0x00011A8F
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x0001389C File Offset: 0x00011A9C
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x000138AA File Offset: 0x00011AAA
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x000138B7 File Offset: 0x00011AB7
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x000138C5 File Offset: 0x00011AC5
		// (set) Token: 0x0600128E RID: 4750 RVA: 0x000138D2 File Offset: 0x00011AD2
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600128F RID: 4751 RVA: 0x000138E0 File Offset: 0x00011AE0
		// (set) Token: 0x06001290 RID: 4752 RVA: 0x000138ED File Offset: 0x00011AED
		public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x000138FB File Offset: 0x00011AFB
		// (set) Token: 0x06001292 RID: 4754 RVA: 0x00013908 File Offset: 0x00011B08
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00013916 File Offset: 0x00011B16
		// (set) Token: 0x06001294 RID: 4756 RVA: 0x00013923 File Offset: 0x00011B23
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x00013931 File Offset: 0x00011B31
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x0001393E File Offset: 0x00011B3E
		public override ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializer.SerializationBinder;
			}
			set
			{
				this._serializer.SerializationBinder = value;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0001394C File Offset: 0x00011B4C
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x00013959 File Offset: 0x00011B59
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00013967 File Offset: 0x00011B67
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x00013974 File Offset: 0x00011B74
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x00013982 File Offset: 0x00011B82
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x0001398F File Offset: 0x00011B8F
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0001399D File Offset: 0x00011B9D
		// (set) Token: 0x0600129E RID: 4766 RVA: 0x000139AA File Offset: 0x00011BAA
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x000139B8 File Offset: 0x00011BB8
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x000139C5 File Offset: 0x00011BC5
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x000139D3 File Offset: 0x00011BD3
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x000139E0 File Offset: 0x00011BE0
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x000139EE File Offset: 0x00011BEE
		// (set) Token: 0x060012A4 RID: 4772 RVA: 0x000139FB File Offset: 0x00011BFB
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x00013A09 File Offset: 0x00011C09
		// (set) Token: 0x060012A6 RID: 4774 RVA: 0x00013A16 File Offset: 0x00011C16
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x00013A24 File Offset: 0x00011C24
		// (set) Token: 0x060012A8 RID: 4776 RVA: 0x00013A31 File Offset: 0x00011C31
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x00013A3F File Offset: 0x00011C3F
		// (set) Token: 0x060012AA RID: 4778 RVA: 0x00013A4C File Offset: 0x00011C4C
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x00013A5A File Offset: 0x00011C5A
		// (set) Token: 0x060012AC RID: 4780 RVA: 0x00013A67 File Offset: 0x00011C67
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x00013A75 File Offset: 0x00011C75
		// (set) Token: 0x060012AE RID: 4782 RVA: 0x00013A82 File Offset: 0x00011C82
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00013A90 File Offset: 0x00011C90
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00013AA7 File Offset: 0x00011CA7
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00013ACD File Offset: 0x00011CCD
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00013AF3 File Offset: 0x00011CF3
		[NullableContext(2)]
		internal override object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00013B19 File Offset: 0x00011D19
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00013B3E File Offset: 0x00011D3E
		[NullableContext(2)]
		internal override void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x04000AEF RID: 2799
		[Nullable(2)]
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x04000AF0 RID: 2800
		[Nullable(2)]
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x04000AF1 RID: 2801
		private readonly JsonSerializer _serializer;
	}
}
