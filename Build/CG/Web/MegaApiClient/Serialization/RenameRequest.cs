using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000188 RID: 392
	internal class RenameRequest : RequestBase
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x0000E31D File Offset: 0x0000C51D
		public RenameRequest(INode node, string attributes) : base("a")
		{
			this.Id = node.Id;
			this.SerializedAttributes = attributes;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0000E33D File Offset: 0x0000C53D
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0000E345 File Offset: 0x0000C545
		[JsonProperty("n")]
		public string Id { get; private set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0000E34E File Offset: 0x0000C54E
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0000E356 File Offset: 0x0000C556
		[JsonProperty("attr")]
		public string SerializedAttributes { get; set; }
	}
}
