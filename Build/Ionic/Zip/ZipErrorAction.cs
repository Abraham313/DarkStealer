using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000EE RID: 238
	[ComVisible(true)]
	public enum ZipErrorAction
	{
		// Token: 0x0400035A RID: 858
		Throw,
		// Token: 0x0400035B RID: 859
		Skip,
		// Token: 0x0400035C RID: 860
		Retry,
		// Token: 0x0400035D RID: 861
		InvokeErrorEvent
	}
}
