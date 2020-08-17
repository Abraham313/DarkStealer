using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000174 RID: 372
	internal class AnonymousLoginRequest : RequestBase
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		public AnonymousLoginRequest(string masterKey, string temporarySession) : base("up")
		{
			this.MasterKey = masterKey;
			this.TemporarySession = temporarySession;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0000DEEF File Offset: 0x0000C0EF
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x0000DEF7 File Offset: 0x0000C0F7
		[JsonProperty("k")]
		public string MasterKey { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0000DF00 File Offset: 0x0000C100
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0000DF08 File Offset: 0x0000C108
		[JsonProperty("ts")]
		public string TemporarySession { get; set; }
	}
}
