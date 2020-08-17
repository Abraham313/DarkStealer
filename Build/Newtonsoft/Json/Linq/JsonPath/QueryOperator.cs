using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020002E2 RID: 738
	internal enum QueryOperator
	{
		// Token: 0x04000C91 RID: 3217
		None,
		// Token: 0x04000C92 RID: 3218
		Equals,
		// Token: 0x04000C93 RID: 3219
		NotEquals,
		// Token: 0x04000C94 RID: 3220
		Exists,
		// Token: 0x04000C95 RID: 3221
		LessThan,
		// Token: 0x04000C96 RID: 3222
		LessThanOrEquals,
		// Token: 0x04000C97 RID: 3223
		GreaterThan,
		// Token: 0x04000C98 RID: 3224
		GreaterThanOrEquals,
		// Token: 0x04000C99 RID: 3225
		And,
		// Token: 0x04000C9A RID: 3226
		Or,
		// Token: 0x04000C9B RID: 3227
		RegexEquals,
		// Token: 0x04000C9C RID: 3228
		StrictEquals,
		// Token: 0x04000C9D RID: 3229
		StrictNotEquals
	}
}
