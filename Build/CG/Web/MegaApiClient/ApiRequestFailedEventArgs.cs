using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000142 RID: 322
	public class ApiRequestFailedEventArgs : EventArgs
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x0000D353 File Offset: 0x0000B553
		public ApiRequestFailedEventArgs(Uri url, int attemptNum, TimeSpan retryDelay, ApiResultCode apiResult, string responseJson) : this(url, attemptNum, retryDelay, apiResult, responseJson, null)
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000D363 File Offset: 0x0000B563
		public ApiRequestFailedEventArgs(Uri url, int attemptNum, TimeSpan retryDelay, ApiResultCode apiResult, Exception exception) : this(url, attemptNum, retryDelay, apiResult, null, exception)
		{
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000D373 File Offset: 0x0000B573
		private ApiRequestFailedEventArgs(Uri url, int attemptNum, TimeSpan retryDelay, ApiResultCode apiResult, string responseJson, Exception exception)
		{
			this.ApiUrl = url;
			this.AttemptNum = attemptNum;
			this.RetryDelay = retryDelay;
			this.ApiResult = apiResult;
			this.ResponseJson = responseJson;
			this.Exception = exception;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
		public Uri ApiUrl { get; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
		public ApiResultCode ApiResult { get; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		public string ResponseJson { get; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		public int AttemptNum { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		public TimeSpan RetryDelay { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		public Exception Exception { get; }
	}
}
