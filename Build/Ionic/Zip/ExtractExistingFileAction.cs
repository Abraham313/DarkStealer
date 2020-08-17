using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000CE RID: 206
	[ComVisible(true)]
	public enum ExtractExistingFileAction
	{
		// Token: 0x0400027D RID: 637
		Throw,
		// Token: 0x0400027E RID: 638
		OverwriteSilently,
		// Token: 0x0400027F RID: 639
		DoNotOverwrite,
		// Token: 0x04000280 RID: 640
		InvokeExtractProgressEvent
	}
}
