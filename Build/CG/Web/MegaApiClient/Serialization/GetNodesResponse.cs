using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x0200017E RID: 382
	internal class GetNodesResponse
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x0000E112 File Offset: 0x0000C312
		public GetNodesResponse(byte[] masterKey)
		{
			this.masterKey = masterKey;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0000E121 File Offset: 0x0000C321
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0000E129 File Offset: 0x0000C329
		public Node[] Nodes { get; private set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0000E132 File Offset: 0x0000C332
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0000E13A File Offset: 0x0000C33A
		public Node[] UndecryptedNodes { get; private set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0000E143 File Offset: 0x0000C343
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0000E14B File Offset: 0x0000C34B
		[JsonProperty("f")]
		public JRaw NodesSerialized { get; private set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0000E154 File Offset: 0x0000C354
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0000E15C File Offset: 0x0000C35C
		[JsonProperty("ok")]
		public List<SharedKey> SharedKeys
		{
			get
			{
				return this.sharedKeys;
			}
			private set
			{
				this.sharedKeys = value;
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004DAC8 File Offset: 0x0004BCC8
		[OnDeserialized]
		public void OnDeserialized(StreamingContext ctx)
		{
			Node[] source = JsonConvert.DeserializeObject<Node[]>(this.NodesSerialized.ToString(), new JsonConverter[]
			{
				new NodeConverter(this.masterKey, ref this.sharedKeys)
			});
			this.UndecryptedNodes = (from x in source
			where x.EmptyKey
			select x).ToArray<Node>();
			this.Nodes = (from x in source
			where !x.EmptyKey
			select x).ToArray<Node>();
		}

		// Token: 0x0400072D RID: 1837
		private readonly byte[] masterKey;

		// Token: 0x0400072E RID: 1838
		private List<SharedKey> sharedKeys;
	}
}
