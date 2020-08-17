using System;
using System.Diagnostics;
using CG.Web.MegaApiClient.Serialization;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200015F RID: 351
	[DebuggerDisplay("NodeInfo - Type: {Type} - Name: {Name} - Id: {Id}")]
	internal class NodeInfo : IEquatable<INodeInfo>, INodeInfo
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x00008754 File Offset: 0x00006954
		protected NodeInfo()
		{
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0000D7DB File Offset: 0x0000B9DB
		internal NodeInfo(string id, DownloadUrlResponse downloadResponse, byte[] key)
		{
			this.Id = id;
			this.Attributes = Crypto.DecryptAttributes(downloadResponse.SerializedAttributes.FromBase64(), key);
			this.Size = downloadResponse.Size;
			this.Type = NodeType.File;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0000D814 File Offset: 0x0000BA14
		[JsonIgnore]
		public string Name
		{
			get
			{
				Attributes attributes = this.Attributes;
				if (attributes == null)
				{
					return null;
				}
				return attributes.Name;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0000D827 File Offset: 0x0000BA27
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0000D82F File Offset: 0x0000BA2F
		[JsonProperty("s")]
		public long Size { get; protected set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0000D838 File Offset: 0x0000BA38
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0000D840 File Offset: 0x0000BA40
		[JsonProperty("t")]
		public NodeType Type { get; protected set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0000D849 File Offset: 0x0000BA49
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0000D851 File Offset: 0x0000BA51
		[JsonProperty("h")]
		public string Id { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0004CA60 File Offset: 0x0004AC60
		[JsonIgnore]
		public DateTime? ModificationDate
		{
			get
			{
				Attributes attributes = this.Attributes;
				if (attributes == null)
				{
					return null;
				}
				return attributes.ModificationDate;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0000D85A File Offset: 0x0000BA5A
		[JsonIgnore]
		public string SerializedFingerprint
		{
			get
			{
				Attributes attributes = this.Attributes;
				if (attributes == null)
				{
					return null;
				}
				return attributes.SerializedFingerprint;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0000D86D File Offset: 0x0000BA6D
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x0000D875 File Offset: 0x0000BA75
		[JsonIgnore]
		public Attributes Attributes { get; protected set; }

		// Token: 0x0600098E RID: 2446 RVA: 0x0000D87E File Offset: 0x0000BA7E
		public bool Equals(INodeInfo other)
		{
			return other != null && this.Id == other.Id;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0000D896 File Offset: 0x0000BA96
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000D8A3 File Offset: 0x0000BAA3
		public override bool Equals(object obj)
		{
			return this.Equals(obj as INodeInfo);
		}
	}
}
