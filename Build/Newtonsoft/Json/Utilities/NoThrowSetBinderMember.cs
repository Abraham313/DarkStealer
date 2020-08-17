using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200020E RID: 526
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		// Token: 0x06000F46 RID: 3910 RVA: 0x00011A3B File Offset: 0x0000FC3B
		public NoThrowSetBinderMember(SetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0005AA58 File Offset: 0x00058C58
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[]
			{
				value
			});
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x0400099E RID: 2462
		private readonly SetMemberBinder _innerBinder;
	}
}
