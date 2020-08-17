using System;
using System.IO;

namespace CG.Web.MegaApiClient
{
	// Token: 0x0200016A RID: 362
	internal class MegaAesCtrStreamCrypter : MegaAesCtrStream
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x0000DC31 File Offset: 0x0000BE31
		public MegaAesCtrStreamCrypter(Stream stream) : base(stream, stream.Length, MegaAesCtrStream.Mode.Crypt, Crypto.CreateAesKey(), Crypto.CreateAesKey().CopySubArray(8, 0))
		{
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0000DC52 File Offset: 0x0000BE52
		public byte[] FileKey
		{
			get
			{
				return this.fileKey;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0000DC5A File Offset: 0x0000BE5A
		public byte[] Iv
		{
			get
			{
				return this.iv;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0000DC62 File Offset: 0x0000BE62
		public byte[] MetaMac
		{
			get
			{
				if (this.position != this.streamLength)
				{
					throw new NotSupportedException("Stream must be fully read to obtain computed FileMac");
				}
				return this.metaMac;
			}
		}
	}
}
