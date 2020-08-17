using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000189 RID: 393
	internal abstract class RequestBase
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0000E35F File Offset: 0x0000C55F
		protected RequestBase(string action)
		{
			this.Action = action;
			this.QueryArguments = new Dictionary<string, string>();
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0000E379 File Offset: 0x0000C579
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x0000E381 File Offset: 0x0000C581
		[JsonProperty("a")]
		public string Action { get; private set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0000E38A File Offset: 0x0000C58A
		[JsonIgnore]
		public Dictionary<string, string> QueryArguments { get; }
	}
}
