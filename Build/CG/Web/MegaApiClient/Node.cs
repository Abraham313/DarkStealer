using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using CG.Web.MegaApiClient.Serialization;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000160 RID: 352
	[DebuggerDisplay("Node - Type: {Type} - Name: {Name} - Id: {Id}")]
	internal class Node : NodeInfo, IEquatable<INodeInfo>, INodeInfo, INode, INodeCrypto
	{
		// Token: 0x06000991 RID: 2449 RVA: 0x0000D8B1 File Offset: 0x0000BAB1
		public Node(byte[] masterKey, ref List<SharedKey> sharedKeys)
		{
			this.masterKey = masterKey;
			this.sharedKeys = sharedKeys;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0000D8D0 File Offset: 0x0000BAD0
		[JsonProperty("p")]
		public string ParentId { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0000D8D9 File Offset: 0x0000BAD9
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0000D8E1 File Offset: 0x0000BAE1
		[JsonProperty("u")]
		public string Owner { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0000D8EA File Offset: 0x0000BAEA
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0000D8F2 File Offset: 0x0000BAF2
		[JsonProperty("su")]
		public string SharingId { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0000D8FB File Offset: 0x0000BAFB
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0000D903 File Offset: 0x0000BB03
		[JsonProperty("sk")]
		public string SharingKey { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0000D90C File Offset: 0x0000BB0C
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0000D914 File Offset: 0x0000BB14
		[JsonIgnore]
		public DateTime CreationDate { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0000D91D File Offset: 0x0000BB1D
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0000D925 File Offset: 0x0000BB25
		[JsonIgnore]
		public byte[] Key { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0000D92E File Offset: 0x0000BB2E
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0000D936 File Offset: 0x0000BB36
		[JsonIgnore]
		public byte[] FullKey { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0000D93F File Offset: 0x0000BB3F
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0000D947 File Offset: 0x0000BB47
		[JsonIgnore]
		public byte[] SharedKey { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0000D950 File Offset: 0x0000BB50
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0000D958 File Offset: 0x0000BB58
		[JsonIgnore]
		public byte[] Iv { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0000D961 File Offset: 0x0000BB61
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0000D969 File Offset: 0x0000BB69
		[JsonIgnore]
		public byte[] MetaMac { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0000D972 File Offset: 0x0000BB72
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0000D97A File Offset: 0x0000BB7A
		[JsonIgnore]
		public bool EmptyKey { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0000D983 File Offset: 0x0000BB83
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0000D98B File Offset: 0x0000BB8B
		[JsonProperty("ts")]
		private long SerializedCreationDate { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0000D994 File Offset: 0x0000BB94
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x0000D99C File Offset: 0x0000BB9C
		[JsonProperty("a")]
		private string SerializedAttributes { get; set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0000D9A5 File Offset: 0x0000BBA5
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0000D9AD File Offset: 0x0000BBAD
		[JsonProperty("k")]
		private string SerializedKey { get; set; }

		// Token: 0x060009AE RID: 2478 RVA: 0x0004CA88 File Offset: 0x0004AC88
		[OnDeserialized]
		public void OnDeserialized(StreamingContext ctx)
		{
			if (this.SharingKey != null && !this.sharedKeys.Any((SharedKey x) => x.Id == base.Id))
			{
				this.sharedKeys.Add(new SharedKey(base.Id, this.SharingKey));
			}
			this.CreationDate = this.SerializedCreationDate.ToDateTime();
			if (base.Type == NodeType.File || base.Type == NodeType.Directory)
			{
				if (string.IsNullOrEmpty(this.SerializedKey))
				{
					this.EmptyKey = true;
					return;
				}
				string text = this.SerializedKey.Split(new char[]
				{
					'/'
				})[0];
				int num = text.IndexOf(":", StringComparison.Ordinal);
				byte[] data = text.Substring(num + 1).FromBase64();
				if (this.sharedKeys != null)
				{
					string handle = text.Substring(0, num);
					SharedKey sharedKey = this.sharedKeys.FirstOrDefault((SharedKey x) => x.Id == handle);
					if (sharedKey != null)
					{
						this.masterKey = Crypto.DecryptKey(sharedKey.Key.FromBase64(), this.masterKey);
						if (base.Type == NodeType.Directory)
						{
							this.SharedKey = this.masterKey;
						}
						else
						{
							this.SharedKey = Crypto.DecryptKey(data, this.masterKey);
						}
					}
				}
				this.FullKey = Crypto.DecryptKey(data, this.masterKey);
				if (base.Type == NodeType.File)
				{
					byte[] iv;
					byte[] metaMac;
					byte[] key;
					Crypto.GetPartsFromDecryptedKey(this.FullKey, out iv, out metaMac, out key);
					this.Iv = iv;
					this.MetaMac = metaMac;
					this.Key = key;
				}
				else
				{
					this.Key = this.FullKey;
				}
				base.Attributes = Crypto.DecryptAttributes(this.SerializedAttributes.FromBase64(), this.Key);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0004CC34 File Offset: 0x0004AE34
		public bool IsShareRoot
		{
			get
			{
				string text = this.SerializedKey.Split(new char[]
				{
					'/'
				})[0];
				int length = text.IndexOf(":", StringComparison.Ordinal);
				return text.Substring(0, length) == base.Id;
			}
		}

		// Token: 0x040006C3 RID: 1731
		private byte[] masterKey;

		// Token: 0x040006C4 RID: 1732
		private List<SharedKey> sharedKeys;
	}
}
