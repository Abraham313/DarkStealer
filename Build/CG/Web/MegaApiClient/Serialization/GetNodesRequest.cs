using System;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200017D RID: 381
	internal class GetNodesRequest : RequestBase
	{
		// Token: 0x06000A6B RID: 2667 RVA: 0x0000E0C1 File Offset: 0x0000C2C1
		public GetNodesRequest(string shareId = null) : base("f")
		{
			this.c = 1;
			if (shareId != null)
			{
				base.QueryArguments["n"] = shareId;
				this.r = 1;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0000E0F0 File Offset: 0x0000C2F0
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
		public int c { get; private set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0000E101 File Offset: 0x0000C301
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x0000E109 File Offset: 0x0000C309
		public int r { get; private set; }
	}
}
