using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000178 RID: 376
	internal class DeleteRequest : RequestBase
	{
		// Token: 0x06000A56 RID: 2646 RVA: 0x0000E004 File Offset: 0x0000C204
		public DeleteRequest(INode node) : base("d")
		{
			this.Node = node.Id;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0000E01D File Offset: 0x0000C21D
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0000E025 File Offset: 0x0000C225
		[JsonProperty("n")]
		public string Node { get; private set; }
	}
}
