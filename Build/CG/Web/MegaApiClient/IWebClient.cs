using System;
using System.IO;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000154 RID: 340
	public interface IWebClient
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000929 RID: 2345
		// (set) Token: 0x0600092A RID: 2346
		int BufferSize { get; set; }

		// Token: 0x0600092B RID: 2347
		string PostRequestJson(Uri url, string jsonData);

		// Token: 0x0600092C RID: 2348
		string PostRequestRaw(Uri url, Stream dataStream);

		// Token: 0x0600092D RID: 2349
		Stream GetRequestRaw(Uri url);
	}
}
