using System;
using System.Collections.Generic;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200014B RID: 331
	public interface IAccountInformation
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008F0 RID: 2288
		long TotalQuota { get; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008F1 RID: 2289
		long UsedQuota { get; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008F2 RID: 2290
		IEnumerable<IStorageMetrics> Metrics { get; }
	}
}
