using System;
using System.Diagnostics;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000162 RID: 354
	[DebuggerDisplay("PublicNode - Type: {Type} - Name: {Name} - Id: {Id}")]
	internal class PublicNode : IEquatable<INodeInfo>, INodeInfo, INode, INodeCrypto
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		internal PublicNode(Node node, string shareId)
		{
			this.node = node;
			this.ShareId = shareId;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0000D9F2 File Offset: 0x0000BBF2
		public string ShareId { get; }

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000D9FA File Offset: 0x0000BBFA
		public bool Equals(INodeInfo other)
		{
			if (this.node.Equals(other))
			{
				string shareId = this.ShareId;
				PublicNode publicNode = other as PublicNode;
				return shareId == ((publicNode != null) ? publicNode.ShareId : null);
			}
			return false;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0000DA29 File Offset: 0x0000BC29
		public long Size
		{
			get
			{
				return this.node.Size;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0000DA36 File Offset: 0x0000BC36
		public string Name
		{
			get
			{
				return this.node.Name;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0000DA43 File Offset: 0x0000BC43
		public DateTime? ModificationDate
		{
			get
			{
				return this.node.ModificationDate;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0000DA50 File Offset: 0x0000BC50
		public string SerializedFingerprint
		{
			get
			{
				return this.node.Attributes.SerializedFingerprint;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0000DA62 File Offset: 0x0000BC62
		public string Id
		{
			get
			{
				return this.node.Id;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0000DA6F File Offset: 0x0000BC6F
		public string ParentId
		{
			get
			{
				if (!this.node.IsShareRoot)
				{
					return this.node.ParentId;
				}
				return null;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0000DA8B File Offset: 0x0000BC8B
		public string Owner
		{
			get
			{
				return this.node.Owner;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public NodeType Type
		{
			get
			{
				if (this.node.IsShareRoot)
				{
					if (this.node.Type == NodeType.Directory)
					{
						return NodeType.Root;
					}
				}
				return this.node.Type;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		public DateTime CreationDate
		{
			get
			{
				return this.node.CreationDate;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0000DAD1 File Offset: 0x0000BCD1
		public byte[] Key
		{
			get
			{
				return this.node.Key;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0000DADE File Offset: 0x0000BCDE
		public byte[] SharedKey
		{
			get
			{
				return this.node.SharedKey;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0000DAEB File Offset: 0x0000BCEB
		public byte[] Iv
		{
			get
			{
				return this.node.Iv;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		public byte[] MetaMac
		{
			get
			{
				return this.node.MetaMac;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0000DB05 File Offset: 0x0000BD05
		public byte[] FullKey
		{
			get
			{
				return this.node.FullKey;
			}
		}

		// Token: 0x040006D4 RID: 1748
		private readonly Node node;
	}
}
