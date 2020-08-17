using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000184 RID: 388
	internal class MoveRequest : RequestBase
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x0000E266 File Offset: 0x0000C466
		public MoveRequest(INode node, INode destinationParentNode) : base("m")
		{
			this.Id = node.Id;
			this.DestinationParentId = destinationParentNode.Id;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0000E28B File Offset: 0x0000C48B
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x0000E293 File Offset: 0x0000C493
		[JsonProperty("n")]
		public string Id { get; private set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0000E29C File Offset: 0x0000C49C
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x0000E2A4 File Offset: 0x0000C4A4
		[JsonProperty("t")]
		public string DestinationParentId { get; private set; }
	}
}
