using System;
using Newtonsoft.Json;

namespace CG.Web.MegaApiClient.Serialization
{
	// Token: 0x02000196 RID: 406
	internal class UploadUrlRequest : RequestBase
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
		public UploadUrlRequest(long fileSize) : base("u")
		{
			this.Size = fileSize;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
		[JsonProperty("s")]
		public long Size { get; private set; }
	}
}
