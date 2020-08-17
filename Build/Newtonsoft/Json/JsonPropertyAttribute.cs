using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001C5 RID: 453
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0000EDC2 File Offset: 0x0000CFC2
		// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x0000EDCA File Offset: 0x0000CFCA
		public Type ItemConverterType { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0000EDD3 File Offset: 0x0000CFD3
		// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0000EDDB File Offset: 0x0000CFDB
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public object[] ItemConverterParameters { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0000EDE4 File Offset: 0x0000CFE4
		// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public Type NamingStrategyType { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0000EDF5 File Offset: 0x0000CFF5
		// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x0000EDFD File Offset: 0x0000CFFD
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public object[] NamingStrategyParameters { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0000EE06 File Offset: 0x0000D006
		// (set) Token: 0x06000BC8 RID: 3016 RVA: 0x0000EE13 File Offset: 0x0000D013
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

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0000EE21 File Offset: 0x0000D021
		// (set) Token: 0x06000BCA RID: 3018 RVA: 0x0000EE2E File Offset: 0x0000D02E
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

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x0000EE3C File Offset: 0x0000D03C
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x0000EE49 File Offset: 0x0000D049
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

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0000EE57 File Offset: 0x0000D057
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0000EE64 File Offset: 0x0000D064
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

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0000EE72 File Offset: 0x0000D072
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x0000EE7F File Offset: 0x0000D07F
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

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0000EE8D File Offset: 0x0000D08D
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x0000EE9A File Offset: 0x0000D09A
		public bool IsReference
		{
			get
			{
				return this._isReference.GetValueOrDefault();
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x0000EEB5 File Offset: 0x0000D0B5
		public int Order
		{
			get
			{
				return this._order.GetValueOrDefault();
			}
			set
			{
				this._order = new int?(value);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0000EEC3 File Offset: 0x0000D0C3
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0000EEDE File Offset: 0x0000D0DE
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
		public string PropertyName { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0000EEEF File Offset: 0x0000D0EF
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return this._itemReferenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0000EF0A File Offset: 0x0000D10A
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x0000EF17 File Offset: 0x0000D117
		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return this._itemTypeNameHandling.GetValueOrDefault();
			}
			set
			{
				this._itemTypeNameHandling = new TypeNameHandling?(value);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0000EF25 File Offset: 0x0000D125
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0000EF32 File Offset: 0x0000D132
		public bool ItemIsReference
		{
			get
			{
				return this._itemIsReference.GetValueOrDefault();
			}
			set
			{
				this._itemIsReference = new bool?(value);
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00009F0E File Offset: 0x0000810E
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0000EF40 File Offset: 0x0000D140
		[NullableContext(1)]
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x040007CC RID: 1996
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x040007CD RID: 1997
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x040007CE RID: 1998
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x040007CF RID: 1999
		internal ObjectCreationHandling? _objectCreationHandling;

		// Token: 0x040007D0 RID: 2000
		internal TypeNameHandling? _typeNameHandling;

		// Token: 0x040007D1 RID: 2001
		internal bool? _isReference;

		// Token: 0x040007D2 RID: 2002
		internal int? _order;

		// Token: 0x040007D3 RID: 2003
		internal Required? _required;

		// Token: 0x040007D4 RID: 2004
		internal bool? _itemIsReference;

		// Token: 0x040007D5 RID: 2005
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x040007D6 RID: 2006
		internal TypeNameHandling? _itemTypeNameHandling;
	}
}
