using System;
using System.Net;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000152 RID: 338
	public interface ISession
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000922 RID: 2338
		string Client { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000923 RID: 2339
		IPAddress IpAddress { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000924 RID: 2340
		string Country { get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000925 RID: 2341
		DateTime LoginTime { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000926 RID: 2342
		DateTime LastSeenTime { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000927 RID: 2343
		SessionStatus Status { get; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000928 RID: 2344
		string SessionId { get; }
	}
}
