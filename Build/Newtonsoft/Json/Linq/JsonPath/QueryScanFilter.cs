using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E8 RID: 744
	[NullableContext(1)]
	[Nullable(0)]
	internal class QueryScanFilter : PathFilter
	{
		// Token: 0x06001786 RID: 6022 RVA: 0x00017172 File Offset: 0x00015372
		public QueryScanFilter(QueryExpression expression)
		{
			this.Expression = expression;
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00017181 File Offset: 0x00015381
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			QueryScanFilter.<ExecuteFilter>d__2 <ExecuteFilter>d__ = new QueryScanFilter.<ExecuteFilter>d__2(-2);
			<ExecuteFilter>d__.<>4__this = this;
			<ExecuteFilter>d__.<>3__root = root;
			<ExecuteFilter>d__.<>3__current = current;
			return <ExecuteFilter>d__;
		}

		// Token: 0x04000CAD RID: 3245
		internal QueryExpression Expression;
	}
}
