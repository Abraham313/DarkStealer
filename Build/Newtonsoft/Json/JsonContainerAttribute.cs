using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x020001B7 RID: 439
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0000E81B File Offset: 0x0000CA1B
		// (set) Token: 0x06000B2C RID: 2860 RVA: 0x0000E823 File Offset: 0x0000CA23
		public string Id { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0000E82C File Offset: 0x0000CA2C
		// (set) Token: 0x06000B2E RID: 2862 RVA: 0x0000E834 File Offset: 0x0000CA34
		public string Title { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0000E83D File Offset: 0x0000CA3D
		// (set) Token: 0x06000B30 RID: 2864 RVA: 0x0000E845 File Offset: 0x0000CA45
		public string Description { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0000E84E File Offset: 0x0000CA4E
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x0000E856 File Offset: 0x0000CA56
		public Type ItemConverterType { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0000E85F File Offset: 0x0000CA5F
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x0000E867 File Offset: 0x0000CA67
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

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0000E870 File Offset: 0x0000CA70
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x0000E878 File Offset: 0x0000CA78
		public Type NamingStrategyType
		{
			get
			{
				return this._namingStrategyType;
			}
			set
			{
				this._namingStrategyType = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0000E888 File Offset: 0x0000CA88
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x0000E890 File Offset: 0x0000CA90
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public object[] NamingStrategyParameters
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this._namingStrategyParameters;
			}
			[param: Nullable(new byte[]
			{
				2,
				1
			})]
			set
			{
				this._namingStrategyParameters = value;
				this.NamingStrategyInstance = null;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0000E8A0 File Offset: 0x0000CAA0
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
		internal NamingStrategy NamingStrategyInstance { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0000E8B1 File Offset: 0x0000CAB1
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x0000E8BE File Offset: 0x0000CABE
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

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0000E8CC File Offset: 0x0000CACC
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x0000E8D9 File Offset: 0x0000CAD9
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0000E8E7 File Offset: 0x0000CAE7
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
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

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0000E902 File Offset: 0x0000CB02
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0000E90F File Offset: 0x0000CB0F
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

		// Token: 0x06000B43 RID: 2883 RVA: 0x00009F0E File Offset: 0x0000810E
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0000E91D File Offset: 0x0000CB1D
		[NullableContext(1)]
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x040007AC RID: 1964
		internal bool? _isReference;

		// Token: 0x040007AD RID: 1965
		internal bool? _itemIsReference;

		// Token: 0x040007AE RID: 1966
		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		// Token: 0x040007AF RID: 1967
		internal TypeNameHandling? _itemTypeNameHandling;

		// Token: 0x040007B0 RID: 1968
		private Type _namingStrategyType;

		// Token: 0x040007B1 RID: 1969
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private object[] _namingStrategyParameters;
	}
}
