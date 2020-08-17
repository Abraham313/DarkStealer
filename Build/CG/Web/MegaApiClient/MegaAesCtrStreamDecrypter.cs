using System;
using System.IO;
using System.Linq;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200016B RID: 363
	internal class MegaAesCtrStreamDecrypter : MegaAesCtrStream
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x0000DC83 File Offset: 0x0000BE83
		public MegaAesCtrStreamDecrypter(Stream stream, long streamLength, byte[] fileKey, byte[] iv, byte[] expectedMetaMac) : base(stream, streamLength, MegaAesCtrStream.Mode.Decrypt, fileKey, iv)
		{
			if (expectedMetaMac == null || expectedMetaMac.Length != 8)
			{
				throw new ArgumentException("Invalid expectedMetaMac");
			}
			this.expectedMetaMac = expectedMetaMac;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0000DCAF File Offset: 0x0000BEAF
		protected override void OnStreamRead()
		{
			if (!this.expectedMetaMac.SequenceEqual(this.metaMac))
			{
				throw new DownloadException();
			}
		}

		// Token: 0x040006EC RID: 1772
		private readonly byte[] expectedMetaMac;
	}
}
