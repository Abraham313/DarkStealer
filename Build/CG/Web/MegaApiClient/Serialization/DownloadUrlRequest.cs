using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000179 RID: 377
	internal class DownloadUrlRequest : RequestBase
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x0004DA84 File Offset: 0x0004BC84
		public DownloadUrlRequest(INode node) : base("g")
		{
			this.Id = node.Id;
			PublicNode publicNode = node as PublicNode;
			if (publicNode != null)
			{
				base.QueryArguments["n"] = publicNode.ShareId;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00009F16 File Offset: 0x00008116
		public int g
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0000E02E File Offset: 0x0000C22E
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0000E036 File Offset: 0x0000C236
		[JsonProperty("n")]
		public string Id { get; private set; }
	}
}
