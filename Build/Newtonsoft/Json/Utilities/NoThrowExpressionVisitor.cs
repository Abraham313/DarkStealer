using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200020F RID: 527
	[NullableContext(1)]
	[Nullable(0)]
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		// Token: 0x06000F48 RID: 3912 RVA: 0x00011A56 File Offset: 0x0000FC56
		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(NoThrowExpressionVisitor.ErrorResult));
			}
			return base.VisitConditional(node);
		}

		// Token: 0x0400099F RID: 2463
		internal static readonly object ErrorResult = new object();
	}
}
