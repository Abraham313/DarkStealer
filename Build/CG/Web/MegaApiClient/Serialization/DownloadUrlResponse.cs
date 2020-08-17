using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200017B RID: 379
	internal class DownloadUrlResponse
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0000E064 File Offset: 0x0000C264
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0000E06C File Offset: 0x0000C26C
		[JsonProperty("g")]
		public string Url { get; private set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0000E075 File Offset: 0x0000C275
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0000E07D File Offset: 0x0000C27D
		[JsonProperty("s")]
		public long Size { get; private set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0000E086 File Offset: 0x0000C286
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0000E08E File Offset: 0x0000C28E
		[JsonProperty("at")]
		public string SerializedAttributes { get; set; }
	}
}
