using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000186 RID: 390
	internal class PreLoginRequest : RequestBase
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x0000E2D6 File Offset: 0x0000C4D6
		public PreLoginRequest(string userHandle) : base("us0")
		{
			this.UserHandle = userHandle;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0000E2EA File Offset: 0x0000C4EA
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x0000E2F2 File Offset: 0x0000C4F2
		[JsonProperty("user")]
		public string UserHandle { get; private set; }
	}
}
