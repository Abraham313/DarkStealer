using System;
using System.Linq;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000165 RID: 357
	public class Options
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x0000DB2A File Offset: 0x0000BD2A
		public Options(string applicationKey = "axhQiYyQ", bool synchronizeApiRequests = true, Options.ComputeApiRequestRetryWaitDelayDelegate computeApiRequestRetryWaitDelay = null, int bufferSize = 65536, int chunksPackSize = 1048576)
		{
			this.ApplicationKey = applicationKey;
			this.SynchronizeApiRequests = synchronizeApiRequests;
			this.ComputeApiRequestRetryWaitDelay = (computeApiRequestRetryWaitDelay ?? new Options.ComputeApiRequestRetryWaitDelayDelegate(this.ComputeDefaultApiRequestRetryWaitDelay));
			this.BufferSize = bufferSize;
			this.ChunksPackSize = chunksPackSize;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0000DB67 File Offset: 0x0000BD67
		public string ApplicationKey { get; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0000DB6F File Offset: 0x0000BD6F
		public bool SynchronizeApiRequests { get; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0000DB77 File Offset: 0x0000BD77
		public Options.ComputeApiRequestRetryWaitDelayDelegate ComputeApiRequestRetryWaitDelay { get; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0000DB7F File Offset: 0x0000BD7F
		public int BufferSize { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x0000DB87 File Offset: 0x0000BD87
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x0000DB8F File Offset: 0x0000BD8F
		public int ChunksPackSize { get; internal set; }

		// Token: 0x060009CF RID: 2511 RVA: 0x0004CD48 File Offset: 0x0004AF48
		private bool ComputeDefaultApiRequestRetryWaitDelay(int attempt, out TimeSpan delay)
		{
			if (attempt > 17)
			{
				delay = default(TimeSpan);
				return false;
			}
			int num = Enumerable.Range(0, attempt).Aggregate(0, (int current, int item) => (int)((current == 0) ? 100f : ((float)current * 1.5f)));
			delay = TimeSpan.FromMilliseconds((double)num);
			return true;
		}

		// Token: 0x040006D7 RID: 1751
		public const string DefaultApplicationKey = "axhQiYyQ";

		// Token: 0x040006D8 RID: 1752
		public const bool DefaultSynchronizeApiRequests = true;

		// Token: 0x040006D9 RID: 1753
		public const int DefaultApiRequestAttempts = 17;

		// Token: 0x040006DA RID: 1754
		public const int DefaultApiRequestDelay = 100;

		// Token: 0x040006DB RID: 1755
		public const float DefaultApiRequestDelayFactor = 1.5f;

		// Token: 0x040006DC RID: 1756
		public const int DefaultBufferSize = 65536;

		// Token: 0x040006DD RID: 1757
		public const int DefaultChunksPackSize = 1048576;

		// Token: 0x02000166 RID: 358
		// (Invoke) Token: 0x060009D1 RID: 2513
		public delegate bool ComputeApiRequestRetryWaitDelayDelegate(int attempt, out TimeSpan delay);
	}
}
