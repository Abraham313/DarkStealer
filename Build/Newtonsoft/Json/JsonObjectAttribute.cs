using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001C2 RID: 450
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0000ED09 File Offset: 0x0000CF09
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x0000ED11 File Offset: 0x0000CF11
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0000ED1A File Offset: 0x0000CF1A
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x0000ED27 File Offset: 0x0000CF27
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

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0000ED35 File Offset: 0x0000CF35
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0000ED42 File Offset: 0x0000CF42
		public NullValueHandling ItemNullValueHandling
		{
			get
			{
				return this._itemNullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._itemNullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0000ED50 File Offset: 0x0000CF50
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x0000ED5D File Offset: 0x0000CF5D
		public Required ItemRequired
		{
			get
			{
				return this._itemRequired.GetValueOrDefault();
			}
			set
			{
				this._itemRequired = new Required?(value);
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0000E7FB File Offset: 0x0000C9FB
		public JsonObjectAttribute()
		{
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0000ED6B File Offset: 0x0000CF6B
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0000E812 File Offset: 0x0000CA12
		[NullableContext(1)]
		public JsonObjectAttribute(string id) : base(id)
		{
		}

		// Token: 0x040007BE RID: 1982
		private MemberSerialization _memberSerialization;

		// Token: 0x040007BF RID: 1983
		internal MissingMemberHandling? _missingMemberHandling;

		// Token: 0x040007C0 RID: 1984
		internal Required? _itemRequired;

		// Token: 0x040007C1 RID: 1985
		internal NullValueHandling? _itemNullValueHandling;
	}
}
