using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000273 RID: 627
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x000130F0 File Offset: 0x000112F0
		// (set) Token: 0x060011A5 RID: 4517 RVA: 0x000130F8 File Offset: 0x000112F8
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00013101 File Offset: 0x00011301
		// (set) Token: 0x060011A7 RID: 4519 RVA: 0x00013109 File Offset: 0x00011309
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x00013112 File Offset: 0x00011312
		// (set) Token: 0x060011A9 RID: 4521 RVA: 0x0001311A File Offset: 0x0001131A
		public Required? ItemRequired { get; set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x00013123 File Offset: 0x00011323
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x0001312B File Offset: 0x0001132B
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00013134 File Offset: 0x00011334
		[Nullable(1)]
		public JsonPropertyCollection Properties { [NullableContext(1)] get; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x0001313C File Offset: 0x0001133C
		[Nullable(1)]
		public JsonPropertyCollection CreatorParameters
		{
			[NullableContext(1)]
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x0001315D File Offset: 0x0001135D
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x00013165 File Offset: 0x00011365
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[]
			{
				2,
				1
			})]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0001316E File Offset: 0x0001136E
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x00013176 File Offset: 0x00011376
		[Nullable(new byte[]
		{
			2,
			1
		})]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this._parameterizedCreator;
			}
			[param: Nullable(new byte[]
			{
				2,
				1
			})]
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0001317F File Offset: 0x0001137F
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x00013187 File Offset: 0x00011387
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00013190 File Offset: 0x00011390
		// (set) Token: 0x060011B5 RID: 4533 RVA: 0x00013198 File Offset: 0x00011398
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x000131A1 File Offset: 0x000113A1
		// (set) Token: 0x060011B7 RID: 4535 RVA: 0x000131A9 File Offset: 0x000113A9
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = (value != null && typeof(JToken).IsAssignableFrom(value));
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x000131D4 File Offset: 0x000113D4
		// (set) Token: 0x060011B9 RID: 4537 RVA: 0x000131DC File Offset: 0x000113DC
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Func<string, string> ExtensionDataNameResolver { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x0006169C File Offset: 0x0005F89C
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								if (!(defaultValueHandling.GetValueOrDefault() == DefaultValueHandling.Populate & defaultValueHandling != null))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x000131E5 File Offset: 0x000113E5
		[NullableContext(1)]
		public JsonObjectContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00013206 File Offset: 0x00011406
		[NullableContext(1)]
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x04000AAD RID: 2733
		internal bool ExtensionDataIsJToken;

		// Token: 0x04000AAE RID: 2734
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x04000AAF RID: 2735
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000AB0 RID: 2736
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000AB1 RID: 2737
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x04000AB2 RID: 2738
		private Type _extensionDataValueType;
	}
}
