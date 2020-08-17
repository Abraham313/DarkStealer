using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000ED RID: 237
	[ComVisible(true)]
	public enum ZipEntrySource
	{
		// Token: 0x04000352 RID: 850
		None,
		// Token: 0x04000353 RID: 851
		FileSystem,
		// Token: 0x04000354 RID: 852
		Stream,
		// Token: 0x04000355 RID: 853
		ZipFile,
		// Token: 0x04000356 RID: 854
		WriteDelegate,
		// Token: 0x04000357 RID: 855
		JitStream,
		// Token: 0x04000358 RID: 856
		ZipOutputStream
	}
}
