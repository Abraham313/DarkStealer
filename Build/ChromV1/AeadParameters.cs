using System;

namespace ChromV1
{
	// Token: 0x0200006A RID: 106
	public class AeadParameters : ICipherParameters
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00009B03 File Offset: 0x00007D03
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
		{
			this.Key = key;
			this.nonce = nonce;
			this.MacSize = macSize;
			this.associatedText = associatedText;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00009B28 File Offset: 0x00007D28
		public virtual KeyParameter Key { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009B30 File Offset: 0x00007D30
		public virtual int MacSize { get; }

		// Token: 0x06000240 RID: 576 RVA: 0x00009B38 File Offset: 0x00007D38
		public virtual byte[] GetAssociatedText()
		{
			return this.associatedText;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009B40 File Offset: 0x00007D40
		public virtual byte[] GetNonce()
		{
			return this.nonce;
		}

		// Token: 0x0400013F RID: 319
		private readonly byte[] associatedText;

		// Token: 0x04000140 RID: 320
		private readonly byte[] nonce;
	}
}
