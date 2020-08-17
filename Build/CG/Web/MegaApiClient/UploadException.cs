using System;

namespace CG.Web.MegaApiClient
{
	// Token: 0x02000147 RID: 327
	public class UploadException : Exception
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x0000D4AC File Offset: 0x0000B6AC
		public UploadException(string error) : base("Upload error: " + error)
		{
		}
	}
}
