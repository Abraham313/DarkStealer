using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001CC RID: 460
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonSerializerSettings
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0000F702 File Offset: 0x0000D902
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0000F70F File Offset: 0x0000D90F
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0000F71D File Offset: 0x0000D91D
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x0000F72A File Offset: 0x0000D92A
		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling.GetValueOrDefault();
			}
			set
			{
				this._missingMemberHandling = new MissingMemberHandling?(value);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0000F738 File Offset: 0x0000D938
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x0000F745 File Offset: 0x0000D945
		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling.GetValueOrDefault();
			}
			set
			{
				this._objectCreationHandling = new ObjectCreationHandling?(value);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0000F753 File Offset: 0x0000D953
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x0000F760 File Offset: 0x0000D960
		public NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0000F76E File Offset: 0x0000D96E
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0000F77B File Offset: 0x0000D97B
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0000F789 File Offset: 0x0000D989
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0000F791 File Offset: 0x0000D991
		[Nullable(1)]
		public IList<JsonConverter> Converters { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0000F79A File Offset: 0x0000D99A
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x0000F7A7 File Offset: 0x0000D9A7
		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling.GetValueOrDefault();
			}
			set
			{
				this._preserveReferencesHandling = new PreserveReferencesHandling?(value);
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0000F7B5 File Offset: 0x0000D9B5
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0000F7C2 File Offset: 0x0000D9C2
		public TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0000F7D0 File Offset: 0x0000D9D0
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x0000F7DD File Offset: 0x0000D9DD
		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._metadataPropertyHandling.GetValueOrDefault();
			}
			set
			{
				this._metadataPropertyHandling = new MetadataPropertyHandling?(value);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0000F7EB File Offset: 0x0000D9EB
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x0000F7F3 File Offset: 0x0000D9F3
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return (FormatterAssemblyStyle)this.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this.TypeNameAssemblyFormatHandling = (TypeNameAssemblyFormatHandling)value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x0000F809 File Offset: 0x0000DA09
		public TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._typeNameAssemblyFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._typeNameAssemblyFormatHandling = new TypeNameAssemblyFormatHandling?(value);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0000F817 File Offset: 0x0000DA17
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x0000F824 File Offset: 0x0000DA24
		public ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling.GetValueOrDefault();
			}
			set
			{
				this._constructorHandling = new ConstructorHandling?(value);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0000F832 File Offset: 0x0000DA32
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x0000F83A File Offset: 0x0000DA3A
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0000F843 File Offset: 0x0000DA43
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x0000F84B File Offset: 0x0000DA4B
		public IEqualityComparer EqualityComparer { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0000F854 File Offset: 0x0000DA54
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x00050D0C File Offset: 0x0004EF0C
		[Obsolete("ReferenceResolver property is obsolete. Use the ReferenceResolverProvider property to set the IReferenceResolver: settings.ReferenceResolverProvider = () => resolver")]
		public IReferenceResolver ReferenceResolver
		{
			get
			{
				Func<IReferenceResolver> referenceResolverProvider = this.ReferenceResolverProvider;
				if (referenceResolverProvider == null)
				{
					return null;
				}
				return referenceResolverProvider();
			}
			set
			{
				this.ReferenceResolverProvider = ((value != null) ? (() => value) : null);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0000F867 File Offset: 0x0000DA67
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x0000F86F File Offset: 0x0000DA6F
		public Func<IReferenceResolver> ReferenceResolverProvider { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0000F878 File Offset: 0x0000DA78
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x0000F880 File Offset: 0x0000DA80
		public ITraceWriter TraceWriter { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00050D44 File Offset: 0x0004EF44
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x0000F889 File Offset: 0x0000DA89
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public SerializationBinder Binder
		{
			get
			{
				if (this.SerializationBinder == null)
				{
					return null;
				}
				SerializationBinderAdapter serializationBinderAdapter = this.SerializationBinder as SerializationBinderAdapter;
				if (serializationBinderAdapter == null)
				{
					throw new InvalidOperationException("Cannot get SerializationBinder because an ISerializationBinder was previously set.");
				}
				return serializationBinderAdapter.SerializationBinder;
			}
			set
			{
				this.SerializationBinder = ((value == null) ? null : new SerializationBinderAdapter(value));
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0000F89D File Offset: 0x0000DA9D
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x0000F8A5 File Offset: 0x0000DAA5
		public ISerializationBinder SerializationBinder { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0000F8AE File Offset: 0x0000DAAE
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x0000F8B6 File Offset: 0x0000DAB6
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public EventHandler<ErrorEventArgs> Error { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00050D7C File Offset: 0x0004EF7C
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x0000F8BF File Offset: 0x0000DABF
		public StreamingContext Context
		{
			get
			{
				StreamingContext? context = this._context;
				if (context == null)
				{
					return JsonSerializerSettings.DefaultContext;
				}
				return context.GetValueOrDefault();
			}
			set
			{
				this._context = new StreamingContext?(value);
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0000F8CD File Offset: 0x0000DACD
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0000F8DE File Offset: 0x0000DADE
		[Nullable(1)]
		public string DateFormatString
		{
			[NullableContext(1)]
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			[NullableContext(1)]
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0000F8EE File Offset: 0x0000DAEE
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00050DA8 File Offset: 0x0004EFA8
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
				this._maxDepthSet = true;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0000F8F6 File Offset: 0x0000DAF6
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x0000F903 File Offset: 0x0000DB03
		public Formatting Formatting
		{
			get
			{
				return this._formatting.GetValueOrDefault();
			}
			set
			{
				this._formatting = new Formatting?(value);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0000F911 File Offset: 0x0000DB11
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x0000F91E File Offset: 0x0000DB1E
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._dateFormatHandling = new DateFormatHandling?(value);
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00050DEC File Offset: 0x0004EFEC
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x0000F92C File Offset: 0x0000DB2C
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				DateTimeZoneHandling? dateTimeZoneHandling = this._dateTimeZoneHandling;
				if (dateTimeZoneHandling == null)
				{
					return DateTimeZoneHandling.RoundtripKind;
				}
				return dateTimeZoneHandling.GetValueOrDefault();
			}
			set
			{
				this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00050E14 File Offset: 0x0004F014
		// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x0000F93A File Offset: 0x0000DB3A
		public DateParseHandling DateParseHandling
		{
			get
			{
				DateParseHandling? dateParseHandling = this._dateParseHandling;
				if (dateParseHandling == null)
				{
					return DateParseHandling.DateTime;
				}
				return dateParseHandling.GetValueOrDefault();
			}
			set
			{
				this._dateParseHandling = new DateParseHandling?(value);
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0000F948 File Offset: 0x0000DB48
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x0000F955 File Offset: 0x0000DB55
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling.GetValueOrDefault();
			}
			set
			{
				this._floatFormatHandling = new FloatFormatHandling?(value);
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0000F963 File Offset: 0x0000DB63
		// (set) Token: 0x06000CCD RID: 3277 RVA: 0x0000F970 File Offset: 0x0000DB70
		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._floatParseHandling.GetValueOrDefault();
			}
			set
			{
				this._floatParseHandling = new FloatParseHandling?(value);
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0000F97E File Offset: 0x0000DB7E
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0000F98B File Offset: 0x0000DB8B
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling.GetValueOrDefault();
			}
			set
			{
				this._stringEscapeHandling = new StringEscapeHandling?(value);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0000F999 File Offset: 0x0000DB99
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0000F9AA File Offset: 0x0000DBAA
		[Nullable(1)]
		public CultureInfo Culture
		{
			[NullableContext(1)]
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			[NullableContext(1)]
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0000F9B3 File Offset: 0x0000DBB3
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		public bool CheckAdditionalContent
		{
			get
			{
				return this._checkAdditionalContent.GetValueOrDefault();
			}
			set
			{
				this._checkAdditionalContent = new bool?(value);
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0000F9E5 File Offset: 0x0000DBE5
		[DebuggerStepThrough]
		public JsonSerializerSettings()
		{
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x0400081E RID: 2078
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x0400081F RID: 2079
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000820 RID: 2080
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000821 RID: 2081
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000822 RID: 2082
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000823 RID: 2083
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x04000824 RID: 2084
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x04000825 RID: 2085
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		// Token: 0x04000826 RID: 2086
		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		// Token: 0x04000827 RID: 2087
		internal static readonly StreamingContext DefaultContext = default(StreamingContext);

		// Token: 0x04000828 RID: 2088
		internal const Formatting DefaultFormatting = Formatting.None;

		// Token: 0x04000829 RID: 2089
		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		// Token: 0x0400082A RID: 2090
		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		// Token: 0x0400082B RID: 2091
		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		// Token: 0x0400082C RID: 2092
		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		// Token: 0x0400082D RID: 2093
		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		// Token: 0x0400082E RID: 2094
		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		// Token: 0x0400082F RID: 2095
		internal const TypeNameAssemblyFormatHandling DefaultTypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

		// Token: 0x04000830 RID: 2096
		[Nullable(1)]
		internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

		// Token: 0x04000831 RID: 2097
		internal const bool DefaultCheckAdditionalContent = false;

		// Token: 0x04000832 RID: 2098
		[Nullable(1)]
		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x04000833 RID: 2099
		internal Formatting? _formatting;

		// Token: 0x04000834 RID: 2100
		internal DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000835 RID: 2101
		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x04000836 RID: 2102
		internal DateParseHandling? _dateParseHandling;

		// Token: 0x04000837 RID: 2103
		internal FloatFormatHandling? _floatFormatHandling;

		// Token: 0x04000838 RID: 2104
		internal FloatParseHandling? _floatParseHandling;

		// Token: 0x04000839 RID: 2105
		internal StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x0400083A RID: 2106
		internal CultureInfo _culture;

		// Token: 0x0400083B RID: 2107
		internal bool? _checkAdditionalContent;

		// Token: 0x0400083C RID: 2108
		internal int? _maxDepth;

		// Token: 0x0400083D RID: 2109
		internal bool _maxDepthSet;

		// Token: 0x0400083E RID: 2110
		internal string _dateFormatString;

		// Token: 0x0400083F RID: 2111
		internal bool _dateFormatStringSet;

		// Token: 0x04000840 RID: 2112
		internal TypeNameAssemblyFormatHandling? _typeNameAssemblyFormatHandling;

		// Token: 0x04000841 RID: 2113
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x04000842 RID: 2114
		internal PreserveReferencesHandling? _preserveReferencesHandling;

		// Token: 0x04000843 RID: 2115
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x04000844 RID: 2116
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x04000845 RID: 2117
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x04000846 RID: 2118
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x04000847 RID: 2119
		internal StreamingContext? _context;

		// Token: 0x04000848 RID: 2120
		internal ConstructorHandling? _constructorHandling;

		// Token: 0x04000849 RID: 2121
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x0400084A RID: 2122
		internal MetadataPropertyHandling? _metadataPropertyHandling;
	}
}
