using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200017C RID: 380
	internal class GetDownloadLinkRequest : RequestBase
	{
		// Token: 0x06000A68 RID: 2664 RVA: 0x0000E097 File Offset: 0x0000C297
		public GetDownloadLinkRequest(INode node) : base("l")
		{
			this.Id = node.Id;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		[JsonProperty("n")]
		public string Id { get; private set; }
	}
}
