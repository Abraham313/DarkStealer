using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000171 RID: 369
	internal class AccountInformationResponse : IAccountInformation
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0000DDF3 File Offset: 0x0000BFF3
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0000DDFB File Offset: 0x0000BFFB
		[JsonProperty("mstrg")]
		public long TotalQuota { get; private set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0000DE04 File Offset: 0x0000C004
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x0000DE0C File Offset: 0x0000C00C
		[JsonProperty("cstrg")]
		public long UsedQuota { get; private set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0000DE15 File Offset: 0x0000C015
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x0000DE1D File Offset: 0x0000C01D
		[JsonProperty("cstrgn")]
		private Dictionary<string, long[]> MetricsSerialized { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0000DE26 File Offset: 0x0000C026
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x0000DE2E File Offset: 0x0000C02E
		public IEnumerable<IStorageMetrics> Metrics { get; private set; }

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000DE37 File Offset: 0x0000C037
		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			this.Metrics = from x in this.MetricsSerialized
			select new AccountInformationResponse.StorageMetrics(x.Key, x.Value);
		}

		// Token: 0x02000172 RID: 370
		private class StorageMetrics : IStorageMetrics
		{
			// Token: 0x06000A2B RID: 2603 RVA: 0x0000DE69 File Offset: 0x0000C069
			public StorageMetrics(string nodeId, long[] metrics)
			{
				this.NodeId = nodeId;
				this.BytesUsed = metrics[0];
				this.FilesCount = metrics[1];
				this.FoldersCount = metrics[2];
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0000DE93 File Offset: 0x0000C093
			public string NodeId { get; }

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0000DE9B File Offset: 0x0000C09B
			public long BytesUsed { get; }

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0000DEA3 File Offset: 0x0000C0A3
			public long FilesCount { get; }

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0000DEAB File Offset: 0x0000C0AB
			public long FoldersCount { get; }
		}
	}
}
