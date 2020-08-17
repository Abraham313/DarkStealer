using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000182 RID: 386
	internal class LoginResponse
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0000E215 File Offset: 0x0000C415
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0000E21D File Offset: 0x0000C41D
		[JsonProperty("csid")]
		public string SessionId { get; private set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0000E226 File Offset: 0x0000C426
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0000E22E File Offset: 0x0000C42E
		[JsonProperty("tsid")]
		public string TemporarySessionId { get; private set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0000E237 File Offset: 0x0000C437
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0000E23F File Offset: 0x0000C43F
		[JsonProperty("privk")]
		public string PrivateKey { get; private set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0000E248 File Offset: 0x0000C448
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0000E250 File Offset: 0x0000C450
		[JsonProperty("k")]
		public string MasterKey { get; private set; }
	}
}
