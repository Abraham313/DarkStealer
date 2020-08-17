using System;
using System.Runtime.InteropServices;

namespace Echelon.Global.Other.WinStructs
{
	// Token: 0x02000066 RID: 102
	public struct SYSTEM_KERNEL_DEBUGGER_INFORMATION
	{
		// Token: 0x0400013B RID: 315
		[MarshalAs(UnmanagedType.U1)]
		public bool KernelDebuggerEnabled;

		// Token: 0x0400013C RID: 316
		[MarshalAs(UnmanagedType.U1)]
		public bool KernelDebuggerNotPresent;
	}
}
