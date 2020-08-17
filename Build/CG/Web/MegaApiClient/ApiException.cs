using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000145 RID: 325
	public class ApiException : Exception
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x0000D468 File Offset: 0x0000B668
		internal ApiException(ApiResultCode apiResultCode)
		{
			this.ApiResultCode = apiResultCode;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0000D477 File Offset: 0x0000B677
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0000D47F File Offset: 0x0000B67F
		public ApiResultCode ApiResultCode { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0000D488 File Offset: 0x0000B688
		public override string Message
		{
			get
			{
				return string.Format("API response: {0}", this.ApiResultCode);
			}
		}
	}
}
