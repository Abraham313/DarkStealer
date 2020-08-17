using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000150 RID: 336
	internal interface INodeCrypto
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600091D RID: 2333
		byte[] Key { get; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600091E RID: 2334
		byte[] SharedKey { get; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600091F RID: 2335
		byte[] Iv { get; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000920 RID: 2336
		byte[] MetaMac { get; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000921 RID: 2337
		byte[] FullKey { get; }
	}
}
