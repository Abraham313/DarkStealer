using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x020000F3 RID: 243
	[ComVisible(true)]
	public enum Zip64Option
	{
		// Token: 0x040003A8 RID: 936
		Default,
		// Token: 0x040003A9 RID: 937
		Never = 0,
		// Token: 0x040003AA RID: 938
		AsNecessary,
		// Token: 0x040003AB RID: 939
		Always
	}
}
