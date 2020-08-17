using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	// Token: 0x020001B5 RID: 437
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0000E7EA File Offset: 0x0000C9EA
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x0000E7F2 File Offset: 0x0000C9F2
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0000E7FB File Offset: 0x0000C9FB
		public JsonArrayAttribute()
		{
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0000E803 File Offset: 0x0000CA03
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0000E812 File Offset: 0x0000CA12
		[NullableContext(1)]
		public JsonArrayAttribute(string id) : base(id)
		{
		}

		// Token: 0x040007A5 RID: 1957
		private bool _allowNullItems;
	}
}
