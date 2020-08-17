using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200017A RID: 378
	internal class DownloadUrlRequestFromId : RequestBase
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x0000E03F File Offset: 0x0000C23F
		public DownloadUrlRequestFromId(string id) : base("g")
		{
			this.Id = id;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00009F16 File Offset: 0x00008116
		public int g
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0000E053 File Offset: 0x0000C253
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0000E05B File Offset: 0x0000C25B
		[JsonProperty("p")]
		public string Id { get; private set; }
	}
}
