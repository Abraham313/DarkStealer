using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E4 RID: 740
	[NullableContext(1)]
	[Nullable(0)]
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x000170A1 File Offset: 0x000152A1
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x000170A9 File Offset: 0x000152A9
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06001771 RID: 6001 RVA: 0x000170B2 File Offset: 0x000152B2
		public CompositeExpression(QueryOperator @operator) : base(@operator)
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000722EC File Offset: 0x000704EC
		public override bool IsMatch(JToken root, JToken t)
		{
			QueryOperator @operator = this.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
