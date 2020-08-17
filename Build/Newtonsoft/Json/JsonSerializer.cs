using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x020001CB RID: 459
	[NullableContext(1)]
	[Nullable(0)]
	public class JsonSerializer
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000C3C RID: 3132 RVA: 0x000500D8 File Offset: 0x0004E2D8
		// (remove) Token: 0x06000C3D RID: 3133 RVA: 0x00050110 File Offset: 0x0004E310
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		[Nullable(2)]
		public virtual IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this.GetReferenceResolver();
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				this._referenceResolver = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00050148 File Offset: 0x0004E348
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0000F2F0 File Offset: 0x0000D4F0
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public virtual SerializationBinder Binder
		{
			get
			{
				SerializationBinder serializationBinder = this._serializationBinder as SerializationBinder;
				if (serializationBinder != null)
				{
					return serializationBinder;
				}
				SerializationBinderAdapter serializationBinderAdapter = this._serializationBinder as SerializationBinderAdapter;
				if (serializationBinderAdapter == null)
				{
					throw new InvalidOperationException("Cannot get SerializationBinder because an ISerializationBinder was previously set.");
				}
				return serializationBinderAdapter.SerializationBinder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._serializationBinder = ((value as ISerializationBinder) ?? new SerializationBinderAdapter(value));
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0000F31B File Offset: 0x0000D51B
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0000F323 File Offset: 0x0000D523
		public virtual ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializationBinder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._serializationBinder = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0000F33F File Offset: 0x0000D53F
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0000F347 File Offset: 0x0000D547
		[Nullable(2)]
		public virtual ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._traceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._traceWriter = value;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0000F350 File Offset: 0x0000D550
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0000F358 File Offset: 0x0000D558
		[Nullable(2)]
		public virtual IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._equalityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._equalityComparer = value;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0000F361 File Offset: 0x0000D561
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0000F369 File Offset: 0x0000D569
		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameHandling = value;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0000F385 File Offset: 0x0000D585
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0000F38D File Offset: 0x0000D58D
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return (FormatterAssemblyStyle)this._typeNameAssemblyFormatHandling;
			}
			set
			{
				if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormatHandling = (TypeNameAssemblyFormatHandling)value;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0000F385 File Offset: 0x0000D585
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0000F38D File Offset: 0x0000D58D
		public virtual TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._typeNameAssemblyFormatHandling;
			}
			set
			{
				if (value < TypeNameAssemblyFormatHandling.Simple || value > TypeNameAssemblyFormatHandling.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0000F3A9 File Offset: 0x0000D5A9
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0000F3B1 File Offset: 0x0000D5B1
		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._preserveReferencesHandling = value;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0000F3CD File Offset: 0x0000D5CD
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0000F3D5 File Offset: 0x0000D5D5
		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._referenceLoopHandling = value;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0000F3F1 File Offset: 0x0000D5F1
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x0000F3F9 File Offset: 0x0000D5F9
		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._missingMemberHandling = value;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0000F415 File Offset: 0x0000D615
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x0000F41D File Offset: 0x0000D61D
		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._nullValueHandling = value;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0000F439 File Offset: 0x0000D639
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x0000F441 File Offset: 0x0000D641
		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultValueHandling = value;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0000F45D File Offset: 0x0000D65D
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0000F465 File Offset: 0x0000D665
		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._objectCreationHandling = value;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0000F481 File Offset: 0x0000D681
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x0000F489 File Offset: 0x0000D689
		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._constructorHandling = value;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0000F4A5 File Offset: 0x0000D6A5
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x0000F4AD File Offset: 0x0000D6AD
		public virtual MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._metadataPropertyHandling;
			}
			set
			{
				if (value < MetadataPropertyHandling.Default || value > MetadataPropertyHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._metadataPropertyHandling = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0000F4C9 File Offset: 0x0000D6C9
		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (this._converters == null)
				{
					this._converters = new JsonConverterCollection();
				}
				return this._converters;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0000F4E4 File Offset: 0x0000D6E4
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x0000F4EC File Offset: 0x0000D6EC
		public virtual IContractResolver ContractResolver
		{
			get
			{
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = (value ?? DefaultContractResolver.Instance);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0000F4FE File Offset: 0x0000D6FE
		// (set) Token: 0x06000C62 RID: 3170 RVA: 0x0000F506 File Offset: 0x0000D706
		public virtual StreamingContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0000F50F File Offset: 0x0000D70F
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x0000F51C File Offset: 0x0000D71C
		public virtual Formatting Formatting
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

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0000F52A File Offset: 0x0000D72A
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x0000F537 File Offset: 0x0000D737
		public virtual DateFormatHandling DateFormatHandling
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

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00050188 File Offset: 0x0004E388
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x0000F545 File Offset: 0x0000D745
		public virtual DateTimeZoneHandling DateTimeZoneHandling
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x000501B0 File Offset: 0x0004E3B0
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x0000F553 File Offset: 0x0000D753
		public virtual DateParseHandling DateParseHandling
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

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0000F561 File Offset: 0x0000D761
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x0000F56E File Offset: 0x0000D76E
		public virtual FloatParseHandling FloatParseHandling
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

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0000F57C File Offset: 0x0000D77C
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0000F589 File Offset: 0x0000D789
		public virtual FloatFormatHandling FloatFormatHandling
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

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0000F597 File Offset: 0x0000D797
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		public virtual StringEscapeHandling StringEscapeHandling
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

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0000F5B2 File Offset: 0x0000D7B2
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x0000F5C3 File Offset: 0x0000D7C3
		public virtual string DateFormatString
		{
			get
			{
				return this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				this._dateFormatString = value;
				this._dateFormatStringSet = true;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0000F5D3 File Offset: 0x0000D7D3
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
		public virtual CultureInfo Culture
		{
			get
			{
				return this._culture ?? JsonSerializerSettings.DefaultCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0000F5ED File Offset: 0x0000D7ED
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x000501D8 File Offset: 0x0004E3D8
		public virtual int? MaxDepth
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

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0000F5F5 File Offset: 0x0000D7F5
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x0000F602 File Offset: 0x0000D802
		public virtual bool CheckAdditionalContent
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

		// Token: 0x06000C79 RID: 3193 RVA: 0x0000F610 File Offset: 0x0000D810
		internal bool IsCheckAdditionalContentSet()
		{
			return this._checkAdditionalContent != null;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0005021C File Offset: 0x0004E41C
		public JsonSerializer()
		{
			this._referenceLoopHandling = ReferenceLoopHandling.Error;
			this._missingMemberHandling = MissingMemberHandling.Ignore;
			this._nullValueHandling = NullValueHandling.Include;
			this._defaultValueHandling = DefaultValueHandling.Include;
			this._objectCreationHandling = ObjectCreationHandling.Auto;
			this._preserveReferencesHandling = PreserveReferencesHandling.None;
			this._constructorHandling = ConstructorHandling.Default;
			this._typeNameHandling = TypeNameHandling.None;
			this._metadataPropertyHandling = MetadataPropertyHandling.Default;
			this._context = JsonSerializerSettings.DefaultContext;
			this._serializationBinder = DefaultSerializationBinder.Instance;
			this._culture = JsonSerializerSettings.DefaultCulture;
			this._contractResolver = DefaultContractResolver.Instance;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0000F61D File Offset: 0x0000D81D
		public static JsonSerializer Create()
		{
			return new JsonSerializer();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0005029C File Offset: 0x0004E49C
		public static JsonSerializer Create([Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.Create();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0000F624 File Offset: 0x0000D824
		public static JsonSerializer CreateDefault()
		{
			Func<JsonSerializerSettings> defaultSettings = JsonConvert.DefaultSettings;
			return JsonSerializer.Create((defaultSettings != null) ? defaultSettings() : null);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000502BC File Offset: 0x0004E4BC
		public static JsonSerializer CreateDefault([Nullable(2)] JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
			if (settings != null)
			{
				JsonSerializer.ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x000502DC File Offset: 0x0004E4DC
		private static void ApplySerializerSettings(JsonSerializer serializer, JsonSerializerSettings settings)
		{
			if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
			{
				for (int i = 0; i < settings.Converters.Count; i++)
				{
					serializer.Converters.Insert(i, settings.Converters[i]);
				}
			}
			if (settings._typeNameHandling != null)
			{
				serializer.TypeNameHandling = settings.TypeNameHandling;
			}
			if (settings._metadataPropertyHandling != null)
			{
				serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
			}
			if (settings._typeNameAssemblyFormatHandling != null)
			{
				serializer.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
			}
			if (settings._preserveReferencesHandling != null)
			{
				serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
			}
			if (settings._referenceLoopHandling != null)
			{
				serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
			}
			if (settings._missingMemberHandling != null)
			{
				serializer.MissingMemberHandling = settings.MissingMemberHandling;
			}
			if (settings._objectCreationHandling != null)
			{
				serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
			}
			if (settings._nullValueHandling != null)
			{
				serializer.NullValueHandling = settings.NullValueHandling;
			}
			if (settings._defaultValueHandling != null)
			{
				serializer.DefaultValueHandling = settings.DefaultValueHandling;
			}
			if (settings._constructorHandling != null)
			{
				serializer.ConstructorHandling = settings.ConstructorHandling;
			}
			if (settings._context != null)
			{
				serializer.Context = settings.Context;
			}
			if (settings._checkAdditionalContent != null)
			{
				serializer._checkAdditionalContent = settings._checkAdditionalContent;
			}
			if (settings.Error != null)
			{
				serializer.Error += settings.Error;
			}
			if (settings.ContractResolver != null)
			{
				serializer.ContractResolver = settings.ContractResolver;
			}
			if (settings.ReferenceResolverProvider != null)
			{
				serializer.ReferenceResolver = settings.ReferenceResolverProvider();
			}
			if (settings.TraceWriter != null)
			{
				serializer.TraceWriter = settings.TraceWriter;
			}
			if (settings.EqualityComparer != null)
			{
				serializer.EqualityComparer = settings.EqualityComparer;
			}
			if (settings.SerializationBinder != null)
			{
				serializer.SerializationBinder = settings.SerializationBinder;
			}
			if (settings._formatting != null)
			{
				serializer._formatting = settings._formatting;
			}
			if (settings._dateFormatHandling != null)
			{
				serializer._dateFormatHandling = settings._dateFormatHandling;
			}
			if (settings._dateTimeZoneHandling != null)
			{
				serializer._dateTimeZoneHandling = settings._dateTimeZoneHandling;
			}
			if (settings._dateParseHandling != null)
			{
				serializer._dateParseHandling = settings._dateParseHandling;
			}
			if (settings._dateFormatStringSet)
			{
				serializer._dateFormatString = settings._dateFormatString;
				serializer._dateFormatStringSet = settings._dateFormatStringSet;
			}
			if (settings._floatFormatHandling != null)
			{
				serializer._floatFormatHandling = settings._floatFormatHandling;
			}
			if (settings._floatParseHandling != null)
			{
				serializer._floatParseHandling = settings._floatParseHandling;
			}
			if (settings._stringEscapeHandling != null)
			{
				serializer._stringEscapeHandling = settings._stringEscapeHandling;
			}
			if (settings._culture != null)
			{
				serializer._culture = settings._culture;
			}
			if (settings._maxDepthSet)
			{
				serializer._maxDepth = settings._maxDepth;
				serializer._maxDepthSet = settings._maxDepthSet;
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0000F63C File Offset: 0x0000D83C
		[DebuggerStepThrough]
		public void Populate(TextReader reader, object target)
		{
			this.Populate(new JsonTextReader(reader), target);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0000F64B File Offset: 0x0000D84B
		[DebuggerStepThrough]
		public void Populate(JsonReader reader, object target)
		{
			this.PopulateInternal(reader, target);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000505D0 File Offset: 0x0004E7D0
		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			CultureInfo previousCulture;
			DateTimeZoneHandling? previousDateTimeZoneHandling;
			DateParseHandling? previousDateParseHandling;
			FloatParseHandling? previousFloatParseHandling;
			int? previousMaxDepth;
			string previousDateFormatString;
			this.SetupReader(reader, out previousCulture, out previousDateTimeZoneHandling, out previousDateParseHandling, out previousFloatParseHandling, out previousMaxDepth, out previousDateFormatString);
			TraceJsonReader traceJsonReader = (this.TraceWriter == null || this.TraceWriter.LevelFilter < TraceLevel.Verbose) ? null : this.CreateTraceJsonReader(reader);
			new JsonSerializerInternalReader(this).Populate(traceJsonReader ?? reader, target);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonReader.GetDeserializedJsonMessage(), null);
			}
			this.ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0000F655 File Offset: 0x0000D855
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public object Deserialize(JsonReader reader)
		{
			return this.Deserialize(reader, null);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0000F65F File Offset: 0x0000D85F
		[DebuggerStepThrough]
		[return: Nullable(2)]
		public object Deserialize(TextReader reader, Type objectType)
		{
			return this.Deserialize(new JsonTextReader(reader), objectType);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0000F66E File Offset: 0x0000D86E
		[DebuggerStepThrough]
		[return: MaybeNull]
		public T Deserialize<[Nullable(2)] T>(JsonReader reader)
		{
			return (T)((object)this.Deserialize(reader, typeof(T)));
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0000F686 File Offset: 0x0000D886
		[NullableContext(2)]
		[DebuggerStepThrough]
		public object Deserialize([Nullable(1)] JsonReader reader, Type objectType)
		{
			return this.DeserializeInternal(reader, objectType);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00050664 File Offset: 0x0004E864
		[NullableContext(2)]
		internal virtual object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			CultureInfo previousCulture;
			DateTimeZoneHandling? previousDateTimeZoneHandling;
			DateParseHandling? previousDateParseHandling;
			FloatParseHandling? previousFloatParseHandling;
			int? previousMaxDepth;
			string previousDateFormatString;
			this.SetupReader(reader, out previousCulture, out previousDateTimeZoneHandling, out previousDateParseHandling, out previousFloatParseHandling, out previousMaxDepth, out previousDateFormatString);
			TraceJsonReader traceJsonReader = (this.TraceWriter == null || this.TraceWriter.LevelFilter < TraceLevel.Verbose) ? null : this.CreateTraceJsonReader(reader);
			object result = new JsonSerializerInternalReader(this).Deserialize(traceJsonReader ?? reader, objectType, this.CheckAdditionalContent);
			if (traceJsonReader != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonReader.GetDeserializedJsonMessage(), null);
			}
			this.ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
			return result;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000506F4 File Offset: 0x0004E8F4
		[NullableContext(2)]
		private void SetupReader([Nullable(1)] JsonReader reader, out CultureInfo previousCulture, out DateTimeZoneHandling? previousDateTimeZoneHandling, out DateParseHandling? previousDateParseHandling, out FloatParseHandling? previousFloatParseHandling, out int? previousMaxDepth, out string previousDateFormatString)
		{
			if (this._culture != null && !this._culture.Equals(reader.Culture))
			{
				previousCulture = reader.Culture;
				reader.Culture = this._culture;
			}
			else
			{
				previousCulture = null;
			}
			if (this._dateTimeZoneHandling != null)
			{
				DateTimeZoneHandling dateTimeZoneHandling = reader.DateTimeZoneHandling;
				DateTimeZoneHandling? dateTimeZoneHandling2 = this._dateTimeZoneHandling;
				if (!(dateTimeZoneHandling == dateTimeZoneHandling2.GetValueOrDefault() & dateTimeZoneHandling2 != null))
				{
					previousDateTimeZoneHandling = new DateTimeZoneHandling?(reader.DateTimeZoneHandling);
					reader.DateTimeZoneHandling = this._dateTimeZoneHandling.GetValueOrDefault();
					goto IL_8C;
				}
			}
			previousDateTimeZoneHandling = null;
			IL_8C:
			if (this._dateParseHandling != null)
			{
				DateParseHandling dateParseHandling = reader.DateParseHandling;
				DateParseHandling? dateParseHandling2 = this._dateParseHandling;
				if (!(dateParseHandling == dateParseHandling2.GetValueOrDefault() & dateParseHandling2 != null))
				{
					previousDateParseHandling = new DateParseHandling?(reader.DateParseHandling);
					reader.DateParseHandling = this._dateParseHandling.GetValueOrDefault();
					goto IL_E6;
				}
			}
			previousDateParseHandling = null;
			IL_E6:
			if (this._floatParseHandling != null)
			{
				FloatParseHandling floatParseHandling = reader.FloatParseHandling;
				FloatParseHandling? floatParseHandling2 = this._floatParseHandling;
				if (!(floatParseHandling == floatParseHandling2.GetValueOrDefault() & floatParseHandling2 != null))
				{
					previousFloatParseHandling = new FloatParseHandling?(reader.FloatParseHandling);
					reader.FloatParseHandling = this._floatParseHandling.GetValueOrDefault();
					goto IL_140;
				}
			}
			previousFloatParseHandling = null;
			IL_140:
			if (this._maxDepthSet)
			{
				int? maxDepth = reader.MaxDepth;
				int? maxDepth2 = this._maxDepth;
				if (!(maxDepth.GetValueOrDefault() == maxDepth2.GetValueOrDefault() & maxDepth != null == (maxDepth2 != null)))
				{
					previousMaxDepth = reader.MaxDepth;
					reader.MaxDepth = this._maxDepth;
					goto IL_19D;
				}
			}
			previousMaxDepth = null;
			IL_19D:
			if (this._dateFormatStringSet && reader.DateFormatString != this._dateFormatString)
			{
				previousDateFormatString = reader.DateFormatString;
				reader.DateFormatString = this._dateFormatString;
			}
			else
			{
				previousDateFormatString = null;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null && jsonTextReader.PropertyNameTable == null)
			{
				DefaultContractResolver defaultContractResolver = this._contractResolver as DefaultContractResolver;
				if (defaultContractResolver != null)
				{
					jsonTextReader.PropertyNameTable = defaultContractResolver.GetNameTable();
				}
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00050908 File Offset: 0x0004EB08
		[NullableContext(2)]
		private void ResetReader([Nullable(1)] JsonReader reader, CultureInfo previousCulture, DateTimeZoneHandling? previousDateTimeZoneHandling, DateParseHandling? previousDateParseHandling, FloatParseHandling? previousFloatParseHandling, int? previousMaxDepth, string previousDateFormatString)
		{
			if (previousCulture != null)
			{
				reader.Culture = previousCulture;
			}
			if (previousDateTimeZoneHandling != null)
			{
				reader.DateTimeZoneHandling = previousDateTimeZoneHandling.GetValueOrDefault();
			}
			if (previousDateParseHandling != null)
			{
				reader.DateParseHandling = previousDateParseHandling.GetValueOrDefault();
			}
			if (previousFloatParseHandling != null)
			{
				reader.FloatParseHandling = previousFloatParseHandling.GetValueOrDefault();
			}
			if (this._maxDepthSet)
			{
				reader.MaxDepth = previousMaxDepth;
			}
			if (this._dateFormatStringSet)
			{
				reader.DateFormatString = previousDateFormatString;
			}
			JsonTextReader jsonTextReader = reader as JsonTextReader;
			if (jsonTextReader != null && jsonTextReader.PropertyNameTable != null)
			{
				DefaultContractResolver defaultContractResolver = this._contractResolver as DefaultContractResolver;
				if (defaultContractResolver != null && jsonTextReader.PropertyNameTable == defaultContractResolver.GetNameTable())
				{
					jsonTextReader.PropertyNameTable = null;
				}
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0000F690 File Offset: 0x0000D890
		public void Serialize(TextWriter textWriter, [Nullable(2)] object value)
		{
			this.Serialize(new JsonTextWriter(textWriter), value);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0000F69F File Offset: 0x0000D89F
		[NullableContext(2)]
		public void Serialize([Nullable(1)] JsonWriter jsonWriter, object value, Type objectType)
		{
			this.SerializeInternal(jsonWriter, value, objectType);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0000F6AA File Offset: 0x0000D8AA
		public void Serialize(TextWriter textWriter, [Nullable(2)] object value, Type objectType)
		{
			this.Serialize(new JsonTextWriter(textWriter), value, objectType);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0000F6BA File Offset: 0x0000D8BA
		public void Serialize(JsonWriter jsonWriter, [Nullable(2)] object value)
		{
			this.SerializeInternal(jsonWriter, value, null);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000509B8 File Offset: 0x0004EBB8
		private TraceJsonReader CreateTraceJsonReader(JsonReader reader)
		{
			TraceJsonReader traceJsonReader = new TraceJsonReader(reader);
			if (reader.TokenType != JsonToken.None)
			{
				traceJsonReader.WriteCurrentToken();
			}
			return traceJsonReader;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000509DC File Offset: 0x0004EBDC
		[NullableContext(2)]
		internal virtual void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			Formatting? formatting = null;
			if (this._formatting != null)
			{
				Formatting formatting2 = jsonWriter.Formatting;
				Formatting? formatting3 = this._formatting;
				if (!(formatting2 == formatting3.GetValueOrDefault() & formatting3 != null))
				{
					formatting = new Formatting?(jsonWriter.Formatting);
					jsonWriter.Formatting = this._formatting.GetValueOrDefault();
				}
			}
			DateFormatHandling? dateFormatHandling = null;
			if (this._dateFormatHandling != null)
			{
				DateFormatHandling dateFormatHandling2 = jsonWriter.DateFormatHandling;
				DateFormatHandling? dateFormatHandling3 = this._dateFormatHandling;
				if (!(dateFormatHandling2 == dateFormatHandling3.GetValueOrDefault() & dateFormatHandling3 != null))
				{
					dateFormatHandling = new DateFormatHandling?(jsonWriter.DateFormatHandling);
					jsonWriter.DateFormatHandling = this._dateFormatHandling.GetValueOrDefault();
				}
			}
			DateTimeZoneHandling? dateTimeZoneHandling = null;
			if (this._dateTimeZoneHandling != null)
			{
				DateTimeZoneHandling dateTimeZoneHandling2 = jsonWriter.DateTimeZoneHandling;
				DateTimeZoneHandling? dateTimeZoneHandling3 = this._dateTimeZoneHandling;
				if (!(dateTimeZoneHandling2 == dateTimeZoneHandling3.GetValueOrDefault() & dateTimeZoneHandling3 != null))
				{
					dateTimeZoneHandling = new DateTimeZoneHandling?(jsonWriter.DateTimeZoneHandling);
					jsonWriter.DateTimeZoneHandling = this._dateTimeZoneHandling.GetValueOrDefault();
				}
			}
			FloatFormatHandling? floatFormatHandling = null;
			if (this._floatFormatHandling != null)
			{
				FloatFormatHandling floatFormatHandling2 = jsonWriter.FloatFormatHandling;
				FloatFormatHandling? floatFormatHandling3 = this._floatFormatHandling;
				if (!(floatFormatHandling2 == floatFormatHandling3.GetValueOrDefault() & floatFormatHandling3 != null))
				{
					floatFormatHandling = new FloatFormatHandling?(jsonWriter.FloatFormatHandling);
					jsonWriter.FloatFormatHandling = this._floatFormatHandling.GetValueOrDefault();
				}
			}
			StringEscapeHandling? stringEscapeHandling = null;
			if (this._stringEscapeHandling != null)
			{
				StringEscapeHandling stringEscapeHandling2 = jsonWriter.StringEscapeHandling;
				StringEscapeHandling? stringEscapeHandling3 = this._stringEscapeHandling;
				if (!(stringEscapeHandling2 == stringEscapeHandling3.GetValueOrDefault() & stringEscapeHandling3 != null))
				{
					stringEscapeHandling = new StringEscapeHandling?(jsonWriter.StringEscapeHandling);
					jsonWriter.StringEscapeHandling = this._stringEscapeHandling.GetValueOrDefault();
				}
			}
			CultureInfo cultureInfo = null;
			if (this._culture != null && !this._culture.Equals(jsonWriter.Culture))
			{
				cultureInfo = jsonWriter.Culture;
				jsonWriter.Culture = this._culture;
			}
			string dateFormatString = null;
			if (this._dateFormatStringSet && jsonWriter.DateFormatString != this._dateFormatString)
			{
				dateFormatString = jsonWriter.DateFormatString;
				jsonWriter.DateFormatString = this._dateFormatString;
			}
			TraceJsonWriter traceJsonWriter = (this.TraceWriter == null || this.TraceWriter.LevelFilter < TraceLevel.Verbose) ? null : new TraceJsonWriter(jsonWriter);
			new JsonSerializerInternalWriter(this).Serialize(traceJsonWriter ?? jsonWriter, value, objectType);
			if (traceJsonWriter != null)
			{
				this.TraceWriter.Trace(TraceLevel.Verbose, traceJsonWriter.GetSerializedJsonMessage(), null);
			}
			if (formatting != null)
			{
				jsonWriter.Formatting = formatting.GetValueOrDefault();
			}
			if (dateFormatHandling != null)
			{
				jsonWriter.DateFormatHandling = dateFormatHandling.GetValueOrDefault();
			}
			if (dateTimeZoneHandling != null)
			{
				jsonWriter.DateTimeZoneHandling = dateTimeZoneHandling.GetValueOrDefault();
			}
			if (floatFormatHandling != null)
			{
				jsonWriter.FloatFormatHandling = floatFormatHandling.GetValueOrDefault();
			}
			if (stringEscapeHandling != null)
			{
				jsonWriter.StringEscapeHandling = stringEscapeHandling.GetValueOrDefault();
			}
			if (this._dateFormatStringSet)
			{
				jsonWriter.DateFormatString = dateFormatString;
			}
			if (cultureInfo != null)
			{
				jsonWriter.Culture = cultureInfo;
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0000F6C5 File Offset: 0x0000D8C5
		internal IReferenceResolver GetReferenceResolver()
		{
			if (this._referenceResolver == null)
			{
				this._referenceResolver = new DefaultReferenceResolver();
			}
			return this._referenceResolver;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		[return: Nullable(2)]
		internal JsonConverter GetMatchingConverter(Type type)
		{
			return JsonSerializer.GetMatchingConverter(this._converters, type);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00050CD4 File Offset: 0x0004EED4
		[return: Nullable(2)]
		internal static JsonConverter GetMatchingConverter([Nullable(new byte[]
		{
			2,
			1
		})] IList<JsonConverter> converters, Type objectType)
		{
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0000F6EE File Offset: 0x0000D8EE
		internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
			if (error == null)
			{
				return;
			}
			error(this, e);
		}

		// Token: 0x040007FF RID: 2047
		internal TypeNameHandling _typeNameHandling;

		// Token: 0x04000800 RID: 2048
		internal TypeNameAssemblyFormatHandling _typeNameAssemblyFormatHandling;

		// Token: 0x04000801 RID: 2049
		internal PreserveReferencesHandling _preserveReferencesHandling;

		// Token: 0x04000802 RID: 2050
		internal ReferenceLoopHandling _referenceLoopHandling;

		// Token: 0x04000803 RID: 2051
		internal MissingMemberHandling _missingMemberHandling;

		// Token: 0x04000804 RID: 2052
		internal ObjectCreationHandling _objectCreationHandling;

		// Token: 0x04000805 RID: 2053
		internal NullValueHandling _nullValueHandling;

		// Token: 0x04000806 RID: 2054
		internal DefaultValueHandling _defaultValueHandling;

		// Token: 0x04000807 RID: 2055
		internal ConstructorHandling _constructorHandling;

		// Token: 0x04000808 RID: 2056
		internal MetadataPropertyHandling _metadataPropertyHandling;

		// Token: 0x04000809 RID: 2057
		[Nullable(2)]
		internal JsonConverterCollection _converters;

		// Token: 0x0400080A RID: 2058
		internal IContractResolver _contractResolver;

		// Token: 0x0400080B RID: 2059
		[Nullable(2)]
		internal ITraceWriter _traceWriter;

		// Token: 0x0400080C RID: 2060
		[Nullable(2)]
		internal IEqualityComparer _equalityComparer;

		// Token: 0x0400080D RID: 2061
		internal ISerializationBinder _serializationBinder;

		// Token: 0x0400080E RID: 2062
		internal StreamingContext _context;

		// Token: 0x0400080F RID: 2063
		[Nullable(2)]
		private IReferenceResolver _referenceResolver;

		// Token: 0x04000810 RID: 2064
		private Formatting? _formatting;

		// Token: 0x04000811 RID: 2065
		private DateFormatHandling? _dateFormatHandling;

		// Token: 0x04000812 RID: 2066
		private DateTimeZoneHandling? _dateTimeZoneHandling;

		// Token: 0x04000813 RID: 2067
		private DateParseHandling? _dateParseHandling;

		// Token: 0x04000814 RID: 2068
		private FloatFormatHandling? _floatFormatHandling;

		// Token: 0x04000815 RID: 2069
		private FloatParseHandling? _floatParseHandling;

		// Token: 0x04000816 RID: 2070
		private StringEscapeHandling? _stringEscapeHandling;

		// Token: 0x04000817 RID: 2071
		private CultureInfo _culture;

		// Token: 0x04000818 RID: 2072
		private int? _maxDepth;

		// Token: 0x04000819 RID: 2073
		private bool _maxDepthSet;

		// Token: 0x0400081A RID: 2074
		private bool? _checkAdditionalContent;

		// Token: 0x0400081B RID: 2075
		[Nullable(2)]
		private string _dateFormatString;

		// Token: 0x0400081C RID: 2076
		private bool _dateFormatStringSet;
	}
}
