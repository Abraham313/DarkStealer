using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200018A RID: 394
	internal class SessionHistoryRequest : RequestBase
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0000E392 File Offset: 0x0000C592
		public SessionHistoryRequest() : base("usl")
		{
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00009F16 File Offset: 0x00008116
		[JsonProperty("x")]
		public int LoadSessionIds
		{
			get
			{
				return 1;
			}
		}
	}
}
