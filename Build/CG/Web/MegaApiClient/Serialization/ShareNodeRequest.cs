using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000192 RID: 402
	internal class ShareNodeRequest : RequestBase
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x0004DF58 File Offset: 0x0004C158
		public ShareNodeRequest(INode node, byte[] masterKey, IEnumerable<INode> nodes) : base("s2")
		{
			this.Id = node.Id;
			this.Options = new object[]
			{
				new
				{
					r = 0,
					u = "EXP"
				}
			};
			INodeCrypto nodeCrypto = (INodeCrypto)node;
			byte[] array = nodeCrypto.SharedKey;
			if (array == null)
			{
				array = Crypto.CreateAesKey();
			}
			this.SharedKey = Crypto.EncryptKey(array, masterKey).ToBase64();
			if (nodeCrypto.SharedKey == null)
			{
				this.Share = new ShareData(node.Id);
				this.Share.AddItem(node.Id, nodeCrypto.FullKey, array);
				foreach (INode node2 in this.GetRecursiveChildren(nodes.ToArray<INode>(), node))
				{
					this.Share.AddItem(node2.Id, ((INodeCrypto)node2).FullKey, array);
				}
			}
			byte[] data = (node.Id + node.Id).ToBytes();
			this.HandleAuth = Crypto.EncryptKey(data, masterKey).ToBase64();
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0000E4E8 File Offset: 0x0000C6E8
		private IEnumerable<INode> GetRecursiveChildren(INode[] nodes, INode parent)
		{
			ShareNodeRequest.<GetRecursiveChildren>d__1 <GetRecursiveChildren>d__ = new ShareNodeRequest.<GetRecursiveChildren>d__1(-2);
			<GetRecursiveChildren>d__.<>3__nodes = nodes;
			<GetRecursiveChildren>d__.<>3__parent = parent;
			return <GetRecursiveChildren>d__;
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0000E4FF File Offset: 0x0000C6FF
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x0000E507 File Offset: 0x0000C707
		[JsonProperty("n")]
		public string Id { get; private set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0000E510 File Offset: 0x0000C710
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x0000E518 File Offset: 0x0000C718
		[JsonProperty("ha")]
		public string HandleAuth { get; private set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0000E521 File Offset: 0x0000C721
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x0000E529 File Offset: 0x0000C729
		[JsonProperty("s")]
		public object[] Options { get; private set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0000E532 File Offset: 0x0000C732
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x0000E53A File Offset: 0x0000C73A
		[JsonProperty("cr")]
		public ShareData Share { get; private set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0000E543 File Offset: 0x0000C743
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0000E54B File Offset: 0x0000C74B
		[JsonProperty("ok")]
		public string SharedKey { get; private set; }
	}
}
