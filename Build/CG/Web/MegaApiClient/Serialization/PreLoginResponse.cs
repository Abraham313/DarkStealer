using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000187 RID: 391
	internal class PreLoginResponse
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0000E2FB File Offset: 0x0000C4FB
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0000E303 File Offset: 0x0000C503
		[JsonProperty("s")]
		public string Salt { get; private set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0000E30C File Offset: 0x0000C50C
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0000E314 File Offset: 0x0000C514
		[JsonProperty("v")]
		public int Version { get; private set; }
	}
}
