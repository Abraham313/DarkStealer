using System;
using System.Diagnostics;

namespace Echelon.Global.Protect
{
	// Token: 0x0200005D RID: 93
	internal class DebugProtect1
	{
		// Token: 0x0600022F RID: 559 RVA: 0x00022808 File Offset: 0x00020A08
		public static int PerformChecks()
		{
			if (DebugProtect1.CheckDebuggerManagedPresent() == 1)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Обнаружен регулируемый дебаггер: HIT");
				return 1;
			}
			if (DebugProtect1.CheckDebuggerUnmanagedPresent() == 1)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Обнаружен нерегулируемый дебаггер: HIT");
				return 1;
			}
			if (DebugProtect1.CheckRemoteDebugger() == 1)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Обнаружен удаленный дебаггер: HIT");
				return 1;
			}
			return 0;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009AC9 File Offset: 0x00007CC9
		private static int CheckDebuggerManagedPresent()
		{
			if (Debugger.IsAttached)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009AD5 File Offset: 0x00007CD5
		private static int CheckDebuggerUnmanagedPresent()
		{
			if (NativeMethods.IsDebuggerPresent())
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00022868 File Offset: 0x00020A68
		private static int CheckRemoteDebugger()
		{
			bool flag = false;
			if (NativeMethods.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref flag) && flag)
			{
				return 1;
			}
			return 0;
		}
	}
}
