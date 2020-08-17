using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200014C RID: 332
	public interface IStorageMetrics
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008F3 RID: 2291
		string NodeId { get; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008F4 RID: 2292
		long BytesUsed { get; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008F5 RID: 2293
		long FilesCount { get; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008F6 RID: 2294
		long FoldersCount { get; }
	}
}
