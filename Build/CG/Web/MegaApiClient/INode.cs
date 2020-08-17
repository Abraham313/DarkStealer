using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200014F RID: 335
	public interface INode : IEquatable<INodeInfo>, INodeInfo
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600091A RID: 2330
		string ParentId { get; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600091B RID: 2331
		DateTime CreationDate { get; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600091C RID: 2332
		string Owner { get; }
	}
}
