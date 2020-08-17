using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200018E RID: 398
	[JsonConverter(typeof(ShareDataConverter))]
	internal class ShareData
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0000E438 File Offset: 0x0000C638
		public ShareData(string nodeId)
		{
			this.NodeId = nodeId;
			this.items = new List<ShareData.ShareDataItem>();
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0000E452 File Offset: 0x0000C652
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x0000E45A File Offset: 0x0000C65A
		public string NodeId { get; private set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0000E463 File Offset: 0x0000C663
		public IEnumerable<ShareData.ShareDataItem> Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0004DE14 File Offset: 0x0004C014
		public void AddItem(string nodeId, byte[] data, byte[] key)
		{
			ShareData.ShareDataItem item = new ShareData.ShareDataItem
			{
				NodeId = nodeId,
				Data = data,
				Key = key
			};
			this.items.Add(item);
		}

		// Token: 0x0400074F RID: 1871
		private IList<ShareData.ShareDataItem> items;

		// Token: 0x0200018F RID: 399
		public class ShareDataItem
		{
			// Token: 0x17000240 RID: 576
			// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0000E46B File Offset: 0x0000C66B
			// (set) Token: 0x06000ACA RID: 2762 RVA: 0x0000E473 File Offset: 0x0000C673
			public string NodeId { get; set; }

			// Token: 0x17000241 RID: 577
			// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0000E47C File Offset: 0x0000C67C
			// (set) Token: 0x06000ACC RID: 2764 RVA: 0x0000E484 File Offset: 0x0000C684
			public byte[] Data { get; set; }

			// Token: 0x17000242 RID: 578
			// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0000E48D File Offset: 0x0000C68D
			// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0000E495 File Offset: 0x0000C695
			public byte[] Key { get; set; }
		}
	}
}
