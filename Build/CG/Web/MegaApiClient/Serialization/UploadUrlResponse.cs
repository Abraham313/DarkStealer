using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000197 RID: 407
	internal class UploadUrlResponse
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0000E5F9 File Offset: 0x0000C7F9
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0000E601 File Offset: 0x0000C801
		[JsonProperty("p")]
		public string Url { get; private set; }
	}
}
