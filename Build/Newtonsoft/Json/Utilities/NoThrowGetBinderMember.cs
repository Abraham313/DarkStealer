using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200020D RID: 525
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x00011A20 File Offset: 0x0000FC20
		public NoThrowGetBinderMember(GetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0005AA1C File Offset: 0x00058C1C
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x0400099D RID: 2461
		private readonly GetMemberBinder _innerBinder;
	}
}
