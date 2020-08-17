using System;
using System.Diagnostics;
using Echelon.Global.Other;
using Echelon.Global.Other.WinStructs;

namespace Echelon.Global.Protect
{
	// Token: 0x0200005F RID: 95
	internal class DebugProtect3
	{
		// Token: 0x0600023A RID: 570 RVA: 0x000229FC File Offset: 0x00020BFC
		public static void HideOSThreads()
		{
			foreach (object obj in Process.GetCurrentProcess().Threads)
			{
				ProcessThread processThread = (ProcessThread)obj;
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine("[Получение потоков]: thread.Id {0:X}", processThread.Id);
				IntPtr intPtr = NativeMethods.OpenThread(ThreadAccess.SET_INFORMATION, false, (uint)processThread.Id);
				if (intPtr == IntPtr.Zero)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("[Получение потоков]: порпуск thread.Id {0:X}", processThread.Id);
				}
				else
				{
					if (DebugProtect3.HideFromDebugger(intPtr))
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("[Получение потоков]: thread.Id {0:X} спрятан от дебаггера.", processThread.Id);
					}
					NativeMethods.CloseHandle(intPtr);
				}
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009AEE File Offset: 0x00007CEE
		public static bool HideFromDebugger(IntPtr Handle)
		{
			return NativeMethods.NtSetInformationThread(Handle, ThreadInformationClass.ThreadHideFromDebugger, IntPtr.Zero, 0) == NtStatus.Success;
		}
	}
}
