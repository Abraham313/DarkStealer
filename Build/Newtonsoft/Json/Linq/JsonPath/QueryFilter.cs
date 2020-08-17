using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E6 RID: 742
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryFilter : PathFilter
	{
		// Token: 0x0600177A RID: 6010 RVA: 0x000170DD File Offset: 0x000152DD
		public QueryFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000170EC File Offset: 0x000152EC
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			QueryFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new QueryFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__root = root;
			<ExecuteFilter>d__.<>3__current = current;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000CA2 RID: 3234
		internal QueryExpression Expression;
	}
}
