using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000176 RID: 374
	internal class CreateNodeRequest : RequestBase
	{
		// Token: 0x06000A44 RID: 2628 RVA: 0x0004D9E8 File Offset: 0x0004BBE8
		private CreateNodeRequest(INode parentNode, NodeType type, string attributes, string encryptedKey, byte[] key, string completionHandle) : base("p")
		{
			this.ParentId = parentNode.Id;
			this.Nodes = new CreateNodeRequest.CreateNodeRequestData[]
			{
				new CreateNodeRequest.CreateNodeRequestData
				{
					Attributes = attributes,
					Key = encryptedKey,
					Type = type,
					CompletionHandle = completionHandle
				}
			};
			INodeCrypto nodeCrypto = parentNode as INodeCrypto;
			if (nodeCrypto == null)
			{
				throw new ArgumentException("parentNode node must implement INodeCrypto");
			}
			if (nodeCrypto.SharedKey != null)
			{
				this.Share = new ShareData(parentNode.Id);
				this.Share.AddItem(completionHandle, key, nodeCrypto.SharedKey);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0000DF6E File Offset: 0x0000C16E
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0000DF76 File Offset: 0x0000C176
		[JsonProperty("t")]
		public string ParentId { get; private set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0000DF7F File Offset: 0x0000C17F
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0000DF87 File Offset: 0x0000C187
		[JsonProperty("cr")]
		public ShareData Share { get; private set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0000DF90 File Offset: 0x0000C190
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0000DF98 File Offset: 0x0000C198
		[JsonProperty("n")]
		public CreateNodeRequest.CreateNodeRequestData[] Nodes { get; private set; }

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000DFA1 File Offset: 0x0000C1A1
		public static CreateNodeRequest CreateFileNodeRequest(INode parentNode, string attributes, string encryptedkey, byte[] fileKey, string completionHandle)
		{
			return new CreateNodeRequest(parentNode, NodeType.File, attributes, encryptedkey, fileKey, completionHandle);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000DFAF File Offset: 0x0000C1AF
		public static CreateNodeRequest CreateFolderNodeRequest(INode parentNode, string attributes, string encryptedkey, byte[] key)
		{
			return new CreateNodeRequest(parentNode, NodeType.Directory, attributes, encryptedkey, key, "xxxxxxxx");
		}

		// Token: 0x02000177 RID: 375
		internal class CreateNodeRequestData
		{
			// Token: 0x17000213 RID: 531
			// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
			// (set) Token: 0x06000A4E RID: 2638 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
			[JsonProperty("h")]
			public string CompletionHandle { get; set; }

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0000DFD1 File Offset: 0x0000C1D1
			// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0000DFD9 File Offset: 0x0000C1D9
			[JsonProperty("t")]
			public NodeType Type { get; set; }

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0000DFE2 File Offset: 0x0000C1E2
			// (set) Token: 0x06000A52 RID: 2642 RVA: 0x0000DFEA File Offset: 0x0000C1EA
			[JsonProperty("a")]
			public string Attributes { get; set; }

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0000DFF3 File Offset: 0x0000C1F3
			// (set) Token: 0x06000A54 RID: 2644 RVA: 0x0000DFFB File Offset: 0x0000C1FB
			[JsonProperty("k")]
			public string Key { get; set; }
		}
	}
}
