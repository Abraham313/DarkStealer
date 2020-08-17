using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000153 RID: 339
	[Flags]
	public enum SessionStatus
	{
		// Token: 0x0400069A RID: 1690
		Undefined = 0,
		// Token: 0x0400069B RID: 1691
		Current = 1,
		// Token: 0x0400069C RID: 1692
		Active = 2,
		// Token: 0x0400069D RID: 1693
		Expired = 4
	}
}
