using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200014E RID: 334
	public interface INodeInfo : IEquatable<INodeInfo>
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000914 RID: 2324
		string Id { get; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000915 RID: 2325
		NodeType Type { get; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000916 RID: 2326
		string Name { get; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000917 RID: 2327
		long Size { get; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000918 RID: 2328
		DateTime? ModificationDate { get; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000919 RID: 2329
		string SerializedFingerprint { get; }
	}
}
