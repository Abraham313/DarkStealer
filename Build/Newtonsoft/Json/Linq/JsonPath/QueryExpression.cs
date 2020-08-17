using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E3 RID: 739
	internal abstract class QueryExpression
	{
		// Token: 0x0600176D RID: 5997 RVA: 0x00017092 File Offset: 0x00015292
		public QueryExpression(QueryOperator @operator)
		{
			this.Operator = @operator;
		}

		// Token: 0x0600176E RID: 5998
		[NullableContext(1)]
		public abstract bool IsMatch(JToken root, JToken t);

		// Token: 0x04000C9E RID: 3230
		internal QueryOperator Operator;
	}
}
