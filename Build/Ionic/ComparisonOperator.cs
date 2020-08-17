using System;
using System.ComponentModel;

namespace Ionic
{
	// Token: 0x020000D1 RID: 209
	internal enum ComparisonOperator
	{
		// Token: 0x0400028B RID: 651
		[Description(">")]
		GreaterThan,
		// Token: 0x0400028C RID: 652
		[Description(">=")]
		GreaterThanOrEqualTo,
		// Token: 0x0400028D RID: 653
		[Description("<")]
		LesserThan,
		// Token: 0x0400028E RID: 654
		[Description("<=")]
		LesserThanOrEqualTo,
		// Token: 0x0400028F RID: 655
		[Description("=")]
		EqualTo,
		// Token: 0x04000290 RID: 656
		[Description("!=")]
		NotEqualTo
	}
}
