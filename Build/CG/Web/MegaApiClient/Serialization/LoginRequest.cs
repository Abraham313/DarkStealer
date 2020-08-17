using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000181 RID: 385
	internal class LoginRequest : RequestBase
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0000E1A5 File Offset: 0x0000C3A5
		public LoginRequest(string userHandle, string passwordHash) : base("us")
		{
			this.UserHandle = userHandle;
			this.PasswordHash = passwordHash;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0000E1C0 File Offset: 0x0000C3C0
		public LoginRequest(string userHandle, string passwordHash, string mfaKey) : base("us")
		{
			this.UserHandle = userHandle;
			this.PasswordHash = passwordHash;
			this.MFAKey = mfaKey;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0000E1E2 File Offset: 0x0000C3E2
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x0000E1EA File Offset: 0x0000C3EA
		[JsonProperty("user")]
		public string UserHandle { get; private set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0000E1F3 File Offset: 0x0000C3F3
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x0000E1FB File Offset: 0x0000C3FB
		[JsonProperty("uh")]
		public string PasswordHash { get; private set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0000E204 File Offset: 0x0000C404
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0000E20C File Offset: 0x0000C40C
		[JsonProperty("mfa")]
		public string MFAKey { get; private set; }
	}
}
