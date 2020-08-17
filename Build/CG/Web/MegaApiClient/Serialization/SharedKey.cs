using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000191 RID: 401
	[DebuggerDisplay("Id: {Id} / Key: {Key}")]
	internal class SharedKey
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		public SharedKey(string id, string key)
		{
			this.Id = id;
			this.Key = key;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0000E4C6 File Offset: 0x0000C6C6
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0000E4CE File Offset: 0x0000C6CE
		[JsonProperty("h")]
		public string Id { get; private set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0000E4D7 File Offset: 0x0000C6D7
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x0000E4DF File Offset: 0x0000C6DF
		[JsonProperty("k")]
		public string Key { get; private set; }
	}
}
