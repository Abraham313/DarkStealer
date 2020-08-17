using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000BC RID: 188
	[ComVisible(true)]
	public enum EncryptionAlgorithm
	{
		// Token: 0x04000253 RID: 595
		None,
		// Token: 0x04000254 RID: 596
		PkzipWeak,
		// Token: 0x04000255 RID: 597
		WinZipAes128,
		// Token: 0x04000256 RID: 598
		WinZipAes256,
		// Token: 0x04000257 RID: 599
		Unsupported
	}
}
