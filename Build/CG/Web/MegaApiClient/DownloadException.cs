using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000146 RID: 326
	public class DownloadException : Exception
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x0000D49F File Offset: 0x0000B69F
		public DownloadException() : base("Invalid file checksum")
		{
		}
	}
}
